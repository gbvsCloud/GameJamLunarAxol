using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriggerSwing : MonoBehaviour
{
    [Header("Língua")]
    public LineRenderer lineRenderer; 
    public GameObject jointPosition; // Ponto de ancoragem final da lingua
    public GameObject tongue;
    public float timeTongueAnimation = .5f;

    [Header("Animação Swing")]
    public float swingDuration = .5f;
    public float distLerp = 1f;
    public GameObject player;
    public List<GameObject> listLerps = new List<GameObject>();

    //privates
    private string _tagPlayer = "Player";
    private bool lingua = false;
    private Vector2 playerVector;
    private int _index;
    private Tween tween;

    private void Start()
    {

    }

    private void Update()
    {
        if (player != null) return;
            
        playerVector = player.transform.position;

        if (jointPosition != null)
        {
            if(lingua == true)
            {
                tongue.transform.SetParent(this.transform);
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, player.transform.position);
                lineRenderer.SetPosition(1, tongue.transform.position);
                MotionBetween();
            }
            else
            {
                tongue.transform.SetParent(player.transform);

            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == _tagPlayer)
        {
            if (Input.GetKey(KeyCode.E))
            {
                StartCoroutine(TongueAnimationStart(jointPosition));
                player.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }

    public void MotionBetween()
    {
        Vector2 lerpTarget = (Vector2)listLerps[_index].transform.position;

        if (_index >= listLerps.Count - 1 && Vector2.Distance(playerVector, lerpTarget) < distLerp / 2)
        {
            player.GetComponent<Rigidbody2D>().gravityScale = player.GetComponent<Player>().GetGravity();
            lingua = false;
            _index = 0;
            StartCoroutine(TongueAnimationEnd(player));
        }
        else if (_index < listLerps.Count - 1 && Vector2.Distance(playerVector, lerpTarget) < distLerp)
        {
            _index++;
        }

        if(Vector2.Distance(playerVector, lerpTarget) >= distLerp) SwingMotion(lerpTarget);
    }

    public void SwingMotion(Vector2 lerpPosition)
    {
        tween = player.transform.DOMove(lerpPosition, swingDuration);
        tween.SetEase(Ease.Linear);
    }

    public void TongueMotion(GameObject target)
    {
        Vector2 jointVector = (Vector2)target.transform.position;
        tongue.transform.DOMove(jointVector, timeTongueAnimation);
    }

    IEnumerator TongueAnimationStart(GameObject target)
    {
        TongueMotion(target);
        yield return new WaitForSeconds(timeTongueAnimation);
        lingua = true;
    }

    IEnumerator TongueAnimationEnd(GameObject target)
    {
        TongueMotion(target);
        yield return new WaitForSeconds(timeTongueAnimation);
        lineRenderer.positionCount = 0;
    }
}
