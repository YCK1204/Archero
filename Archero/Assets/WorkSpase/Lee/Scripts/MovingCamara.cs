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

   void Update()       // late�� �ٸ� �۾��� ���� �Ϸ�ȵ� �������� ������Ʈ�ȴ�
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
                // ���� �÷��̾ ������ �ڷ� ����
                return;
            }
        }
    }
}
