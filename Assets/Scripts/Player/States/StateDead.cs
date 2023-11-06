using UnityEngine;

public class StateDead : StateBase
{
    private Player player;
    public override void OnStateEnter(params object[] objs)
    {
        player = (Player)objs[0];
        player.GetComponent<Animator>().SetBool("Dead", true);
        player.StopVelocity();
    }
    public override void OnStateExit()
    {
        player.GetComponent<Animator>().SetBool("Dead", false);
    }
}
    