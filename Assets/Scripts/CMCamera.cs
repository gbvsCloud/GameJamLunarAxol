using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private PolygonCollider2D cameraConfiner;
    Player player;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        cameraConfiner = GameObject.FindGameObjectWithTag("CameraBounds").GetComponent<PolygonCollider2D>();
        virtualCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = cameraConfiner;

        virtualCamera.LookAt = player.transform;
        virtualCamera.Follow = player.transform;
        transform.position = player.transform.position;      
    }
}
