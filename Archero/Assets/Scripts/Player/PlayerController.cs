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
        movingHandler.HandleMove(inputHandler.MoveInput); // 인풋핸들러에서 받은 인풋을 가져와 무빙핸들러로 처리
    }

    private void Update() // 대쉬가능하면 대쉬
    {
        if (inputHandler.DashHeld && movingHandler.CanDash(inputHandler.MoveInput))
            movingHandler.TryDash();
    }
}