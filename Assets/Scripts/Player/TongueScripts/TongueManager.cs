using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TongueManager : MonoBehaviour
{
    public GameObject player;
    [Header("Língua")]
    public GameObject tongue;
    public LineRenderer lineRenderer;
    public float timeTongueAnimation = .5f;
    private void Start()
    {
        lineRenderer.positionCount = 2;
    }
    private void Update()
    {
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, tongue.transform.position);
    }

    public void TonguePosition()
    {
        tongue.transform.position = player.transform.position;
    }
    public void TongueMotion(GameObject target)
    {
        Vector2 jointVector = (Vector2)target.transform.position;
        tongue.transform.DOMove(jointVector, timeTongueAnimation);
    }
    public void TongueAnimationStart(GameObject target)
    {
        StartCoroutine(TongueAnimationStartCoroutine(target));
    }

    private IEnumerator TongueAnimationStartCoroutine(GameObject target)
    {
        TongueMotion(target);
        yield return new WaitForSeconds(timeTongueAnimation);
        
    }
    public void TongueAnimationEnd(GameObject target)
    {
        TongueMotion(target);
    }
}