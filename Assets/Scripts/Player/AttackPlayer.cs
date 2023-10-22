using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackPlayer : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.R;
    public Animator animator;

    //privates
    private string _tagEnemy = "Enemy";
    private bool _attack = false;
    private AnimatorStateInfo stateInfo;
    private float _normalizedTime;

    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            StartCoroutine(Attack());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == _tagEnemy && _attack)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage();
        }
    }

    private IEnumerator Attack()
    {
        _attack = true;
        animator.SetTrigger("Attack");
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        _normalizedTime = stateInfo.normalizedTime;
        yield return new WaitForSeconds(_normalizedTime);
        _attack = false;
    }
}
