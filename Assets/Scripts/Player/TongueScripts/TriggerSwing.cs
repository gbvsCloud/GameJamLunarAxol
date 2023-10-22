using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TriggerSwing : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.E;

    [Header("Animação da Língua")]
    public GameObject jointPosition;
    public TongueManager manager;
    [Header("Animação do Swing")]
    public float swingDuration = .5f;
    public float distLerp = 3f;
    public List<GameObject> listLerps = new List<GameObject>();
    public GameObject player;
    //privates
    private string _tagPlayer = "Player";
    private int _index;
    private Tween tween;
    private Vector2 playerVector;

    private void Update()
    {
        if(player != null)
            playerVector = player.transform.position;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == _tagPlayer)
        {
            if (Input.GetKey(keyCode))
            {
                TongueAnimationStart();
            }
        }
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void TongueAnimationStart()
    {
        manager.TongueAnimationStart(jointPosition, this);
        player.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
    public void TongueAnimationEnd()
    {
        manager.TongueAnimationEnd(player);
        player.GetComponent<Rigidbody2D>().gravityScale = player.GetComponent<Player>().GetGravity();
    }
    public void MotionBetween()
    {
        Vector2 lerpTarget = (Vector2)listLerps[_index].transform.position;
        if (_index >= listLerps.Count - 1 && Vector2.Distance(playerVector, lerpTarget) < distLerp)
        {
            TongueAnimationEnd();
        }
        else if (_index < listLerps.Count - 1 && Vector2.Distance(playerVector, lerpTarget) < distLerp)
        {
            _index++;
        }
        if (Vector2.Distance(playerVector, lerpTarget) >= distLerp) SwingMotion(lerpTarget);
    }
    public void SwingMotion(Vector2 lerpPosition)
    {
        tween.SetEase(Ease.Linear);
        tween = player.transform.DOMove(lerpPosition, swingDuration);
    }
}