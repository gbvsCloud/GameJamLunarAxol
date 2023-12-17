    using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIsGrounded : MonoBehaviour
{
    [SerializeField]
    private Player player;

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
        {    
            player.StopVelocity();
            player.usingSuperJump = false;           
            player.isGrounded = true;
            player.knockbackWorking = false;
            
        }else if(collision.CompareTag("Plataforms")){
            player.StopVelocity();
            player.usingSuperJump = false;     
            player.isGrounded = true;
            player.knockbackWorking = false;
        }
    }*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Map") && player.falling)
        {  
            player.StopVelocity();
            player.isGrounded = true;
            player.usingSuperJump = false;                      
        }
        else if(collision.CompareTag("Plataforms") && player.falling){
            player.StopVelocity();
            player.usingSuperJump = false;           
            player.isGrounded = true;
            player.knockbackWorking = false;
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
