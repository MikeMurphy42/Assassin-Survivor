using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapons2 : Weapon
{
    public EnemyDamager damager;
    public Projectile projectile;
    

    private float shotCounter;
    public float weaponRange;
    public LayerMask whatIsEnemy;

    // How many times per second to perform the check
    public float checkFrequency;
    private float checkCounter;

    // Variable to toggle distance checking
    public bool checkDistance;

    void Start()
    {
        SetStats();
    }

    void Update()
    {
        
        
        if (statsUpdated)
        {
            statsUpdated = false;
            SetStats();
        }

        shotCounter -= Time.deltaTime;
        checkCounter -= Time.deltaTime;

        if (checkCounter <= 0)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange * stats[weaponLvl].range, whatIsEnemy);

            if (enemies.Length > 0)
            {
                // Reset check counter
                checkCounter = 1f / checkFrequency;

                // If it's time to shoot
                if (shotCounter <= 0)
                {
                    shotCounter = stats[weaponLvl].timeBetweenAttacks;

                    // If distance checking is enabled
                    if (checkDistance)
                    {
                        List<Collider2D> sortedEnemies = SortEnemiesByDistance(enemies);

                        // Attack enemies in order of distance
                        for (int i = 0; i < Mathf.Min(stats[weaponLvl].amount, sortedEnemies.Count); i++)
                        {
                            Vector3 enemyPosition = sortedEnemies[i].transform.position;
                            ShootProjectile(enemyPosition);
                            SFXManager.instance.PlaySFXPitched(6);
                        }
                    }
                    else
                    {
                        // Random attacks
                        for (int i = 0; i < stats[weaponLvl].amount; i++)
                        {
                            Vector3 randomEnemyPosition = enemies[Random.Range(0, enemies.Length)].transform.position;
                            ShootProjectile(randomEnemyPosition);
                            SFXManager.instance.PlaySFXPitched(6);
                        }
                    }
                }
            }
        }
    }

    List<Collider2D> SortEnemiesByDistance(Collider2D[] enemies)
    {
        List<Collider2D> sortedEnemies = new List<Collider2D>(enemies);
        sortedEnemies.Sort((a, b) => (transform.position - a.transform.position).sqrMagnitude.CompareTo((transform.position - b.transform.position).sqrMagnitude));
        return sortedEnemies;
    }

    void ShootProjectile(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Instantiate(projectile, transform.position, rotation).gameObject.SetActive(true);
    }

    void SetStats()
    {
        damager.damageAmount = stats[weaponLvl].damage;
        damager.lifeTime = stats[weaponLvl].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLvl].range;
        shotCounter = 0f;
        projectile.moveSpeed = stats[weaponLvl].speed;

        // Initialize the check counter
        checkCounter = 1f / checkFrequency;
    }
}
