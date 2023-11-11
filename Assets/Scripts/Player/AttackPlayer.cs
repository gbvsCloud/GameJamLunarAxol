using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackPlayer : MonoBehaviour
{
    public KeyCode keyCodeLick = KeyCode.Q;
    public Animator animator;

    [SerializeField] private SpriteRenderer spriteRenderer;

    //privates
    private string _tagEnemy = "Enemy";
    private bool _attack = false;
    private AnimatorStateInfo stateInfo;
    private float _normalizedTime;
    bool canAttack = true;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            StartCoroutine(Attack());
        }
        if (Input.GetKeyDown(keyCodeLick))
        {
            Lick();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == _tagEnemy && _attack)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage();
            collision.gameObject.GetComponent<Enemy>().Knockback(transform.parent, 2);
            _attack = false;
        }
    }

    private void Lick()
    {
        animator.SetTrigger("Lick");
    }

    private IEnumerator Attack()
    {
        Debug.Log("ataque");
        _attack = true;
        canAttack = false;
        spriteRenderer.enabled = true;
        animator.SetTrigger("Attack");
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);      
        _normalizedTime = stateInfo.normalizedTime;
        yield return new WaitForSeconds(0.25f);
        canAttack = true;
        _attack = false;
        spriteRenderer.enabled = false;
    }



}
