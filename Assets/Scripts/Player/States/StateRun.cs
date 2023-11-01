using UnityEngine;

public class StateRun : StateBase
{
    private Player player;
    public override void OnStateEnter(params object[] objs)
    {
        player = (Player)objs[0];
        player.GetComponent<Animator>().SetBool("Run", true);
    }
    public override void OnStateExit()
    {
        player.GetComponent<Animator>().SetBool("Run", false);
    }
}