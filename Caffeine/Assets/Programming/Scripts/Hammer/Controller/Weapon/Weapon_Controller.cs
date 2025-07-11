using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Controller : MonoBehaviour
{
    // Private Variable //
    // manager
    private Weapon_Manager weaponManager;
    // timer
    public float coolTimer = 0.0f;

    // Public Variable //


    // Functions //
    // default functions
    void Start()
    {
        GetWeaponManager();
    }

    void Update()
    {
        WeaponType();
        LookAtMouse();
    }

    // system Functions
    // look at mouse cusor
    private void LookAtMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = targetPosition - transform.position;
        direction.z = 0;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    // weapon
    private void GetWeaponManager()
    {
        weaponManager = GameObject.FindWithTag("Weapon_Manager").GetComponent<Weapon_Manager>();
    }

    private void WeaponType()
    {
        if (weaponManager)
        {
            StrawberryJam();
            GrapeJam();
            AppleJam();
            BlueberryJam();
            GameJam();
        }
        else
        {
            Debug.Log("No weapon manager");
        }
    }
    private void StrawberryJam()
    {
        if (weaponManager.strawberryJam)
        {
            if (coolTimer < weaponManager.weaponShootTime)
            {
                coolTimer += Time.deltaTime * 1.0f;
            }
            else
            {
                if (weaponManager.strawberryPrefab)
                {
                    Instantiate(weaponManager.strawberryPrefab, transform.position, transform.rotation);
                }
                coolTimer = 0.0f;
            }
        }
    }
    private void GrapeJam()
    {
        if (weaponManager.grapeJam)
        {
            if (coolTimer < weaponManager.weaponShootTime)
            {
                coolTimer += Time.deltaTime * 1.0f;
            }
            else
            {
                if (weaponManager.grapePrefab)
                {
                    Instantiate(weaponManager.grapePrefab, transform.position, transform.rotation);
                }
                coolTimer = 0.0f;
            }
        }
    }
    private void AppleJam()
    {
        if (weaponManager.appleJam)
        {
            if (coolTimer < weaponManager.appleJamShootTime)
            {
                coolTimer += Time.deltaTime * 1.0f;
            }
            else
            {
                if (weaponManager.applePrefab)
                {
                    Instantiate(weaponManager.grapePrefab, transform.position, transform.rotation);
                }
                coolTimer = 0.0f;
            }
        }
    }
    private void BlueberryJam()
    {
        if (weaponManager.blueberryJam)
        {
            if (coolTimer < weaponManager.weaponShootTime)
            {
                coolTimer += Time.deltaTime * 1.0f;
            }
            else
            {
                if (weaponManager.blueberryPrefab)
                {
                    Instantiate(weaponManager.grapePrefab, transform.position, transform.rotation);
                }
                coolTimer = 0.0f;
            }
        }
    }
    private void GameJam()
    {
        if (weaponManager.gameJam)
        {
            if (coolTimer < weaponManager.appleJamShootTime)
            {
                coolTimer += Time.deltaTime * 1.0f;
            }
            else
            {
                if (weaponManager.gamePrefab)
                {
                    Instantiate(weaponManager.gamePrefab, transform.position, transform.rotation);
                }
                coolTimer = 0.0f;
            }
        }
    }
}
