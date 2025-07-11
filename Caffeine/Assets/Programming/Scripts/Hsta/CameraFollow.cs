using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 플레이어의 Transform
    public Vector3 offset; // 카메라와 플레이어 간의 오프셋
    public float smoothSpeed = 0.125f; // 부드러운 이동 속도 (0~1, 낮을수록 부드러움)

    void LateUpdate()
    {
        // 목표 위치 (플레이어 위치 + 오프셋)
        Vector3 desiredPosition = target.position + offset;
        // Z축은 카메라의 원래 Z값 유지 (2D에서 중요)
        desiredPosition.z = transform.position.z;
        // 부드럽게 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}