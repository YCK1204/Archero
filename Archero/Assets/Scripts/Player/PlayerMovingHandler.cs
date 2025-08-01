using UnityEngine;
using System.Collections;

public class PlayerMovingHandler : MonoBehaviour
{
    //[SerializeField] private float moveSpeed = 5f; 
    //[SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float acceleration = 15f;

    private bool isDashing = false;
    private float currentSpeed; // ���� ���� ���� �̵� �ӵ� (moveSpeed �Ǵ� dashSpeed ���̸� �ε巴�� ��ȯ)
    private float lastDashTime = -Mathf.Infinity;
    private Rigidbody2D rb;
    private CharacterStats characterStats;

    // public bool IsInvincible { get; private set; } = false;       // �ӽ÷� ���� �뽬�߹��� �Ұ�

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        characterStats = GetComponent<CharacterStats>();
        currentSpeed = characterStats.TotalStats.MoveSpeed;
    }

    public void HandleMove(Vector2 input)
    {
        float targetSpeed = isDashing ? characterStats.TotalStats.DashSpeed : characterStats.TotalStats.MoveSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * acceleration); // ���� �ӵ��� ��ǥ �ӵ��� �ε巴�� ����
        rb.velocity = input * currentSpeed; // ���� �̵�
    }

    public bool CanDash(Vector2 input)
    {
        return !isDashing && input != Vector2.zero && Time.time >= lastDashTime + dashCooldown;
    }

    public void TryDash()
    {
        lastDashTime = Time.time;
        StartCoroutine(DashRoutine());             // �뽬 ��ƾ(�ڷ�ƾ) ����
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
      //  IsInvincible = true;
        yield return new WaitForSeconds(dashDuration); // ���� �ð� ���� �뽬 ����

        isDashing = false;
      //  IsInvincible = false;
        yield break;
    }
}