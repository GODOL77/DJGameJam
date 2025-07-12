using UnityEngine;

public class Missile : MonoBehaviour
{
    public Vector3 targetPosition;
    public GameObject toastEnemyPrefab;
    public float speed = 15f; // 미사일 속도 (이 값을 높이면 더 직사처럼 보입니다)
    public float arcHeight = 1.5f; // 포물선 높이 (이 값을 낮추면 더 직사처럼 보입니다)

    private Vector3 startPosition;
    private float journeyDistance;
    private float startTime;

    void Start()
    {
        startPosition = transform.position;
        startTime = Time.time;
        journeyDistance = Vector3.Distance(startPosition, targetPosition);
    }

    void Update()
    {
        // 이동 거리 및 진행률 계산
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyDistance;

        // 시작점과 끝점 사이의 직선 보간
        Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

        // 포물선 높이 추가
        if (arcHeight > 0)
        {
            currentPos.y += Mathf.Sin(fractionOfJourney * Mathf.PI) * arcHeight;
        }

        transform.position = currentPos;

        // 목표 도달 시
        if (fractionOfJourney >= 1.0f)
        {
            SpawnToastAndDestroy();
        }
    }

    void SpawnToastAndDestroy()
    {
        Debug.Log("Missile reached target. Spawning toast!");
        if (toastEnemyPrefab != null)
        {
            Instantiate(toastEnemyPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject); // 미사일 오브젝트 파괴
    }

    // 미사일이 다른 물체와 충돌했을 경우 (예: 벽)
    void OnCollisionEnter2D(Collision2D collision)
    {
        SpawnToastAndDestroy();
    }
}