using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    // Private Variable //
    // rigidbody2D
    private Rigidbody2D rigid;
    // manager
    private Weapon_Manager weaponManager;
    private PlayerManager playerManager;
    // target
    private Transform target;
    private float angle = 0.0f;

    // Public Variable //
    // state
    public float speed = 10.0f;
    public float lifeTime = 3.0f;
    // blueberryjam
    public float blueberryJamRotateSpeed = 200.0f;
    public float homingRange = 5f;


    // Functions //
    // default functions
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);

        GetManager();
        BlueberryJamFindEnemy();
    }

    void Update()
    {
        WeaponMovement();
    }

    // system functions
    // weapon
    private void GetManager()
    {
        weaponManager = GameObject.FindWithTag("Weapon_Manager").GetComponent<Weapon_Manager>();
        if (!weaponManager)
        {
            Debug.LogError("No weapon manager");
        }

        playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        if (!playerManager)
        {
            Debug.LogError("No player manager");
        }
    }

    private void WeaponMovement()
    {
        if (weaponManager.blueberryJam || weaponManager.gameJam)
        {
            if (target == null || Vector2.Distance(transform.position, target.position) > homingRange)
            {
                rigid.angularVelocity = 0f;
                rigid.velocity = transform.right * speed;
                BlueberryJamFindEnemy();
                return;
            }

            Vector2 direction = ((Vector2)target.position - rigid.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle = Vector3.Cross(transform.right, direction).z;
            rigid.angularVelocity = angle * blueberryJamRotateSpeed;
            rigid.velocity = transform.right * speed;
        }
        else
        {
            rigid.velocity = transform.right * speed;
        }
    }

    private void BlueberryJamFindEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.transform;
        }
    }

    // OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyMovement>().TakeDamage(playerManager.attackDamage);
            if (!weaponManager.grapeJam && !weaponManager.gameJam)
            {
                Destroy(gameObject);

            }
        }
    }
}
