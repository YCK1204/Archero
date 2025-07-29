using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterStats stats;
    [Header("Movement")]
    //[SerializeField] private float moveSpeed = 5f;  // 이동속도 인스펙터에서 조정 가능

    [Header("Dash")]
    //[SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    //   [SerializeField] private float invincibleDuration = 0.2f;  // (일단 임시로 만든) 대쉬 중 무적 시간
    [SerializeField] private float acceleration = 15f;  // 가속 정도

    [SerializeField] private bool canDash = true;  // 대쉬기능 락/언락 가능
    public bool CanDash { get => canDash; set => canDash = value; }
    public bool IsMoving { get; private set; }

    private Vector2 moveInput;
    private Rigidbody2D rgdbd;
    private bool isDashing = false;
    private float currentSpeed;
    private float dashTimeLeft = 0f;
    private float lastDashTime = -Mathf.Infinity;

    void Awake()
    {
        rgdbd = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        // 시작 속도를 CharacterStats에서 가져와 초기화
        currentSpeed = stats.FinalMoveSpeed;
    }

    void Update()
    {
        HandleMovementInput();
        HandleDashInput();
    }

    void FixedUpdate() // 이동
    {
        float targetSpeed = isDashing ? stats.FinalDashSpeed : stats.FinalMoveSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * acceleration); // 부드러운 대쉬 이동
        rgdbd.velocity = moveInput * currentSpeed;
    }

    private void HandleMovementInput() // 일반 이동 입력 감지 및 처리
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        IsMoving = moveInput != Vector2.zero;
    }

    private void HandleDashInput() // 대쉬 입력 감지 및 처리
    {
        if (!canDash) return; // 대쉬 락 상태면 빠꾸
        if (!Input.GetKey(KeyCode.Space)) return; // 누르고 있는 동안 검사

        if (Time.time >= lastDashTime + dashCooldown && moveInput != Vector2.zero && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;

        // ?? 여기에 무적 로직 넣기 ??
        // EnableInvincibility(invincibleDuration); ??????

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        // ?? 무적 해제 로직 ??
        // DisableInvincibility(); ??????
    }
}