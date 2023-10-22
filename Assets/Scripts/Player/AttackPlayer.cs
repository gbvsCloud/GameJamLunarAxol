using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class AttackPlayer : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.R;
    public Animator animator;
    private bool attack = false;
    AnimatorStateInfo stateInfo;
    float normalizedTime;

    //privates
    private string _tagEnemy = "Enemy";

    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            StartCoroutine(Attack());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == _tagEnemy && attack)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage();
        }
              
    }

    private IEnumerator Attack()
    {
        attack = true;
        animator.SetTrigger("Attack");
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        normalizedTime = stateInfo.normalizedTime;
        yield return new WaitForSeconds(normalizedTime);
        attack = false;
    }
}
