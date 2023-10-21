using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TongueManager : MonoBehaviour
{
    public StateMachine stateMachine;
    public GameObject player;
    [Header("Língua")]
    public GameObject tongue;
    public LineRenderer lineRenderer;
    public float timeTongueAnimation = .5f;
    private void Start()
    {
        lineRenderer.positionCount = 2;
        tongue.transform.position = player.transform.position;
    }
    private void Update()
    {
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, tongue.transform.position);
    }

    public void TonguePosition()
    {
        TongueMotion(player);
    }
    public void TongueMotion(GameObject target)
    {
        Vector2 jointVector = (Vector2)target.transform.position;
        tongue.transform.DOMove(jointVector, timeTongueAnimation);
    }
    public void TongueAnimationStart(GameObject target, TriggerSwing trigger)
    {
        StartCoroutine(TongueAnimationStartCoroutine(target, trigger));
    }

    private IEnumerator TongueAnimationStartCoroutine(GameObject target, TriggerSwing trigger)
    {
        TongueMotion(target);
        yield return new WaitForSeconds(timeTongueAnimation);
        stateMachine.SwitchState(StateMachine.States.SWING, trigger);
    }
    public void TongueAnimationEnd(GameObject target)
    {
        TongueMotion(target);
        player.GetComponent<Rigidbody2D>().gravityScale = player.GetComponent<Player>().GetGravity();
    }
}