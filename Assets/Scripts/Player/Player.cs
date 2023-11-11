
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : EntityBase
{   
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    public PlayerMovement playerMovement;
    [SerializeField]
    private Animator animator;
    [SerializeField] private float _currentGravity;
    private string _tagEnemy = "Enemy";

    #region AudioSource
    [SerializeField]
    private RandomSound damageSound;
    [SerializeField]
    private RandomSound jumpAudioSource;
    #endregion

    private float invunerableTime = 0;

    
    #region AttackArea 
    [Header("Attack Area")] 
    [SerializeField]
    private GameObject firstAttack;
    public int currentAttack = 0;


    #endregion

    #region MovementVariables
    [Header("Movement Area")] 
    public bool isGrounded = false;
    public float xInput;
    float speed = 6.5f;
    float jumpStrength = 12f;
    public bool usingSuperJump;
    private float fallGravityAcceleration = 2f;
    private float maxFallSpeed = -25;
    private float maxRiseSpeed = 50;
    #endregion

    #region StateMachine
    public enum States
    {
        IDLE,
        RUNNING,
        DEAD,
        SWING,
        JUMPING,
        CROUCH,
        FALLING,
        ATTACK
    }
    public StateMachine<States> stateMachine;

    public void Awake()
    {
        stateMachine = new StateMachine<States>();
        stateMachine.Init();
        stateMachine.RegisterStates(States.IDLE, new StateIdle());
        stateMachine.RegisterStates(States.RUNNING, new StateRun());
        stateMachine.RegisterStates(States.DEAD, new StateDead());
        stateMachine.RegisterStates(States.SWING, new StateSwing());
        stateMachine.RegisterStates(States.JUMPING, new StateJump());
        stateMachine.RegisterStates(States.CROUCH, new StateCrouch());
        stateMachine.RegisterStates(States.FALLING, new StateFalling());
        stateMachine.RegisterStates(States.ATTACK, new StateAttack());

        stateMachine.SwitchState(States.IDLE, this);
    }

    public TongueManager manager;
    #endregion

    void Start()
    {
        Init(3);
        _currentGravity = GetComponent<Rigidbody2D>().gravityScale;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.Update();
        xInput = Input.GetAxisRaw("Horizontal");
        QuicklyFall();
        if (invunerableTime > 0) invunerableTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();

        rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, maxFallSpeed, maxRiseSpeed));
    }

    public void QuicklyFall()
    {
        if(rigidBody.velocity.y < 0)
        {
            rigidBody.gravityScale = fallGravityAcceleration;
        }
        else
        {
            rigidBody.gravityScale = 1;
        }
    }
    public void Run()
    {
        if (knockbackTimer > 0) return;


        if (xInput == 1)
        {
            spriteRenderer.flipX = false;
        }
        else if (xInput == -1)
        {
            spriteRenderer.flipX = true;
        }

        rigidBody.velocity = new Vector2(xInput * speed, rigidBody.velocity.y);
    }

    public float GetGravity()
    {
        return _currentGravity;
    }

    public void StopVelocity()
    {
        rigidBody.velocity = Vector2.zero;
    }
    public void Crouch()
    {
        if (xInput == 1)
        {
            spriteRenderer.flipX = false;
        }
        else if (xInput == -1)
        {
            spriteRenderer.flipX = true;
        }

    }

    

    public override void Knockback(Transform knockbackOrigin, float strength)
    {
        base.Knockback(knockbackOrigin, strength);
    }
    public void Jump()
    {
        //rigidBody.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpStrength);
        jumpAudioSource.PlayRandomSoundWithVariation();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(_tagEnemy)) { 

            if (invunerableTime <= 0)
            {
                TakeDamage();
                damageSound.PlayRandomSoundWithVariation();
                Knockback(collision.transform, 25);
                invunerableTime = 1.5f;
            }

        }
        else if (collision.transform.CompareTag("TornTiles"))
        {
            TakeDamage();
            gameManager.ReturnToLastCheckpoint();
            rigidBody.gravityScale = 1;
            damageSound.PlayRandomSoundWithVariation();
            stateMachine.SwitchState(States.DEAD, this);
            invunerableTime = 1.5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Checkpoint"))
        {
            gameManager.NewCheckPoint(collision.transform);
        }
    }
    public override void KnockbackEnd()
    {
        StopVelocity();
    }
    public override void Death()
    {
        stateMachine.SwitchState(Player.States.DEAD, this);
        StartCoroutine(DeathAnimation());
    }

    public void Attack()
    {
        GameObject attack = Instantiate(firstAttack);
        if(!spriteRenderer.flipX)
            attack.transform.position = new Vector2(transform.position.x + 2, transform.position.y);
        else
            attack.transform.position = new Vector2(transform.position.x - 2, transform.position.y);
        attack.GetComponent<RapierAttack>().player = this;

    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        gameManager.CloseEye();
        if (health <= 0)
        {
            rigidBody.gravityScale = 1;
        }

    }

    public void SuperJump(float superJumpCharge)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcao = (mousePos - (Vector2)transform.position).normalized;

        if (superJumpCharge < 1)
        {
            rigidBody.AddForce(30 * superJumpCharge * direcao, ForceMode2D.Impulse);
        }
        else
        {
            rigidBody.AddForce(30 * direcao, ForceMode2D.Impulse);
        }
        jumpAudioSource.PlayRandomSound();

        isGrounded = false;
        usingSuperJump = true;

        //mouseJumpTarget = mousePos;
        //lineRenderer.positionCount = 0;
    }

    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

}
