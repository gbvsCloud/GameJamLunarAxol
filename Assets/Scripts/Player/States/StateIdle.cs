using UnityEngine;

public class StateIdle : StateBase
{
    private TongueManager manager;
    private Player player;
    public override void OnStateEnter(params object[] objs)
    {
        manager = (TongueManager)objs[0];
        player = (Player)objs[1];
        player.GetComponent<Animator>().SetTrigger("Idle");
    }
    public override void OnStateStay()
    {
        manager.TonguePosition();
    }
}