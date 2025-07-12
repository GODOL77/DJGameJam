using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Manager : MonoBehaviour
{
    // Single Tone //
    private static State_Manager stateManager = null;
    // Private Variable //

    // Public Variable //
    // state
    public int xpIncrease = 0;
    public int damageIncrease = 0;
    public int armorIncrease = 0;
    public int hpIncrease = 0;
    public int attackSpeedIncrease = 0;
    // item
    public int magnet = 0;
    public int retry = 0;
    public int aroundSphere = 0;


    // Functions //
    // default functions
    void Awake()
    {
        SingleTone();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    // system Functions

    // single tone
    void SingleTone()
    {
        if (null == stateManager)
        {
            stateManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static State_Manager StateManager
    {
        get
        {
            if (null == stateManager)
            {
                return null;
            }
            return stateManager;
        }
    }
}
