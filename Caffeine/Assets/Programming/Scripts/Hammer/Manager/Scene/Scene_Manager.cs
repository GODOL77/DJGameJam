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

    public void TitleScene()
    {
        SceneManager.LoadScene("Scene_Start");
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        // Find and destroy the existing PlayerManager instance
        if (PlayerManager.Instance != null)
        {
            Destroy(PlayerManager.Instance.gameObject);
        }

        SceneManager.LoadScene("Scene_InGame");
    }

    public void QuitGame()
    {
        Application.Quit();
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
