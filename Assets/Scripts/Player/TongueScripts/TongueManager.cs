using DG.Tweening;
using UnityEngine;

public class TongueManager : Singleton<TongueManager>
{
    public Player player;
    public GameObject playerObject;
    public StateMachine stateMachine;
    [Header("Língua")]
    public GameObject tongue;
    public LineRenderer lineRenderer;
    public float timeTongueAnimation = .5f;

    private void Start()
    {
        lineRenderer.positionCount = 2;
        TonguePosition();
    }
    private void Update()
    {
        if(lineRenderer != null)
        {
            lineRenderer.SetPosition(0, playerObject.transform.position);
            lineRenderer.SetPosition(1, tongue.transform.position);
        }
    }

    public void TonguePosition()
    {
        if (tongue != null) tongue.transform.position = playerObject.transform.position;
    }
    public void TongueMotion(GameObject target)
    {
        Vector2 jointVector = (Vector2)target.transform.position;
        if (tongue != null) tongue.transform.DOMove(jointVector, timeTongueAnimation);
    }
    public void TongueAnimationStart(GameObject target, TriggerSwing trigger)
    {
        TongueMotion(target);
        stateMachine.SwitchState(StateMachine.States.SWING, trigger, player);
    }
    public void TongueAnimationEnd(GameObject target)
    {
        TongueMotion(target);
        stateMachine.SwitchState(StateMachine.States.IDLE, this, player);
    }
}