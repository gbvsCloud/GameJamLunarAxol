using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    Player player;
    private void Awake()
    {
        player = FindObjectOfType<Player>();     

        virtualCamera.LookAt = player.transform;
        virtualCamera.Follow = player.transform;
        transform.position = player.transform.position;      
    }
}
