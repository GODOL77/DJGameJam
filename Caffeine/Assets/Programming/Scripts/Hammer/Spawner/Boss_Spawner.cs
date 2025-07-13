using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Spawner : MonoBehaviour
{
    public GameObject[] bossMonsters;
    public float spawnRadius = 5f;
    private Vector3 playerPos = new Vector3(0, 0, 0);
    private Transform playerTransform;
    private bool bossSpawn = false;
    private EnemyMovement enemy;

    void Update()
    {
        playerPos = GameObject.FindWithTag("Player").transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public void BossSpawn()
    {
        if (!bossSpawn)
        {
            bossSpawn = true;

            GameObject[] inGameEnemy = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject obj in inGameEnemy)
            {
                obj.GetComponent<EnemyMovement>().Die();
            }

            int bossSpawnIndex = Random.Range(0, bossMonsters.Length);

            Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPosition = playerTransform.position + (Vector3)randomOffset;

            if (bossMonsters[bossSpawnIndex])
            {
                Instantiate(bossMonsters[bossSpawnIndex], spawnPosition, Quaternion.identity);
            }
        }
    }
}
