using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

   
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2.0f;
    public float runSpeed = 5.0f;
    public float rotationSpeed = 10.0f;

    // 중력 설정 추가
    [Header("Gravity Settings")]
    public float gravity = -9.81f;     // 중력 가속도
    private float verticalVelocity;    // 수직 속도

    private Animator anim;
    private CharacterController controller;
    private Vector2 moveInput;
    private bool isRunning;
    private bool isAttacking;

    void Start()
    {
        anim = GetComponent<Animator>();

        controller = GetComponent<CharacterController>();
        if (controller == null) controller = gameObject.AddComponent<CharacterController>();

        // [중요 수정] 아래 두 줄을 지워야 Inspector에서 설정한 값이 유지됩니다.
        // controller.center = new Vector3(0, 1, 0); 
        // controller.height = 2.0f;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (isAttacking || anim.GetBool("IsDead")) return;

        // 1. 땅에 닿아 있는지 확인 (중력 리셋)
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // 바닥에 확실히 붙어있게 약간의 힘 유지
        }

        // 2. 카메라 기준 방향 계산
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        // 3. 이동 및 회전 처리
        if (direction.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            // [중요] 이동 벡터 계산 (수평 이동)
            Vector3 moveDir = direction * currentSpeed;

            // 4. 중력 적용 (수직 이동 추가)
            verticalVelocity += gravity * Time.deltaTime;
            moveDir.y = verticalVelocity;

            // 최종 이동 실행
            controller.Move(moveDir * Time.deltaTime);

            anim.SetFloat("MoveSpeed", direction.magnitude * (isRunning ? 1.0f : 0.5f));
        }
        else
        {
            // 움직이지 않아도 중력은 받아야 함
            verticalVelocity += gravity * Time.deltaTime;
            controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

            anim.SetFloat("MoveSpeed", 0);
        }

        anim.SetBool("IsRunning", isRunning);
    }

    // --- Input System 함수들 ---
    void OnMove(InputValue value) { moveInput = value.Get<Vector2>(); }
    void OnRun(InputValue value) { isRunning = value.isPressed; }
    void OnAttack(InputValue value)
    {
        if (anim.GetBool("IsDead")) return;
        if (value.isPressed) anim.SetTrigger("DoAttack");
    }
    void OnCharge(InputValue value)
    {
        if (anim.GetBool("IsDead")) return;
        bool isCharging = value.isPressed;
        anim.SetBool("IsCharging", isCharging);
    }
    void OnDodge(InputValue value)
    {
        if (anim.GetBool("IsDead")) return;
        if (value.isPressed) anim.SetTrigger("DoDodge");
    }
    void OnTestHit(InputValue value) { if (value.isPressed) GetHit(); }
    public void GetHit()
    {
        if (anim.GetBool("IsDead")) return;
        anim.SetTrigger("GetHit");
    }
    public void Die() { anim.SetBool("IsDead", true); }
}
