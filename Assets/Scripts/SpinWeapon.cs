using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWeapon : Weapon
{

    public float rotateSpeed;

    public Transform holder, blackHoleToSpawn;

    public float timeBetweenSpawn;
    private float spawnCounter;

    public EnemyDamager damager;

    private float scaleUpdateCounter = 0.2f; // Counter for scale update
    
    // Start is called before the first frame update
    void Start()
    {
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));

        spawnCounter -= Time.deltaTime;
        if (spawnCounter <0)
        {
            spawnCounter = timeBetweenSpawn;

            Transform blackHole = Instantiate(blackHoleToSpawn, blackHoleToSpawn.position, blackHoleToSpawn.rotation, holder);
            blackHole.gameObject.SetActive(true);
        }

        // Update range every 0.2 seconds
        scaleUpdateCounter -= Time.deltaTime;
        if (scaleUpdateCounter <= 0)
        {
            float scale = stats[weaponLvl].range;
            transform.localScale = Vector3.one * scale;
            scaleUpdateCounter = 0.2f;
        }
    }

    public void SetStats()
    {
        damager.damageAmount = stats[weaponLvl].damage;
        
        float scale = stats[weaponLvl].range;
        transform.localScale = Vector3.one * scale;

        timeBetweenSpawn = stats[weaponLvl].timeBetweenAttacks;

        damager.lifeTime = stats[weaponLvl].duration;

        spawnCounter = 0f;

        // Instantiate and scale black hole, then deactivate it for later use
        Transform blackHole = Instantiate(blackHoleToSpawn, blackHoleToSpawn.position, blackHoleToSpawn.rotation, holder);
        blackHole.localScale = Vector3.one * scale;

        foreach (Collider collider in blackHole.GetComponentsInChildren<Collider>())
        {
            collider.transform.localScale = Vector3.one * scale;
        }

        foreach (ParticleSystem particleSystem in blackHole.GetComponentsInChildren<ParticleSystem>())
        {
            var main = particleSystem.main;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            particleSystem.transform.localScale = Vector3.one * scale;
        }

        blackHole.gameObject.SetActive(false);
    }
    
}
