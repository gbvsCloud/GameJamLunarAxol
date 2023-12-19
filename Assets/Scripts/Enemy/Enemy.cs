using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : EntityBase
{
    private Transform currentPatrolPosition;

    protected float speed = 4;

    public bool goingRight = true;

    [SerializeField] private RandomSound damageAudio;

     #region StateMachine
    public enum States
    {
        IDLE,
        WALKING
    }
    public StateMachine<States> stateMachine;



    #endregion

    public void Start()
    {
        Init(2);
    }

    // Update is called once per fram

    public void HitPlayer(Transform playerPosition){
        if(playerPosition.position.x > transform.position.x && goingRight){
            goingRight = !goingRight;
        }else if(playerPosition.position.x < transform.position.x && !goingRight){
            goingRight = !goingRight;
        }
    }

    protected override void Update()
    {
        base.Update();
        if(goingRight)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        damageAudio?.PlayRandomSoundWithVariation();

    }

    public override void Knockback(Transform knockbackOrigin, float strength)
    { 
        if(knockbackOrigin.position.x < transform.position.x)
            rigidBody.AddForce(new Vector2(knockbackOrigin.position.x, 2) * strength, ForceMode2D.Impulse);
        else
            rigidBody.AddForce(new Vector2(-knockbackOrigin.position.x, 2) * strength, ForceMode2D.Impulse);
        knockbackTimer = knockbackStunDuration;
        knockbackWorking = true;
    }
    public virtual void TurnAround(){

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ReturnEnemy"))
        {
            goingRight = !goingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "TornTiles")
        {
            Death();
        }
    }

}
