using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterStats stats;
    [Header("Movement")]
    //[SerializeField] private float moveSpeed = 5f;  // �̵��ӵ� �ν����Ϳ��� ���� ����

    [Header("Dash")]
    //[SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    //   [SerializeField] private float invincibleDuration = 0.2f;  // (�ϴ� �ӽ÷� ����) �뽬 �� ���� �ð�
    [SerializeField] private float acceleration = 15f;  // ���� ����

    [SerializeField] private bool canDash = true;  // �뽬��� ��/��� ����
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
        // ���� �ӵ��� CharacterStats���� ������ �ʱ�ȭ
        currentSpeed = stats.FinalMoveSpeed;
    }

    void Update()
    {
        HandleMovementInput();
        HandleDashInput();
    }

    void FixedUpdate() // �̵�
    {
        float targetSpeed = isDashing ? stats.FinalDashSpeed : stats.FinalMoveSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * acceleration); // �ε巯�� �뽬 �̵�
        rgdbd.velocity = moveInput * currentSpeed;
    }

    private void HandleMovementInput() // �Ϲ� �̵� �Է� ���� �� ó��
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        IsMoving = moveInput != Vector2.zero;
    }

    private void HandleDashInput() // �뽬 �Է� ���� �� ó��
    {
        if (!canDash) return; // �뽬 �� ���¸� ����
        if (!Input.GetKey(KeyCode.Space)) return; // ������ �ִ� ���� �˻�

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

        // ?? ���⿡ ���� ���� �ֱ� ??
        // EnableInvincibility(invincibleDuration); ??????

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        // ?? ���� ���� ���� ??
        // DisableInvincibility(); ??????
    }
}