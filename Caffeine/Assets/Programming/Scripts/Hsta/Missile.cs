using UnityEngine;
/*
  직사와 포물선 사이의 느낌을 조절하는 방법:

  이제 Unity 에디터에서 미사일 프리팹의 Missile 컴포넌트를 선택하고 아래 두 값을 조정하여 원하는 궤적을 만드시면 됩니다.


   1. `Arc Height` (포물선 높이):
       * 이 값을 낮추면 궤적이 점점 직선에 가까워집니다. 0으로 설정하면 완전한 직사가 됩니다.
       * 이 값을 높이면 더 큰 포물선을 그리며 날아갑니다.
       * 추천: 0.5 ~ 2.0 사이의 값으로 시작하여 조절해보세요.


   2. `Speed` (미사일 속도):
       * 이 값을 높이면 미사일이 빠르게 목표에 도달하여 플레이어가 피하기 어려워지고, 궤적 또한 더 직선처럼 느껴집니다.
       * 이 값을 낮추면 미사일이 느리게 날아가 피하기 쉬워집니다.

  예시:


   * 빠른 직사 느낌: Speed를 25, Arc Height를 0.5로 설정
   * 부드러운 곡사 느낌: Speed를 15, Arc Height를 2.0으로 설정
*/
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