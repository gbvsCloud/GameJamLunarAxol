using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TongueManager : Singleton<TongueManager>
{
    public GameObject player;
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
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, tongue.transform.position);
        }
    }

    public void TonguePosition()
    {
        if (tongue != null) tongue.transform.position = player.transform.position;
    }
    public void TongueMotion(GameObject target)
    {
        Vector2 jointVector = (Vector2)target.transform.position;
        if (tongue != null) tongue.transform.DOMove(jointVector, timeTongueAnimation);
    }
    public void TongueAnimationStart(GameObject target, TriggerSwing trigger)
    {
        TongueMotion(target);
        stateMachine.SwitchState(StateMachine.States.SWING, trigger);
        //StartCoroutine(TongueAnimationStartCoroutine(target, trigger));
    }

    private IEnumerator TongueAnimationStartCoroutine(GameObject target, TriggerSwing trigger)
    {
        TongueMotion(target);
        yield return new WaitForSeconds(timeTongueAnimation);
        stateMachine.SwitchState(StateMachine.States.SWING, trigger);
    }
    public void TongueAnimationEnd(GameObject target)
    {
        StartCoroutine(TongueAnimationEndCoroutine(target));
    }

    private IEnumerator TongueAnimationEndCoroutine(GameObject target)
    {
        TongueMotion(target);
        yield return new WaitForSeconds(timeTongueAnimation - 0.1f);
        stateMachine.SwitchState(StateMachine.States.IDLE, this);
    }
}