using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MovingCamera : MonoBehaviour
{
    private GameObject player;
    CinemachineVirtualCamera Cinemachine;
    Vector3 offset;
    Vector3 targetPos;

    private void Awake()
    {
        Cinemachine = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

   void Update()     
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Cinemachine.Follow = player.transform;
            }
            else
            {
                // 아직 플레이어가 없으면 뒤로 빠짐
                return;
            }
        }
    }
}
