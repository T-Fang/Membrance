using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/**
 *  This guy creates all the different states, that inherits from PlayerState
 */
public class Player : MonoBehaviour
{
    //public static Player SingoTone { get; private set; }

    #region FSM stuffs
    // State Machine Stuffs
    public PlayerFSM StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirborneState AirborneState { get; private set; }
    public PlayerTouchDownState TdState { get; private set; }
    public PlayerWallGrabState WGrabState { get; private set; }
    public PlayerWallClimbState WClimbState { get; private set; }
    public PlayerWallJumpState WJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerAttackState SlashAttackState { get; private set; }
    public PlayerRangedAttackState DoMagicState { get; private set; }
    public PlayerAttackState UpSlashAttackState { get; private set; }
    public PlayerDeathState DeathState { get; private set; }
    public PlayerSuccState SuccAbilityState { get; private set; }
    public PlayerAbsorbState AbsorbState { get; private set; }
    public PlayerChangeBackState BackToDefaultState { get; private set; }

    // Player (ref) input, states (ref) player --> states (can access) input
    [SerializeField] public PlayerData data;
    public PlayerArsenal Arsernal { get; set; }
    #endregion

    #region Unity References

    public Animator Animator { get; private set; }
    public PlayerInputHandler Ih { get; private set; }
    public Rigidbody2D BodyRef { get; private set; }
    public Vector2 CurrVel { get; set; }
    public Vector2 CurrPos { get; private set; }
    public GameMaster gameMaster { get; private set; }
    public AudioManager AudioManager { get; private set; }

    public Health playerHealth { get; set; }

    // GUN STUFF HACK IN FOR NOW
    public PsycheBar psycheBar;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    #endregion

    #region Helper vars
   private Vector2 _helperVec; // So we don't call new Vector2() all the time
   public int FacingDir { get; private set; }
   public int CurrentSanity {get; private set;}
   public bool VelocitySettable;
   //private AbilityLoot Tester;
   public LootVacDetector LootVacDetector;
   public WeaponList WeaponList;
   [SerializeField] private int _characterId;
   public static int TypeOfAttack; // Another hack for weaponing
   public int absorbedId { get; private set; }

   #endregion
   

    #region  Physics helpers

   [SerializeField] private Transform groundBeepBeep;
   [SerializeField] private Transform wallBeepBeep;
   #endregion

   public SimpleFlash Flickerer;
   public bool InIFrame;
   public SpriteRenderer SpriteRenderer;
   public Sprite CurrentSprite;
   public static bool IsAbsorbing; // TODO: Potential game crashing on level reload
   public GameObject AbsorbedPlayer;
   public AbilityLoot DefaultPlayer;
   private string MagicId;
   private float MagicCd;
   public MagicStatsData magicdata;
   public AbilityHolder2 DashHolder;
   public GameObject Magic;


   #region recover from absorb

   private Sprite _defaultSprite;
   private RuntimeAnimatorController _defaultSlashBase;
   private RuntimeAnimatorController _defaultSlashWeapon;
   private Vector3 _defaultSlashScale;
   private RuntimeAnimatorController _defaultUpSlashBase;
   private RuntimeAnimatorController _defaultUpSlashWeapon;
   private Vector3 _defaultUpSlashScale;
   private int _defaultUniqueId;
   private string _defaultMagicId;
   private float _defaultMagicCd;
   private GameObject _defaultMagic;
   private MagicStatsData _defaultMagicData;
   private bool _isInDefault;
   public GameObject ChangeBackParticle;
   public static bool FinishChangeBack;
   #endregion
   
   #region Unity Events
    private void Awake()
    {
        // FML finally fix the null references?
        /*
        if (SingoTone != null)
		{
			Destroy(gameObject);
		}
		else
		{
			SingoTone = this;
			DontDestroyOnLoad(gameObject);
		}
		*/
        
        // Call this in awake to make sure FSM is up before everyone else
        StateMachine = new PlayerFSM();
        IdleState = new PlayerIdleState(this, StateMachine, data, "idle");
        RunState = new PlayerRunState(this, StateMachine, data, "run");
        JumpState = new PlayerJumpState(this, StateMachine, data, "airborne");
        AirborneState = new PlayerAirborneState(this, StateMachine, data, "airborne");
        TdState = new PlayerTouchDownState(this, StateMachine, data, "touchdown");
        WClimbState = new PlayerWallClimbState(this, StateMachine, data, "wallClimb");
        WGrabState = new PlayerWallGrabState(this, StateMachine, data, "wallGrab");
        WJumpState = new PlayerWallJumpState(this, StateMachine, data, "airborne");
        DashState = new PlayerDashState(this, StateMachine, data, "airborne"); // reusing fly animation
        SlashAttackState = new PlayerAttackState(this, StateMachine, data, "attack");
        DoMagicState = new PlayerRangedAttackState(this, StateMachine, data, "airborne");
        UpSlashAttackState = new PlayerAttackState(this, StateMachine, data, "attack");
        DeathState = new PlayerDeathState(this, StateMachine, data, "dead");
        SuccAbilityState = new PlayerSuccState(this, StateMachine, data, "dead");
        AbsorbState = new PlayerAbsorbState(this, StateMachine, data, "transform");
        BackToDefaultState = new PlayerChangeBackState(this, StateMachine, data, "transform");
    }

    private void Start()
    {
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        /*
        Tester = GameObject.FindGameObjectWithTag("Loots").GetComponent<AbilityLoot>();
        Tester.gameObject.SetActive(true);
        */
        playerHealth = GetComponent<Health>();
        Animator = GetComponent<Animator>();
        Ih = GetComponent<PlayerInputHandler>();
        BodyRef = GetComponent<Rigidbody2D>();
        Arsernal = GetComponent<PlayerArsenal>();
        AudioManager = FindObjectOfType<AudioManager>();
        absorbedId = 0; // default self
        InitAbilityHolders();

        _defaultSprite = GetComponent<SpriteRenderer>().sprite;
       _defaultSlashBase = transform.Find("Weapons/Sword/Base").GetComponent<Animator>().runtimeAnimatorController ;
       _defaultSlashWeapon = transform.Find("Weapons/Sword/Weapon").GetComponent<Animator>().runtimeAnimatorController ;
       _defaultSlashScale = transform.Find("Weapons/UpSword/Weapon").transform.localScale;
       _defaultUpSlashBase = transform.Find("Weapons/UpSword/Base").GetComponent<Animator>().runtimeAnimatorController ;
       _defaultUpSlashWeapon = transform.Find("Weapons/UpSword/Base").GetComponent<Animator>().runtimeAnimatorController ;
       _defaultUpSlashScale = transform.Find("Weapons/UpSword/Weapon").transform.localScale;
       _defaultUniqueId = _characterId; // should i just hardcode this
       _defaultMagicId = data.MagicId;
       _defaultMagicCd = data.MagicCd;
       _defaultMagicData = magicdata;

        FacingDir = 1;
        VelocitySettable = true;
        CurrentSanity = data.MaxSanity;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SlashAttackState.SetCurrWeapon(Arsernal.WeaponArr[(int)AttackInputs.slash]);
        //GunAttackState.SetCurrWeapon(Arsernal.WeaponArr[(int)AttackInputs.opGun]);
        UpSlashAttackState.SetCurrWeapon(Arsernal.WeaponArr[(int)AttackInputs.upSlash]);
        StateMachine.Init(IdleState); // first state is always idle

        if (gameMaster.lastCheckPointPos != Vector2.zero)
        {
            Debug.Log("Respawning at the last checkpoint");
            transform.position = gameMaster.lastCheckPointPos;
        }
    }

    /*
    private void OnEnable()
    {
        SingoTone = this;
    }

    private void OnDisable()
    {
        SingoTone = null;
    }
    */

    private void Update()
    {
        CurrVel = BodyRef.velocity;
        CurrPos = transform.position;
        StateMachine.CurrState.UpdateLogic();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrState.UpdatePhysics();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundBeepBeep.position, data.beepBeepTolerance);
        Gizmos.DrawLine(wallBeepBeep.position,
            new Vector3(wallBeepBeep.position.x + data.wallBeepTolerance, wallBeepBeep.position.y, wallBeepBeep.position.z));
        Vector3 center = SpriteRenderer.bounds.center;
        float radius = SpriteRenderer.bounds.extents.magnitude;
        Gizmos.DrawWireSphere(center,radius);
    }

    // Quick hack to integrate dash as ability. TODO: Find permanent fix
    private void InitAbilityHolders()
    {
        AbilityHolder2[] AllHolders = GetComponents<AbilityHolder2>();
        foreach(AbilityHolder2 Holder in AllHolders) {
            if (Holder.ability.name.Contains("Dash"))
            {
                DashHolder = Holder;
                break;
            }
        }
    }


    #endregion

    #region Setters
    public void SetVelX(float vel)
    {
        //if (vel == 0.0f) return;
        _helperVec.Set(vel, CurrVel.y);
        //BodyRef.MovePosition(CurrPos + _helperVec * Time.deltaTime);
        SetFinalVelocity();
    }


    public void Jump(float vel)
    {
        /*
         TODO: Get MovePosition to work with jumps
         I don't even know how to get this to work
        _helperVec.Set(CurrVel.x, vel+10*5*Time.unscaledDeltaTime);
        Vector2 newPos = _helperVec * Time.unscaledDeltaTime;
        //BodyRef.MovePosition(Vector2.Lerp(CurrPos,CurrPos+newPos,5.0f));
        BodyRef.MovePosition(CurrPos+_helperVec*Time.unscaledDeltaTime);
        */
        
        _helperVec.Set(CurrVel.x,vel);
        SetFinalVelocity();
        
        // This produce least "laggy" jump
        //BodyRef.AddForce(Vector2.up * data.jumpForce, ForceMode2D.Impulse);
    }

    // DUPES of Jump, but better name
    public void SetVelY(float vel)
    {
       _helperVec.Set(CurrVel.x,vel);
       SetFinalVelocity();
    }

    public void SetVelAtAngle(float vel, Vector2 angle, int dir)
    {
        angle.Normalize();
        _helperVec.Set(angle.x*vel*dir,angle.y*vel);
        SetFinalVelocity();
    }

    // Overload for dashing
    public void SetVelAtAngle(float vel, Vector2 dir)
    {
        _helperVec = dir * vel; // scale direction to match dash vel
        SetFinalVelocity();
    }

    private void SetFinalVelocity()
    {
        if (!VelocitySettable) return;
        BodyRef.velocity = CurrVel = _helperVec;
    }

    public void SetAbsorbedId(int id) => absorbedId = id;

    public void SetMagicInfo(string id, float cd, GameObject magic)
    {
        MagicId = id;
        MagicCd = cd;
        Magic = magic; // TODO: type check this
    }

    public static void SetFinishChangeBack() => FinishChangeBack = true;

    public void ShootFireball()
    {
        if (CurrentSanity <= 0) return;
        //cooldownTimer = 0;
        fireballs[0].transform.position = firePoint.position;
        fireballs[0].GetComponent<Projectile>().SetDirection(FacingDir);
    }

    public void mentalDown(int psycho)
    {
        CurrentSanity -= psycho;
        psycheBar.setSanity(CurrentSanity);
    }
    
    public void MakeInvincible()
    {
        InIFrame = true;
        StartCoroutine(InFrame());

    }

    public static void SetCurrentTypeOfAttack(int attackId) => TypeOfAttack = attackId;

    // TODO: Potential game crashing, find a better way than this hax
    public static void SetFinishAbsorbing()
    {
        IsAbsorbing = false;
    }

    public IEnumerator InFrame()
    {
        /*
        Physics2D.IgnoreLayerCollision(3,12,true); // Spikes
        */
        Physics2D.IgnoreLayerCollision(3,7,true); // Enemies

        //Flickerer.Flash();
        for (int i = 0; i < data.numOfFlickers; ++i)
        {
            SpriteRenderer.color = new Color(1.0f,1.0f,1.0f,1.0f);
            yield return new WaitForSeconds(data.iFrame/(data.numOfFlickers*2));
            SpriteRenderer.color = new Color(1.0f,1.0f,1.0f,0.5f);
            yield return new WaitForSeconds(data.iFrame/(data.numOfFlickers*2));
        }
        
        Physics2D.IgnoreLayerCollision(3,7,false); // Enemies
        /*
        Physics2D.IgnoreLayerCollision(3,12,false); // Spikes
        */
        InIFrame = false;
        SpriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0);
    }

    private float _physicsScale = 0.01f;
    public void BeginSloMo(float timeScale)
    {
        FindObjectOfType<AudioManager>().Play("SlowMotionStart");
        data.dashVel /= timeScale;
        data.dragForce /= timeScale;
        _helperVec /= timeScale;
        data.walkingVel /= timeScale;
        data.jumpVel /= timeScale;
        BodyRef.velocity /= timeScale;
        BodyRef.gravityScale /= _physicsScale;
        magicdata.ProjectileSpeed /= timeScale;
    }
    public void EndSloMo(float timeScale)
    {
        FindObjectOfType<AudioManager>().Play("SlowMotionEnd");
        data.dashVel *= timeScale;
        data.dragForce *= timeScale;
        _helperVec *= timeScale;
        data.walkingVel *= timeScale;
        data.jumpVel *= timeScale;
        BodyRef.velocity *= timeScale;
        BodyRef.gravityScale *= _physicsScale;
        magicdata.ProjectileSpeed *= timeScale;
    }

    public void BeginBigSordForm()
    {
        //TODO: Finish this
    }

    #endregion

    #region Impure,dirty predicates
    public bool FlipIfNeeded(int xInput)
    {
        // if there isn't directional input OR direction equates current facing, quit
        if (xInput == 0 || xInput == FacingDir) return false;

        // else flip
        FacingDir *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
        return true;
    }

    public bool IsOnTheGround()
    {
        return Physics2D.OverlapCircle(groundBeepBeep.position, data.beepBeepTolerance, data.groundLabel);
    }

    public bool IsKissingWall()
    {
        return Physics2D.Raycast(wallBeepBeep.position, Vector2.right * FacingDir, data.wallBeepTolerance,
            data.groundLabel);
    }

    public bool IsBackKissingWall()
    {
        return Physics2D.Raycast(wallBeepBeep.position, Vector2.right * -FacingDir, data.wallBeepTolerance,
            data.groundLabel);
    }

    public int UniqueId()
    {
        return _characterId;
    }

    public bool IsInDefault()
    {
        return _isInDefault;
    }

    public void SetIfIsInDefault(bool val) => _isInDefault = val;


    #endregion

    private void TriggerAnimFromPlayer() => StateMachine.CurrState.TriggerAnimation();
    private void TriggerFinishAnimFromPlayer() => StateMachine.CurrState.TriggerFinishAnimation(); 
    //public void TriggerFinishSmoke() => IsAbsorbing = true;
}
