using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public AudioMixer theMixer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Master"))
        {
            theMixer.SetFloat("Master", PlayerPrefs.GetFloat("Master"));
        }
        
        if (PlayerPrefs.HasKey("Music"))
        {
            theMixer.SetFloat("Music", PlayerPrefs.GetFloat("Music"));
        }
        
        if (PlayerPrefs.HasKey("SFX"))
        {
            theMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX"));
        }
        
        // SFX Panel
        if (PlayerPrefs.HasKey("Enemy Death"))
        {
            theMixer.SetFloat("Enemy Death", PlayerPrefs.GetFloat("Enemy Death"));
        }
        
        if (PlayerPrefs.HasKey("Hit Enemy"))
        {
            theMixer.SetFloat("Hit Enemy", PlayerPrefs.GetFloat("Hit Enemy"));
        }
        
        if (PlayerPrefs.HasKey("Player Death"))
        {
            theMixer.SetFloat("Player Death", PlayerPrefs.GetFloat("Player Death"));
        }
        
        if (PlayerPrefs.HasKey("EXP"))
        {
            theMixer.SetFloat("EXP", PlayerPrefs.GetFloat("EXP"));
        }
        
        if (PlayerPrefs.HasKey("Gold"))
        {
            theMixer.SetFloat("Gold", PlayerPrefs.GetFloat("Gold"));
        }
        
        if (PlayerPrefs.HasKey("Curse Zone"))
        {
            theMixer.SetFloat("Curse Zone", PlayerPrefs.GetFloat("Curse Zone"));
        }
        
        if (PlayerPrefs.HasKey("Soul Orb"))
        {
            theMixer.SetFloat("Soul Orb", PlayerPrefs.GetFloat("Soul Orb"));
        }
        
        if (PlayerPrefs.HasKey("Dagger"))
        {
            theMixer.SetFloat("Dagger", PlayerPrefs.GetFloat("Dagger"));
        }
        
        if (PlayerPrefs.HasKey("Sword"))
        {
            theMixer.SetFloat("Sword", PlayerPrefs.GetFloat("Sword"));
        }
        
        if (PlayerPrefs.HasKey("Axe"))
        {
            theMixer.SetFloat("Axe", PlayerPrefs.GetFloat("Axe"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
