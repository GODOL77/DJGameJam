using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    // Private Variable //
    private PlayerManager playerManager;

    // Public Variable //
    // exp
    public Slider expSlider;
    public Text levelText;


    // Functions //
    // default functions
    void Start()
    {
        GetManager();
    }

    void Update()
    {
        EXP();
    }

    // system functions
    private void GetManager()
    {
        playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogError("Player Manager not found on Player!");
        }
    }

    private void EXP()
    {
        if (expSlider)
        {
            float startValue = playerManager.xpToNextLevel - (playerManager.xpToNextLevel / 3);
            float sliderValue = (playerManager.playerXP - startValue) / (playerManager.xpToNextLevel - startValue);
            expSlider.value = sliderValue;
        }

        if (levelText)
        {
            levelText.text = "Level : " + playerManager.playerLevel;
        }
    }
}
