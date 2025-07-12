using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_Manager : MonoBehaviour
{
    // Single Tone //
    public static Setting_Manager settingManager { get; private set; }

    // Private Variable //

    // Public Variable //


    // Functions //
    // default functions
    void Awake()
    {
        SingleTone();
    }

    // system Functions

    // single tone
    private void SingleTone()
    {
        if (null == settingManager)
        {
            settingManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
