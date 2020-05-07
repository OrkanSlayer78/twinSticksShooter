using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{


    public Rigidbody2D theRB;
    public float moveSpeed;

    public bool shouldChasePlayer;

    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    public bool shouldRunAway;
    public float runawayRange;

    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;


    public Animator anim;

    public int health = 150;
    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    public bool shouldShoot;
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    public float fireCounter;

    public SpriteRenderer enemyBody;

    public float shootRange;

    // Start is called before the first frame update
    void Start()
    {
        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else if(shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runawayRange)
            {
                moveDirection = transform.position - PlayerController.instance.transform.position;
            }
            else
            {
                if (shouldWander)
                {
                    if(wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;
                        moveDirection = wanderDirection;
                        
                    if(wanderCounter < 0)
                        {
                            pauseCounter = Random.Range(pauseLength* .75f, pauseLength * 1.25f);
                        }
                    }
                    if(pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;
                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);
                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                        }
                    }
                }

                if (shouldPatrol)
                {
                    moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;
                    if(Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position)< .1f)
                    {
                        currentPatrolPoint++;
                        if(currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                }
            }








            moveDirection.Normalize();

            theRB.velocity = moveDirection * moveSpeed;

            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position)< shootRange)
            {
                fireCounter -= Time.deltaTime;
                if (fireCounter <= 0)
                {
                    fireCounter = fireRate;
                    Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
                    AudioManager.instance.PlaySFX(14);

                }
            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        AudioManager.instance.PlaySFX(2);
        Instantiate(hitEffect, transform.position, transform.rotation);

        if(health <= 0)
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(1);

            int selectedSplatter = Random.Range(0, deathSplatters.Length);
            int rotation = Random.Range(0, 4);

            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f ,0f , rotation*90f));
        }
    }
}
