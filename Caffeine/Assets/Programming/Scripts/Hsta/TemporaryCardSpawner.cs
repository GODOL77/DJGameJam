using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TemporaryCardSpawner : MonoBehaviour
{
    public List<GameObject> cardPrefabs; // 스폰할 카드 프리팹 목록

    void OnEnable()
    {
        Time.timeScale = 0f; // 게임 시간 정지
        SpawnUniqueCards();
    }

    void OnDisable()
    {
        Time.timeScale = 1f; // 게임 시간 재개
        // 이 스크립트가 비활성화될 때 생성된 카드들을 파괴할 필요는 없습니다.
        // SpawnUniqueCards()에서 이미 기존 자식들을 파괴하고 새로 생성하기 때문입니다.
        // 만약 이 스크립트가 비활성화될 때 모든 카드를 지우고 싶다면 여기에 Destroy(child.gameObject) 로직을 추가할 수 있습니다.
    }

    void SpawnUniqueCards()
    {
        // 기존 자식 오브젝트 모두 파괴
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (cardPrefabs == null || cardPrefabs.Count == 0)
        {
            Debug.LogWarning("Card Prefabs list is empty or null. Cannot spawn cards.");
            return;
        }

        // 중복되지 않는 3개의 프리팹 선택
        List<GameObject> selectedPrefabs = cardPrefabs.OrderBy(x => Random.value).Take(3).ToList();

        // 선택된 프리팹들을 자식으로 생성
        foreach (GameObject prefab in selectedPrefabs)
        {
            if (prefab != null)
            {
                GameObject newCard = Instantiate(prefab, transform);
                newCard.SetActive(true); // 생성 시 활성화
                Debug.Log($"Spawned: {newCard.name}");
            }
            else
            {
                Debug.LogWarning("A null prefab was found in the list.");
            }
        }
    }
}