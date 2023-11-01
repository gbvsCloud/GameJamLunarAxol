
using UnityEngine;

public class StateJump : StateBase
{
    private Player player;
    public override void OnStateEnter(params object[] objs)
    {
        player = (Player)objs[0];
        player.GetComponent<PlayerMovement>().jumping = true;
        player.GetComponent<Animator>().StopPlayback();
        player.GetComponent<Animator>().SetTrigger("Jump");
    }
    public override void OnStateExit()
    {
        player.GetComponent<PlayerMovement>().jumping = false;
    }
}