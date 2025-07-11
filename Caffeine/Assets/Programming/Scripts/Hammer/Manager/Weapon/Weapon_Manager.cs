using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Manager : MonoBehaviour
{
    // Private Variable //

    // Public Variable //
    // weapon type
    [Header("Weapon Type")]
    public float weaponShootTime = 0.5f;
    public float appleJamShootTime = 0.2f;
    public bool strawberryJam = false;
    public GameObject strawberryPrefab;
    public bool grapeJam = false;
    public GameObject grapePrefab;
    public bool appleJam = false;
    public GameObject applePrefab;
    public bool blueberryJam = false;
    public GameObject blueberryPrefab;
    public bool gameJam = false;
    public GameObject gamePrefab;


    // Functions //
    // default functions
    void Start()
    {

    }

    void Update()
    {

    }

    // system Functions

}
