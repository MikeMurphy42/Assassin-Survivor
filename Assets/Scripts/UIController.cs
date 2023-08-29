using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Slider expLvlSlider;
    public TMP_Text expLvlText;
    public LevelUpSelectionButton[] LevelUpSelectionButtons;
    public GameObject levelUpPanel;
    public TMP_Text coinText; // For main screen
    public TMP_Text upgradeCoinText; // For upgrade screen
    public PlayerStatUpgradeDisplay moveSpeedUpgradeDisplay, healthUpgradeDisplay, pickupRangeUpgradeDisplay, maxWeaponsUpgradeDisplay;
    public TMP_Text timerText;
    public GameObject levelEndScreen;
    public TMP_Text endTimeText;
    public string mainMenuName;
    public GameObject pauseScreen;
    
    // Options
    public GameObject OptionsScreen;
    //public GameObject OptionsMenu;
    

    public float timeScaleDelay = 0f; // Delay before timescale change starts
    public float timeScaleDuration = 1f; // Duration of the timescale transition

    private bool isTimeScaleTransitioning = false; // Track whether the timescale transition is happening
    private float timeScaleTransitionTimer = 0f; // Timer for the timescale transition

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }

        if (isTimeScaleTransitioning)
        {
            timeScaleTransitionTimer += Time.unscaledDeltaTime; // Increment timer by real-world delta time
            Time.timeScale = Mathf.Lerp(0f, 1f, timeScaleTransitionTimer / timeScaleDuration); // Lerp timescale

            if (timeScaleTransitionTimer >= timeScaleDuration)
            {
                isTimeScaleTransitioning = false; // Stop the transition
                Time.timeScale = 1f; // Ensure timescale is set to 1
            }
        }
    }

    public void UpdateExperiance(int currentExp, int levelExp, int currentLvl)
    {
        expLvlSlider.maxValue = levelExp;
        expLvlSlider.value = currentExp;
        expLvlText.text = "LVL - " + currentLvl;
    }

    public void SkipLVLUp()
    {
        levelUpPanel.SetActive(false);
        StartSkipLevelTransition();
        //StartTransition(); // Start the timescale transition
    }

    public void UpdateCoins()
    {
        string coinsDisplay = "coins - " + CoinController.instance.currentCoins;
        coinText.text = coinsDisplay; // Update main screen
        upgradeCoinText.text = coinsDisplay; // Update upgrade screen
    }

    public void PurchaseMoveSpeed()
    {
        PlayerStatController.instance.PurchaseMoveSpeed();
    }

    public void PurchaseHealth()
    {
        PlayerStatController.instance.PurchaseHealth();
    }

    public void PurchasePickUpRange()
    {
        PlayerStatController.instance.PurchasePickupRange();
    }

    public void PurchaseMaxWeapons()
    {
        PlayerStatController.instance.PurchaseMaxWeapons();
    }

    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60f);
        float seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = "Time: " + minutes + ":" + seconds.ToString("00");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuName);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseUnpause()
    {
        if (pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            isTimeScaleTransitioning = false; // Stop any ongoing transition
        }
        else
        {
            pauseScreen.SetActive(false);
            if (levelUpPanel.activeSelf == false)
            {
                isTimeScaleTransitioning = true; // Start the timescale transition
                timeScaleTransitionTimer = 0f; // Reset the transition timer
                Invoke("StartTransition", timeScaleDelay); // Start transition after specified delay
            }
        }
    }
    
    public void ShowHideOptions()
    {
        if (!OptionsScreen.activeInHierarchy)
        {
            OptionsScreen.SetActive(true);
        }
        else
        {
            OptionsScreen.SetActive(false);
        } 
        /*if (!OptionsMenu.activeInHierarchy)
        {
            //OptionsMenu.SetActive(true);
        }
        else
        {
            OptionsMenu.SetActive(false);
        }*/
    }
    
    public void OpenOption()
    {
        OptionsScreen.SetActive(true);
    }

    public void CloseOption()
    {
        OptionsScreen.SetActive(false);
    }
    
    
    public void Options()
    {
        // Add Options Here`                     
        //Debug.Log("No Options YETTT?"); 
        ShowHideOptions();
    }
    
    /*if (OptionsScreen.activeInHierarchy && Cursor.lockState != CursorLockMode.None)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }*/ 
    
    public void StartSkipLevelTransition()
    {
        isTimeScaleTransitioning = true; // Start the timescale transition
        timeScaleTransitionTimer = 0f; // Reset the transition timer
        Invoke("StartTransition", timeScaleDelay); // Start transition after specified delay
    }


    public void StartTransition()
    {
        isTimeScaleTransitioning = true;
    }
}
