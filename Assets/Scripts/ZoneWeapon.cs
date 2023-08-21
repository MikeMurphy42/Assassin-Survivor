using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWeapon : Weapon
{

    public EnemyDamager damager;

    private float spawntTime, spawnCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (statsUpdated == true)
        {
            statsUpdated = false;
            SetStats();
        }

        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0f)
        {
            spawnCounter = spawntTime;

            Instantiate(damager, damager.transform.position, Quaternion.identity, transform).gameObject.SetActive(true);
            
            SFXManager.instance.PlaySFXPitched(8);
        }
    }

    void SetStats()
    {
        damager.damageAmount = stats[weaponLvl].damage;
        damager.lifeTime = stats[weaponLvl].duration;

        damager.timeBetweenDamage = stats[weaponLvl].speed;

        damager.transform.localScale = Vector3.one * stats[weaponLvl].range;

        spawntTime = stats[weaponLvl].timeBetweenAttacks;

        spawnCounter = 0f;

    }
    
}