using UnityEngine;

public class StateRun : StateBase
{
    private Player player;
    public override void OnStateEnter(params object[] objs)
    {
        player = objs[0] as Player;
        player.GetComponent<Animator>().SetBool("Run", true);
    }
    public override void OnStateExit()
    {
        player.GetComponent<Animator>().SetBool("Run", false);
        player.Run();
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
        if(player.xInput == 0)
        {
            player.stateMachine.SwitchState(Player.States.IDLE, player);
        }else if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded)
        {
            player.stateMachine.SwitchState(Player.States.JUMPING, player);
        }
        else if (Input.GetKey(KeyCode.S) && player.isGrounded)
        {
            player.stateMachine.SwitchState(Player.States.CROUCH, player);
        }else if(Input.GetButtonUp("Fire1") && !player.attacking){
            player.stateMachine.SwitchState(Player.States.ATTACK, player);
        }
    }

}