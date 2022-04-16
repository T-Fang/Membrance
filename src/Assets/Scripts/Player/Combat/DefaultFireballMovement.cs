using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultFireballMovement : MonoBehaviour
{
    // TODO: Remove static hack and find better solution
    public float ProjectileSpeed;

    public GameObject impactEffect;
    public GameObject muzzleEffect;
	public List<GameObject> trails;
	private bool collided;

    private static Rigidbody2D BodyRef;

    public static void SetSpeed(float timeScale, bool beginning)
    {
        if (beginning) BodyRef.velocity /= timeScale;
        else BodyRef.velocity *= timeScale;
    }

    private float _lifeSpan = 5.0f;
    private float _startTime;

    void Start()
    {
        BodyRef = GetComponent<Rigidbody2D>();
        BodyRef.velocity = transform.right * ProjectileSpeed;
        var muzzleVFX = Instantiate (muzzleEffect, transform.position, Quaternion.identity);
        _startTime = Time.unscaledTime;
        var ps = muzzleVFX.GetComponent<ParticleSystem>();
        if (ps != null)
            Destroy (muzzleVFX, ps.main.duration);
        else
        {
            var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy(muzzleVFX, psChild.main.duration);
        }
    }

    void Update()
    {
        if (_startTime + _lifeSpan <= Time.unscaledTime)
        {
            Destroy(gameObject);
        }
    }

    // TODO: Object pooling these things
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<HittableObject>() != null && !collided)
        {
            collided = true;

            if (trails.Count > 0)
            {
                for (int i = 0; i < trails.Count; i++)
                {
                    trails[i].transform.parent = null;
                    var ps = trails[i].GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        ps.Stop();
                        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                    }
                }
            }
        }

        if (col.GetComponent<HittableObject>() != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
            //Destroy(col.gameObject);
            col.GetComponent<HittableObject>().Damage(45);
            // TODO: Generalize to other magic sound
            GameObject.FindObjectOfType<AudioManager>().Play("FireImpact");
            var ps = impactEffect.GetComponent<ParticleSystem>();
            if (ps == null)
            {
                var psChild = impactEffect.transform.GetChild(0).GetComponent<ParticleSystem>();
                //Destroy(impactEffect, psChild.main.duration);
            }
            else
                Destroy(impactEffect, ps.main.duration);
            StartCoroutine(DestroyParticle(0f));
        }
    }
	public IEnumerator DestroyParticle (float waitTime) {

		if (transform.childCount > 0 && waitTime != 0) {
			List<Transform> tList = new List<Transform> ();

			foreach (Transform t in transform.GetChild(0).transform) {
				tList.Add (t);
			}		

			while (transform.GetChild(0).localScale.x > 0) {
				yield return new WaitForSeconds (0.01f);
				transform.GetChild(0).localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				for (int i = 0; i < tList.Count; i++) {
					tList[i].localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				}
			}
		}
		
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
	}
}
