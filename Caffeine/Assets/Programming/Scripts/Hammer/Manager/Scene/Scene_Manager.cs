using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    // Single Tone //
    private static Scene_Manager sceneManager = null;
    // Private Variable //

    // Public Variable //


    // Functions //
    // default functions
    void Awake()
    {
        SingleTone();
    }

    // system Functions
    public void StartGameScene()
    {
        SceneManager.LoadScene("Scene_tuto");
    }

    // single tone
    private void SingleTone()
    {
        if (null == sceneManager)
        {
            sceneManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static Scene_Manager SceneManagerSC
    {
        get
        {
            if (null == sceneManager)
            {
                return null;
            }
            return sceneManager;
        }
    }
}
