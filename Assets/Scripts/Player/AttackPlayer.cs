using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackPlayer : MonoBehaviour
{
    public KeyCode keyCodeAttack = KeyCode.R;
    public KeyCode keyCodeLick = KeyCode.Q;
    public Animator animator;

    //privates
    private string _tagEnemy = "Enemy";
    private bool _attack = false;
    private AnimatorStateInfo stateInfo;
    private float _normalizedTime;

    void Update()
    {
        if (Input.GetKeyDown(keyCodeAttack))
        {
            StartCoroutine(Attack());
        }
        if (Input.GetKeyDown(keyCodeLick))
        {
            Lick();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == _tagEnemy && _attack)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage();
        }
    }

    private void Lick()
    {
        animator.SetTrigger("Lick");
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
