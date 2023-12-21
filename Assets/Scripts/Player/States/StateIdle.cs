using Unity.VisualScripting;
using UnityEngine;

public class StateIdle : StateBase
{ 
    private Player player;
    public override void OnStateEnter(params object[] objs)
    {
        
        player = objs[0] as Player;   
        player?.GetComponent<Animator>().SetTrigger("Idle");
    }

    public override void OnStateStay()
    {
        CheckStateSwitch();
    }
    public override void OnStateExit()
    {
        player?.GetComponent<Animator>().ResetTrigger("Idle");
    }
    public override void CheckStateSwitch()
    {   
        if((int)player.rigidBody.velocity.y < 0){
            player.stateMachine.SwitchState(Player.States.FALLING, player);
        }
        else if(player.xInput != 0 && !player.usingSuperJump)
        {
            player.stateMachine?.SwitchState(Player.States.RUNNING, player);
        }else if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded)
        {
            player.stateMachine?.SwitchState(Player.States.JUMPING, player);
        }else if(Input.GetKey(KeyCode.S) && player.isGrounded)
        {
            player.stateMachine?.SwitchState(Player.States.CROUCH, player);
        }else if(Input.GetButtonUp("Fire1") && !player.attacking){
            player.stateMachine?.SwitchState(Player.States.ATTACK, player);
        }
    }

}