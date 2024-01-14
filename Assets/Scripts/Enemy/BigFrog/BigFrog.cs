using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BigFrog : EntityBase
{

    public Player player;
    Animator animator;
    float jumpForce = 28;
    public GameObject waterWavePrefab;
    public bool jumped = false;
    public bool isGrounded = false;
    public float jumpCount = 0;

    public bool bossStunned = false;
    IEnumerator IEJump = null;
    public BlackScreen blackScreen;
    public enum States
    {
        IDLE,
        JUMP,
        GROUNDED,
        STUNNED
    }
    public StateMachine<States> stateMachine;

    private void Start() {
        Init(30);
        player = FindObjectOfType<Player>().GetComponent<Player>();
        animator = GetComponent<Animator>();
        stateMachine = new StateMachine<States>();
        stateMachine.Init();
        stateMachine.RegisterStates(States.IDLE, new BigFrogIdle());
        stateMachine.RegisterStates(States.JUMP, new BigFrogJump());
        stateMachine.RegisterStates(States.GROUNDED, new BigFrogGrounded());
        stateMachine.RegisterStates(States.STUNNED, new BigFrogStunned());
        stateMachine.SwitchState(States.IDLE, this);

    }

    protected override void Update()
    {
        base.Update();
        stateMachine.Update();
        if(health <= 0){
            blackScreen.TurnBlack();
        }
        //Debug.Log(stateMachine.CurrentState);
    }

    /*public void StartBattle(){
        StartCoroutine(IStartBattle());
    }

    public IEnumerator IStartBattle(){
        yield return new WaitForSeconds(1);
        stateMachine.SwitchState(States.JUMP, this);
    }*/
    public void Jump(){    
        if(IEJump == null){
            IEJump = IJump();
            StartCoroutine(IEJump);
        }
    }
    public IEnumerator IJump(){
        if(health > maxHealth * 0.75f){
            yield return new WaitForSeconds(0.85f);
        }else if(health > maxHealth * 0.5f){
            yield return new WaitForSeconds(1.75f);
        }else{
            yield return new WaitForSeconds(2.5f);
        }

        stateMachine.SwitchState(States.JUMP, this);
        IEJump = null;
    }
    public void Stunned(){  
        StartCoroutine(IStunned());
    }

    public IEnumerator IStunned(){
        bossStunned = true;
        yield return new WaitForSeconds(3.5f);
        bossStunned = false;
        jumpCount = 0;
        stateMachine.SwitchState(States.JUMP, this);

    }
    public void CreateWaves(){
        StartCoroutine(ICreateWaves());
    }

    public IEnumerator ICreateWaves(){
        
        int times = 1;

        if(health < maxHealth * 0.7f)
            times++;
        if(health < maxHealth * 0.4f)
            times++;
        if(health < maxHealth * 0.2f)
            times++;
         
        for(int i = 0; i < times; i++){
            if(!isGrounded) break; 
            GameObject leftWave = Instantiate(waterWavePrefab);
            leftWave.transform.position = new Vector2(transform.position.x - 2.7f, transform.position.y - 0.5f);
            leftWave.GetComponent<WaterWave>().goingLeft = true;

            GameObject rightWave = Instantiate(waterWavePrefab);
            rightWave.transform.position = new Vector2(transform.position.x + 2.7f, transform.position.y - 0.5f);
            rightWave.GetComponent<WaterWave>().goingLeft = false;
            yield return new WaitForSeconds(0.15f);
        }
    }


    public void JumpToPlayer(){
        float angleInRadians = Mathf.Deg2Rad * 60;

        float forceX = jumpForce * Mathf.Cos(angleInRadians);
        float forceY = jumpForce * Mathf.Sin(angleInRadians);

        if(player.transform.position.x > transform.position.x)
            rigidBody.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
        else
            rigidBody.AddForce(new Vector2(-forceX, forceY), ForceMode2D.Impulse);
    }
}
