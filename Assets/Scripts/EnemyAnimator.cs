using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyAnimator : MonoBehaviour
{
    public Animator animator;
    public Transform sprite;
    //public AudioClip deathSound; // Reference to the death sound clip
    //public AudioMixerGroup audioMixerGroup; // Reference to the audio mixer group
    public float speed;

    // These variables will be editable in the inspector
    public float minSpeedFactor = 0.75f;
    public float maxSpeedFactor = 1.25f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public GameObject deathEffectPrefab;
    public bool useDeathAnimation = true;

    public bool useIdleAnimation = true;
    public bool useRunAnimation = true;
    public bool useAttackAnimation = true;

    //private AudioSource audioSource; // Reference to the AudioSource component
    private float activeSize;

    // Start is called before the first frame update
    void Start()
    {
        activeSize = maxSize;
        speed = speed * Random.Range(minSpeedFactor, maxSpeedFactor);
        //audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component
        //audioSource.outputAudioMixerGroup = audioMixerGroup; // Set the output to the specified Audio Mixer Group
    }

    // Update is called once per frame
    void Update()
    {
        sprite.localScale = Vector3.MoveTowards(sprite.localScale, Vector3.one * activeSize, speed * Time.deltaTime);
        if (sprite.localScale.x == activeSize)
        {
            if (activeSize == maxSize)
            {
                activeSize = minSize;
            }
            else
            {
                activeSize = maxSize;
            }
        }
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        animator.SetFloat("MoveSpeed", moveSpeed);
    }

    public void SetAttackDistance(bool withinDistance)
    {
        animator.SetBool("WithinAttackDistance", withinDistance);
    }

    // Function to play death sound
    /*public void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.clip = deathSound;
            audioSource.Play();
        }
    }*/
    
    public void PlayDeathAnimationWithoutSound()
    {
        if (useDeathAnimation)
        {
            animator.SetTrigger("Death"); 
        }
        else if (deathEffectPrefab != null)
        {
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, 2f);
        }
    }


    public void PlayDeathAnimation()
    {
        if (useDeathAnimation)
        {
            animator.SetTrigger("Death");
            //PlayDeathSound(); // Sound will be played.
        }
        else if (deathEffectPrefab != null)
        {
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, 2f);
            //PlayDeathSound();
        }
    }

    public void SpawnDeathEffect(Vector3 position)
    {
        if (!useDeathAnimation && deathEffectPrefab != null)
        {
            GameObject deathEffect = Instantiate(deathEffectPrefab, position, Quaternion.identity);
            Destroy(deathEffect, 2f);
        }
    }

    public void SetIdleAnimation(bool active)
    {
        if (useIdleAnimation)
        {
            animator.SetBool("Idle", active);
        }
    }

    public void SetRunAnimation(bool active)
    {
        if (useRunAnimation)
        {
            animator.SetBool("Run", active);
        }
    }

    public void SetAttackAnimation(bool active)
    {
        if (useAttackAnimation)
        {
            animator.SetBool("Attack", active);
        }
    }

    // Function that gets called when the object gets disabled
    private void OnDisable()
    {
        //PlayDeathSound(); // Play death sound when the object gets disabled
    }
}
