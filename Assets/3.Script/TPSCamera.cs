using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCamera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // 따라갈 플레이어 (Erika Archer)
    public Vector3 offset = new Vector3(0, 2.0f, -3.5f); // 카메라 거리 (높이, 뒤쪽 거리)

    [Header("Mouse Settings")]
    public float sensitivity = 0.5f; // 마우스 감도
    public float pitchMin = -10f; // 아래로 쳐다보는 제한 각도
    public float pitchMax = 60f;  // 위로 쳐다보는 제한 각도

    private float yaw = 0.0f;   // 좌우 회전 값
    private float pitch = 0.0f; // 상하 회전 값

    void Start()
    {
        // 마우스 커서를 게임 화면에 가두고 숨깁니다 (ESC 누르면 풀림)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate() // 카메라는 항상 LateUpdate에서 움직여야 부드럽습니다
    {
        if (target == null) return;

        // 1. 마우스 입력 받기 (뉴 인풋 시스템)
        // 만약 마우스가 너무 빠르면 sensitivity 값을 줄이세요
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * sensitivity;

        yaw += mouseDelta.x;
        pitch -= mouseDelta.y;

        // 2. 상하 각도 제한 (목 꺾임 방지)
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // 3. 회전 계산
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // 4. 위치 계산 (플레이어 위치 + 회전값 * 거리)
        // target.position + Vector3.up * 1.5f는 플레이어 발끝이 아니라 '가슴'을 바라보게 하기 위함
        Vector3 targetPos = target.position + Vector3.up * 1.5f;
        transform.position = targetPos + rotation * offset;

        // 5. 플레이어 쳐다보기
        transform.LookAt(targetPos);
    }
}
