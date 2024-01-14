using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCrouch : StateBase
{
    private Player player;
    private float superJumpCharge;
    private bool superJumpTriggered;
    SuperJumpProjection projection;
    public override void OnStateEnter(params object[] objs)
    {
        player = (Player)objs[0];
        player.GetComponent<Animator>().SetTrigger("Idle");
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        projection = player.GetComponent<SuperJumpProjection>();
        superJumpCharge = 0;
        superJumpTriggered = false;
    }

    public override void OnStateStay()
    {
        CheckStateSwitch();
        if(!player.isGrounded)
            return;
        player.Crouch(); 

       
        if(Input.GetKey(KeyCode.Space))
        {
            if(superJumpCharge < 0.85f)superJumpCharge += Time.deltaTime;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direcao = (mousePos - (Vector2)player.transform.position).normalized;
            Vector2 velocity = 30 * superJumpCharge * direcao;
            projection?.SimulateTrajectory(player.echo, player.transform.position, velocity);
        }
        else
        {
            if(superJumpCharge > 0.10f)
            {
                player.SuperJump(superJumpCharge);
                player.usingSuperJump = true;
                superJumpTriggered = true;
                projection.DisableTrajectory();
            }
            superJumpCharge = 0;
        }
        
    }

    public override void CheckStateSwitch()
    {
        if (Input.GetKeyUp(KeyCode.S) && !superJumpTriggered)
        {
            player.stateMachine.SwitchState(Player.States.IDLE, player);
        }else if (superJumpTriggered && !player.usingSuperJump)
        {
            player.stateMachine.SwitchState(Player.States.IDLE, player);
        }
        projection.DisableTrajectory();

    }
}
