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
            int bossSpawnIndex = Random.Range(0, bossMonsters.Length - 1);

            Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPosition = playerTransform.position + (Vector3)randomOffset;
            Instantiate(bossMonsters[bossSpawnIndex], spawnPosition, Quaternion.identity);
        }
    }
}
