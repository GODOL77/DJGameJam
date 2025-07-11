using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // 스폰할 적 프리팹 목록
    public float spawnRadius = 10f; // 플레이어 주변 스폰 반경
    public float spawnInterval = 3f; // 적 스폰 주기 (초)
    public int maxEnemies = 10; // 동시에 존재할 수 있는 최대 적 수

    private Transform playerTransform; // 플레이어의 Transform
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // 스폰된 적 목록

    void Start()
    {
        // "Player" 태그를 가진 오브젝트를 찾습니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found. EnemySpawner will not function.");
            enabled = false; // 스크립트 비활성화
            return;
        }

        // 적 스폰 코루틴 시작
        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 최대 적 수 제한
            CleanUpDestroyedEnemies(); // 파괴된 적 제거
            if (spawnedEnemies.Count < maxEnemies)
            {
                SpawnSingleEnemy();
            }
        }
    }

    void SpawnSingleEnemy()
    {
        if (playerTransform == null || enemyPrefabs.Length == 0) return;

        // 플레이어 주변 랜덤 위치 계산
        Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = playerTransform.position + (Vector3)randomOffset;

        // 랜덤 적 프리팹 선택
        GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // 적 스폰
        GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(newEnemy);
        Debug.Log($"Spawned {newEnemy.name} at {spawnPosition}");
    }

    // 파괴된 적을 spawnedEnemies 목록에서 제거
    void CleanUpDestroyedEnemies()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null);
    }
}
