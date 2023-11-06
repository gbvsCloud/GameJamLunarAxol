
using UnityEngine;

public class StateJump : StateBase
{
    private Player player;
    private float jumpMaxDuration = 0.25f;
    private float jumpHoldTime;
    private bool keyReleased;
    public override void OnStateEnter(params object[] objs)
    {
        player = (Player)objs[0];
        player.GetComponent<Animator>().StopPlayback();
        player.GetComponent<Animator>().SetTrigger("Jump");
        player.Jump();
        jumpHoldTime = 0;
        keyReleased = false;
        player.isGrounded = false;
    }  
    public override void OnStateStay()
    {
        CheckStateSwitch();
    }

    public override void FixedUpdate()
    {
        player.Run();
        if (!keyReleased && Input.GetKey(KeyCode.Space) && jumpHoldTime < jumpMaxDuration)
        {
            player.Jump();
            jumpHoldTime += Time.deltaTime;
        }
        if (!Input.GetKey(KeyCode.Space))
        {
            keyReleased = true;
        }
    }


    public override void CheckStateSwitch()
    {
        if (player.isGrounded)
        {
            player.stateMachine.SwitchState(Player.States.IDLE, player);
        }
    }

    public override void OnStateExit()
    {
        player.isGrounded = false;
    }
}