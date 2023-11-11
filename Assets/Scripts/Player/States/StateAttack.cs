
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StateAttack : StateBase
{
    private Player player;
    private Rigidbody2D rigidBody;

    public override void OnStateEnter(params object[] objs)
    {
        player = (Player)objs[0];
        rigidBody = player.GetComponent<Rigidbody2D>();
        player.Attack();
    }  
    public override void OnStateStay()
    {
        CheckStateSwitch();
    }

    public override void FixedUpdate()
    {
        player.Run();
    }


    public override void CheckStateSwitch()
    {   
        if(player.xInput == 0 && player.isGrounded){
            player.stateMachine.SwitchState(Player.States.IDLE, player);
        }
        else if(rigidBody.velocity.y < 0 && !player.isGrounded){
            player.stateMachine.SwitchState(Player.States.FALLING, player);
        }else if(player.xInput != 0 && player.isGrounded){
            player.stateMachine.SwitchState(Player.States.RUNNING, player);
        }
    }




    

}