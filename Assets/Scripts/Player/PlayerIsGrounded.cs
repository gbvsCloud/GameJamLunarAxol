using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsGrounded : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(player.knockbackTimer > 0)
        {
            player.knockbackTimer = 0;
        }
        if (collision.CompareTag("Map"))
        {
            player.isGrounded = true;
            if (player.usingSuperJump)
            {
                player.usingSuperJump = false;
                player.StopVelocity();
            } 
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
        {
            player.isGrounded = true;
            if (player.usingSuperJump)
            {
                player.usingSuperJump = false;
                player.StopVelocity();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
        {
            player.isGrounded = false;
        }
    }


}
