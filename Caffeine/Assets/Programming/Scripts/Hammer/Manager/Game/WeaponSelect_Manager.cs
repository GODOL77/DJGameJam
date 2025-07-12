using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect_Manager : MonoBehaviour
{
    public Image lockIMG;
    public GameObject WeaponSelectUI;
    public GameObject[] jamSelectButtons;
    public int[] jamLock;

    private Weapon_Manager weaponManager;
    private int pageNumber = 0;

    void Start()
    {
        Time.timeScale = 0.0f;

        weaponManager = GameObject.FindWithTag("Weapon_Manager").GetComponent<Weapon_Manager>();
        if (weaponManager == null)
        {
            Debug.LogError("No Weapon Manager");
        }
    }

    void Update()
    {
        
    }

    private void WeaponPage()
    {
        jamSelectButtons[pageNumber].SetActive(true);

        
    }

    public void NextButton()
    {
        if (pageNumber < jamSelectButtons.Length - 1)
        {
            pageNumber++;
            Debug.Log(pageNumber);
        }
    }

    public void ReturnButton()
    {
        if (pageNumber > 0)
        {
            pageNumber--;
            Debug.Log(pageNumber);
        }
    }

    public void StrawberryJam()
    {
        Time.timeScale = 1.0f;
        if (weaponManager)
        {
            weaponManager.strawberryJam = true;
            weaponManager.grapeJam = false;
            weaponManager.appleJam = false;
            weaponManager.blueberryJam = false;
            weaponManager.gameJam = false;
        }

        if (WeaponSelectUI)
        {
            WeaponSelectUI.SetActive(false);   
        }
    }

    public void GrapeJam()
    {
        Time.timeScale = 1.0f;
        if (weaponManager)
        {
            weaponManager.strawberryJam = false;
            weaponManager.grapeJam = true;
            weaponManager.appleJam = false;
            weaponManager.blueberryJam = false;
            weaponManager.gameJam = false;
        }

        if (WeaponSelectUI)
        {
            WeaponSelectUI.SetActive(false);   
        }
    }

    public void AppleJam()
    {
        Time.timeScale = 1.0f;
        if (weaponManager)
        {
            weaponManager.strawberryJam = false;
            weaponManager.grapeJam = false;
            weaponManager.appleJam = true;
            weaponManager.blueberryJam = false;
            weaponManager.gameJam = false;
        }

        if (WeaponSelectUI)
        {
            WeaponSelectUI.SetActive(false);   
        }
    }

    public void BlueberryJam()
    {
        Time.timeScale = 1.0f;
        if (weaponManager)
        {
            weaponManager.strawberryJam = false;
            weaponManager.grapeJam = false;
            weaponManager.appleJam = false;
            weaponManager.blueberryJam = true;
            weaponManager.gameJam = false;
        }

        if (WeaponSelectUI)
        {
            WeaponSelectUI.SetActive(false);   
        }
    }

    public void GameJam()
    {
        Time.timeScale = 1.0f;
        if (weaponManager)
        {
            weaponManager.strawberryJam = false;
            weaponManager.grapeJam = false;
            weaponManager.appleJam = false;
            weaponManager.blueberryJam = false;
            weaponManager.gameJam = true;
        }

        if (WeaponSelectUI)
        {
            WeaponSelectUI.SetActive(false);   
        }
    }
}
