using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{   
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private PolygonCollider2D cameraConfiner;

    private void Start() {
        cameraConfiner = GetComponent<PolygonCollider2D>();
        virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.transform.tag == "Player"){
            virtualCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = cameraConfiner;
            Debug.Log("Colisão");
        }
    }


}
