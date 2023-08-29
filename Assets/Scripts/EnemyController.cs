using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;
    private Transform target;

    public float damage;

    public float hitWaitTime = 1f;
    private float hitCounter;

    public float health = 5f;

    public float knockBackTime = .5f;
    private float knockBackCounter;

    public int expToGive = 1;

    public int coinValue = 1;

    public float coinDropRate = .5f;

    public float attackDistance = 1f;
    public float attackCooldown = 2f;

    public float checkPlayerInterval = 1f; // Time interval to check for the player

    private bool isAttacking;
    private float attackTimer;
    private float checkPlayerTimer; // Timer to track the interval between player checks

    private EnemyAnimator enemyAnimator;
    private SpriteRenderer spriteRenderer;

    private EnemyPooler enemyPooler; // Reference to the EnemyPooler script

    public static EnemyController instance;

    private void Awake()
    {
        instance = this;
        enemyAnimator = GetComponent<EnemyAnimator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        enemyPooler = FindObjectOfType<EnemyPooler>(); // Find and reference the EnemyPooler script
    }

    private void Start()
    {
        target = PlayerHealthController.instance.transform;
        checkPlayerTimer = checkPlayerInterval; // Initialize the player check timer
    }

    private void Update()
    {
        if (knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;
            if (moveSpeed > 0)
            {
                moveSpeed = -moveSpeed * 2f;
            }

            theRB.velocity = (target.position - transform.position).normalized * moveSpeed;

            if (knockBackCounter <= 0)
            {
                moveSpeed = Mathf.Abs(moveSpeed * .5f);
            }
        }
        else
        {
            // Update the player check timer
            checkPlayerTimer -= Time.deltaTime;

            if (checkPlayerTimer <= 0f)
            {
                // Reset the player check timer and check if the player is active
                checkPlayerTimer = checkPlayerInterval;
                if (!PlayerHealthController.instance.gameObject.activeSelf)
                {
                    // Player is disabled, disable the enemy
                    DisableEnemy();
                    return;
                }
            }

            theRB.velocity = (target.position - transform.position).normalized * moveSpeed;

            if (hitCounter > 0f)
            {
                hitCounter -= Time.deltaTime;
            }

            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            enemyAnimator.SetAttackDistance(distanceToTarget <= attackDistance);
            enemyAnimator.SetMoveSpeed(moveSpeed);

            // Check if within attack distance and not currently attacking
            if (distanceToTarget <= attackDistance && !isAttacking)
            {
                // Start attack
                isAttacking = true;
                attackTimer = attackCooldown;
                Attack();
            }

            // Check attack cooldown timer
            if (isAttacking)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0f)
                {
                    // Reset attack state
                    isAttacking = false;
                }
            }

            // Flip sprite based on movement direction
            if (theRB.velocity.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (theRB.velocity.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && hitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);
            hitCounter = hitWaitTime;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        health -= damageToTake;

        if (health <= 0)
        {
            Die();
        }

        DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);
        SFXManager.instance.PlaySFXPitched(1);
    }

    public void TakeDamage(float damageToTake, bool shouldKnockback)
    {
        TakeDamage(damageToTake);

        if (shouldKnockback)
        {
            knockBackCounter = knockBackTime;
        }
    }

    
    
    private void Die()
    {
        enemyAnimator.PlayDeathAnimation();
        //enemyAnimator.PlayDeathSound(); // Add this line to play the death sound
        enemyAnimator.SpawnDeathEffect(transform.position);

        SFXManager.instance.PlaySFXPitched(0);
        // Disable the enemy using the EnemyPooler script
        enemyPooler.DisableEnemy(gameObject);
        

        ExperianceLevelController.instance.SpawnExp(transform.position, expToGive);

        if (Random.value <= coinDropRate)
        {
            CoinController.instance.DropCoin(transform.position, coinValue);
        }
    }
    
    
    /*private void Die()
    {
        enemyAnimator.PlayDeathAnimation();
        enemyAnimator.SpawnDeathEffect(transform.position);

        // Disable the enemy using the EnemyPooler script
        enemyPooler.DisableEnemy(gameObject);

        ExperianceLevelController.instance.SpawnExp(transform.position, expToGive);

        if (Random.value <= coinDropRate)
        {
            CoinController.instance.DropCoin(transform.position, coinValue);
        }
        
    }*/


    private void Attack()
    {
        // Perform attack logic here
        // For example, deal damage to the player or trigger an attack animation
        PlayerHealthController.instance.TakeDamage(damage);
    }

    public void DisableEnemy()
    {
        // Play death animation
        enemyAnimator.PlayDeathAnimation();

        // Spawn death effect
        enemyAnimator.SpawnDeathEffect(transform.position);

        // Disable the enemy using the EnemyPooler script
        enemyPooler.DisableEnemy(gameObject);
    }

}