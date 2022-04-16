using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Serialization;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class PlayerMovementTest : MonoBehaviour
{

    private Rigidbody2D _bodyRef;
    //[SerializeField] private PlayerData _data;
    
    #region movement variables
    //// moving
    //private float _inputDirection;
    //private bool _facingRight = true;
    
    //// Jumping
    //private bool _onTheGround = true;
    //private int _jumpsRemaining;

    public Animator animator;
    
    // moving
    private float _inputDirection;
    private bool _facingRight = true;
    [SerializeField] public float speed = 5.0f;
    private int _facingDir;

    // Jumping
    [SerializeField] private float verts = 10.0f;
    [SerializeField] public int maxJumps = 2;
    public bool _onTheGround = true;

    public int _jumpsRemaining;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool _canJump;
    // Attack
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private Animator anim;
    private PlayerMovementTest playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    public int MaxSanity = 100;
    public int CurrentSanity;
    public PsycheBar psycheBar;

    public float airResistance = 1.0f; // Change this to 1 for "momentum" effect when jumping, < 1.0f for snap stop
    public float jumpHeightFactor = 0.5f;
    [SerializeField] public float airForce = 5.0f;

    // Wall Sliding + Jumping
    private bool _isAgainstWall;
    public Transform wallCheck;
    public float wallCheckdistance;
    private bool _isSliding;
    public float wallSlideSpeed = 0.0f;
    public Vector2 directionJumpWall;
    public float wallJumpForce;


    // Dashing
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashSpeed = 20.0f;
    [SerializeField] private float imageSeperation = 0.1f;
    [SerializeField] private float dashTimeCD = 1.0f;
    private float _afterImageLastPos;
    private float _dashTimeLeft;
    private float _dashTimeTracker;
    private bool _isDashing = false;
    private bool _allowMove = true; // Prevent additional movement when dashing
    #endregion

    // Checkpoint and respawning system
    private GameMaster gameMaster;
    private Health health;
    private bool isDying = false;

    private void Awake()
    {
        _bodyRef = GetComponent<Rigidbody2D>();
        //_jumpsRemaining = _data.maxJumps;
        _jumpsRemaining = maxJumps;
        _facingDir = 1; //right by default
        CurrentSanity = MaxSanity;

        playerMovement = GetComponent<PlayerMovementTest>();
        directionJumpWall.Normalize();
    }

    private void Start()
    {
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        Debug.Log("last check point: " + gameMaster.lastCheckPointPos);
        if (gameMaster.lastCheckPointPos != Vector2.zero)
        {
            Debug.Log("Respawning at the last checkpoint at: " + gameMaster.lastCheckPointPos);
            transform.position = gameMaster.lastCheckPointPos;
        }

        health = GetComponent<Health>();
    }
    private void FixedUpdate()
    {
        check_for_ground();
        check_ground_and_wall();
    }

    private void Update()
    {
        if (health.isDead)
        {
            if (!isDying)
            {
                isDying = true;
                Die();
            }
            return;
        }
        // move forward
        check_input();

        switch (_onTheGround)
        {
            // condition ensures jump momentum won't stop when directional key is let go
            case true:
                _bodyRef.velocity = new Vector2(_inputDirection * speed, _bodyRef.velocity.y);
                animator.SetBool("IsJumping", false);
                animator.SetFloat("Speed", Mathf.Abs(_inputDirection));
                break;
            case false when !_isSliding && _inputDirection != 0:
            //{
                //// Airborne only allow left-right control
                //_bodyRef.AddForce(new Vector2(airForce*_inputDirection,0));
                //if (_data.walkingVel < Mathf.Abs(_bodyRef.velocity.x))
                //{
                    //_bodyRef.velocity = new Vector2(_data.walkingVel * _inputDirection, _bodyRef.velocity.y);
                //}
                {
                    // Airborne only allow left-right control
                    _bodyRef.AddForce(new Vector2(airForce * _inputDirection, 0));
                    if (speed < Mathf.Abs(_bodyRef.velocity.x))
                    {
                        _bodyRef.velocity = new Vector2(speed * _inputDirection, _bodyRef.velocity.y);
                    }

                    break;
                }
            case false when !_isSliding && _inputDirection == 0 && _allowMove:
                {
                    _bodyRef.velocity = new Vector2(_bodyRef.velocity.x * airResistance, _bodyRef.velocity.y);
                    break;
                }
        }

        // turning
        handle_turn();

        // jumping
        check_jumpable();


        // wall sliding
        handle_wall_slide();

        if (_isDashing)
        {
            if (0 < _dashTimeLeft)
            {
                _bodyRef.velocity = new Vector2(dashSpeed * _inputDirection, _bodyRef.velocity.y);
                _dashTimeLeft -= Time.unscaledDeltaTime;
                if (imageSeperation < Mathf.Abs(transform.position.x - _afterImageLastPos))
                {
                    AfterImagePool.SingleTonInstance.grab_from_pool();
                    _afterImageLastPos = transform.position.x;
                }
            }

            else
            {
                _isDashing = false;
            }
        }
        if (Input.GetMouseButton(1) && cooldownTimer > attackCooldown)
        {

            Attack();
            mentalDown(25);
        }
        cooldownTimer += Time.unscaledDeltaTime;

    }

    private void Die()
    {
        Debug.Log("Player is dead");
        animator.SetBool("IsDead", true);
        gameMaster.ReloadLevel();
        animator.SetBool("IsDead", false);
    }
    private void dash()
    {
        _isDashing = true;
        _allowMove = false;
        _dashTimeLeft = dashTime;
        _dashTimeTracker = Time.time;

        AfterImagePool.SingleTonInstance.grab_from_pool();
        _afterImageLastPos = transform.position.x;
    }

    private void check_input()
    {
        _inputDirection = Input.GetAxisRaw("Horizontal"); // a,left = -1; d,right = 1
        if (Input.GetButtonDown("Jump") && (_onTheGround || 0 < _jumpsRemaining)) handle_jump();

        // Handle different jump height when holding vs tapping
        if (Input.GetButtonUp("Jump"))
        {
            _bodyRef.velocity = new Vector2(_bodyRef.velocity.x, _bodyRef.velocity.y * jumpHeightFactor);
        }

        if (Input.GetButtonDown("Dash"))
        {
            Debug.unityLogger.Log("Dashing");
            // Can only dash off cooldown
            if (_dashTimeTracker + dashTimeCD <= Time.time) dash();
        }
    }

    private void handle_turn()
    {
        if (_facingRight && _inputDirection < 0 || !_facingRight && 0 < _inputDirection)
        {
            // facing right, input left --> flip
            // facing left, input right --> flip
            _facingDir *= -1;
            _facingRight = !_facingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        // sliding condition important for wall jump
        if (_isSliding || (!_facingRight || !(_inputDirection < 0)) && (_facingRight || !(0 < _inputDirection))) return;
        // facing right, input left --> flip
        // facing left, input right --> flip
        _facingRight = !_facingRight;
        _facingDir *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void handle_jump()
    {
        if (!_canJump) return;

        // No arrow key pressed, character get pushed off wall
        if ((_isSliding || _isAgainstWall) && _inputDirection != 0)
        {
            // Wall jumping 
            _isSliding = false;
            _jumpsRemaining--;
            _bodyRef.AddForce(new Vector2(wallJumpForce * directionJumpWall.x * _inputDirection, wallJumpForce * directionJumpWall.y), ForceMode2D.Impulse);
        }
        else if (!_isSliding)
        {
            // Normal Jumping
            _bodyRef.velocity = new Vector2(_bodyRef.velocity.x, speed);
            Debug.unityLogger.Log("Doing decrement");
            _jumpsRemaining -= 1;
            animator.SetBool("IsJumping", true);
        }
    }

    private void check_jumpable()
    {
        //if (_onTheGround && _bodyRef.velocity.y <= 0.001f || _isSliding) _jumpsRemaining = _data.maxJumps;
        if (_onTheGround && _bodyRef.velocity.y <= 0.001f) _jumpsRemaining = maxJumps;
        _canJump = 0 < _jumpsRemaining;
    }

    private void check_for_ground()
    {
        _onTheGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (_onTheGround && _bodyRef.velocity.y <= 0.001f || _isSliding) _jumpsRemaining = maxJumps;
        _canJump = 0 < _jumpsRemaining;
    }

    private void handle_wall_slide()
    {
        // state is sliding against wall if character is above ground, against wall and not airborne
        _isSliding = (_isAgainstWall && !_onTheGround && _bodyRef.velocity.y < 0.001f);
        // Slow down when sliding against wall
        if (_isSliding && _bodyRef.velocity.y < -wallSlideSpeed)
        {
            _bodyRef.velocity = new Vector2(0.0f, -wallSlideSpeed);
        }
    }

    private void check_ground_and_wall()
    {
        _onTheGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        _isAgainstWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckdistance, whatIsGround);
    }

    // To visualize the ground checker (The circle with "+")
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        var wallCheckPosition = wallCheck.position;
        Gizmos.DrawLine(wallCheckPosition,
            new Vector3(wallCheckPosition.x + wallCheckdistance, wallCheckPosition.y, wallCheckPosition.z));
    }

    private void Attack()
    {
        //anim.SetTrigger("GunAttack");
        if (CurrentSanity > 0)
        {
            cooldownTimer = 0;
            fireballs[0].transform.position = firePoint.position;
            fireballs[0].GetComponent<Projectile>().SetDirection(_facingDir);
            Debug.Log("Attack");
        }
    }

    void mentalDown(int psycho)
    {
        CurrentSanity -= psycho;
        psycheBar.setSanity(CurrentSanity);
    }

    private float physicsScale = 0.01f;
    public void BeginSloMo(float timeScale)
    {
        speed /= timeScale;
        verts /= timeScale;
        airForce /= physicsScale;
        _bodyRef.velocity /= timeScale;
        _bodyRef.gravityScale /= physicsScale;
    }
    public void EndSloMo(float timeScale)
    {

        speed *= timeScale;
        verts *= timeScale;
        airForce *= physicsScale;
        _bodyRef.velocity *= timeScale;
        _bodyRef.gravityScale *= physicsScale;
    }
}
