using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어 이동 속도

    void FixedUpdate()
    {
        // WASD 또는 화살표 키 입력 받기
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D 또는 왼쪽/오른쪽 화살표
        float moveY = Input.GetAxisRaw("Vertical");   // W/S 또는 위/아래 화살표

        // 이동 방향 벡터 계산
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized; // 대각선 이동 시 속도 일정하게 유지

        // 플레이어 이동
        // Time.fixedDeltaTime을 곱하여 프레임 속도에 독립적인 움직임 보장
        transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}
