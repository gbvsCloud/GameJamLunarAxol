using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsGrounded : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement movementScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        movementScript.isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        movementScript.isGrounded = false;
    }


}
