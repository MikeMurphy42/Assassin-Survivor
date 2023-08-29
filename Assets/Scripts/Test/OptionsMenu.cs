using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;
    public ResItem[] resolutions;
    public int selectedResolution;
    public TMP_Text resolutionLabel;
    public AudioMixer theMixer;
    public Slider masterSlider, musicSlider, sfxSlider;
    public TMP_Text masterLabel, musicLabel, sfxLabel;
    public AudioSource sfxLoop;

    // SFX Panel and Individual SFX controls
    public GameObject sfxPanel;
    public Slider[] individualSfxSliders;
    public TMP_Text[] individualSfxLabels;
    public string[] sfxParameterNames;

    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        bool foundRes = false;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;
                selectedResolution = i;
                UpdateResLabel();
            }
        }

        if (!foundRes)
        {
            resolutionLabel.text = Screen.width.ToString() + " x " + Screen.height.ToString();
        }

        if (PlayerPrefs.HasKey("Master"))
        {
            theMixer.SetFloat("Master", PlayerPrefs.GetFloat("Master"));
            masterSlider.value = PlayerPrefs.GetFloat("Master");
        }

        if (PlayerPrefs.HasKey("Music"))
        {
            theMixer.SetFloat("Music", PlayerPrefs.GetFloat("Music"));
            musicSlider.value = PlayerPrefs.GetFloat("Music");
        }

        if (PlayerPrefs.HasKey("SFX"))
        {
            theMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX"));
            sfxSlider.value = PlayerPrefs.GetFloat("SFX");
        }

        for (int i = 0; i < individualSfxSliders.Length; i++)
        {
            if (PlayerPrefs.HasKey(sfxParameterNames[i]))
            {
                individualSfxSliders[i].value = PlayerPrefs.GetFloat(sfxParameterNames[i]);
                individualSfxLabels[i].text = (individualSfxSliders[i].value + 80).ToString();
                theMixer.SetFloat(sfxParameterNames[i], individualSfxSliders[i].value);
            }
        }

        masterLabel.text = (masterSlider.value + 80).ToString();
        musicLabel.text = (musicSlider.value + 80).ToString();
        sfxLabel.text = (sfxSlider.value + 80).ToString();
    }

    public void ResLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Length - 1)
        {
            selectedResolution = resolutions.Length - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullscreenTog.isOn;

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
            Debug.Log("Applying Graphics V ON");
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            Debug.Log("Applying Graphics V OFF");
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
        Debug.Log("Applying Graphics");
    }

    public void SetMasterVol()
    {
        masterLabel.text = (masterSlider.value + 80).ToString();
        theMixer.SetFloat("Master", masterSlider.value);
        PlayerPrefs.SetFloat("Master", masterSlider.value);
        PlayerPrefs.Save(); // Save changes immediately
    }

    public void SetMusicVol()
    {
        musicLabel.text = (musicSlider.value + 80).ToString();
        theMixer.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.Save(); // Save changes immediately
    }

    public void SetSfxVol()
    {
        sfxLabel.text = (sfxSlider.value + 80).ToString();
        theMixer.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.Save(); // Save changes immediately
    }

    public void PlaySFXLoop()
    {
        sfxLoop.Play();
    }

    public void StopSFXLoop()
    {
        sfxLoop.Stop();
    }

    public void OpenSfxPanel()
    {
        sfxPanel.SetActive(true);
    }

    public void CloseSfxPanel()
    {
        sfxPanel.SetActive(false);
    }

    public void SetIndividualSfxVolume(int index)
    {
        individualSfxLabels[index].text = (individualSfxSliders[index].value + 80).ToString();
        theMixer.SetFloat(sfxParameterNames[index], individualSfxSliders[index].value);
        PlayerPrefs.SetFloat(sfxParameterNames[index], individualSfxSliders[index].value);
        PlayerPrefs.Save(); // Save changes immediately
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal;
    [FormerlySerializedAs("verticle")] public int vertical;
}
