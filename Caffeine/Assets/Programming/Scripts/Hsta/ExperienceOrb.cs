using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    public int xpAmount = 10; // 이 오브가 주는 경험치 양
    public float attractionSpeed = 5f; // 플레이어에게 끌려가는 속도
    public float attractionRange = 2f; // 플레이어가 이 범위 안에 들어오면 끌려가기 시작

    private Transform playerTransform; // 플레이어의 Transform
    private bool isAttracting = false; // 플레이어에게 끌려가는 중인지 여부

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
            Debug.LogWarning("Player GameObject with tag 'Player' not found. ExperienceOrb will not attract.");
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // 플레이어가 일정 범위 안에 들어오면 끌려가기 시작
        if (distanceToPlayer <= attractionRange)
        {
            isAttracting = true;
        }

        // 끌려가는 중이면 플레이어를 향해 이동
        if (isAttracting)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, attractionSpeed * Time.deltaTime);
        }
    }

    // 플레이어와 충돌 시 경험치 부여 및 오브 파괴
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.GainXP(xpAmount);
                Destroy(gameObject); // 경험치 오브 파괴
            }
            else
            {
                Debug.LogWarning("PlayerManager Instance not found when collecting XP.");
            }
        }
    }
}