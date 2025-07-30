using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private PlayerMovingHandler movingHandler;

    private void Awake() 
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        movingHandler = GetComponent<PlayerMovingHandler>();
    }

    private void FixedUpdate()
    {
        movingHandler.HandleMove(inputHandler.MoveInput); // ��ǲ�ڵ鷯���� ���� ��ǲ�� ������ �����ڵ鷯�� ó��
    }

    private void Update() // �뽬�����ϸ� �뽬
    {
        if (inputHandler.DashHeld && movingHandler.CanDash(inputHandler.MoveInput))
            movingHandler.TryDash();
    }
}