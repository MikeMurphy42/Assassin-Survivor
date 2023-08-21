using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAttackWeapon : Weapon
{
    public EnemyDamager damager;
    private float attackCounter, direction;

    public Transform holder;
    public float[] offsets;  // set offset for each weapon

    private float lastDirection;

    void Start()
    {
        SetStats();
    }

    void Update()
    {
        holder.rotation = Quaternion.Euler(0f, 0f, 0f);

        if (statsUpdated == true)
        {
            statsUpdated = false;
            SetStats();
        }

        attackCounter -= Time.deltaTime;

        direction = Input.GetAxisRaw("Horizontal");

        // store the last direction
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            lastDirection = direction;
        }

        if (attackCounter <= 0)
        {
            attackCounter = stats[weaponLvl].timeBetweenAttacks;

            Quaternion spawnRotation;
            if(lastDirection < 0) // if last direction pressed was left
            {
                spawnRotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else // if last direction pressed was right
            {
                spawnRotation = Quaternion.identity;
            }

            Instantiate(damager, damager.transform.position, spawnRotation, holder.transform).gameObject.SetActive(true);
            SFXManager.instance.PlaySFXPitched(7);

            float rotStep = 90f;
            
            for (int i = 1; i < stats[weaponLvl].amount; i++)
            {
                float rot;
                if(i < offsets.Length)
                {
                    rot = offsets[i];
                }
                else
                {
                    rot = rotStep * i; // if no offset is set, use default distribution
                }

                Quaternion newSpawnRotation;
                if(lastDirection < 0) 
                {
                    newSpawnRotation = Quaternion.Euler(0f, 0f, 180f - rot);
                }
                else 
                {
                    newSpawnRotation = Quaternion.Euler(0f, 0f, rot);
                }

                Instantiate(damager, damager.transform.position, newSpawnRotation, holder.transform).gameObject.SetActive(true);
                
                //SFXManager.instance.PlaySFXPitched(7);
            }
        }
    }

    void SetStats()
    {
        damager.damageAmount = stats[weaponLvl].damage;
        damager.lifeTime = stats[weaponLvl].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLvl].range;
        attackCounter = 0f;
    }
}
