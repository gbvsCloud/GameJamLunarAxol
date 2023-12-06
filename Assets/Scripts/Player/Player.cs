
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
    private Animator animator;

    //privates
    [SerializeField] private float _currentGravity;
    private string _tagEnemy = "Enemy";

    private float invunerableTime = 0;

    [Header("Ataque")]
    public float attackRange = .5f;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [Header("Teclas de Ataque")]
    public KeyCode keyCodeAttack = KeyCode.R;
    public KeyCode keyCodeLick = KeyCode.Q;
    [Header("Tipos de Ataques")]
    public List<string> attacks;

    private int _indexAttack = 0;

    #region AudioSource
    [SerializeField]
    private RandomSound damageSound;
    [SerializeField]
    private RandomSound jumpAudioSource;
    #endregion 
    
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
        stateMachine.RegisterStates(States.JUMPING, new StateJump());
        stateMachine.RegisterStates(States.CROUCH, new StateCrouch());
        stateMachine.RegisterStates(States.FALLING, new StateFalling());
        stateMachine.RegisterStates(States.ATTACK, new StateAttack());

        stateMachine.SwitchState(States.IDLE, this);
    }

    #endregion

    void Start()
    {
        Init(3);
        _currentGravity = GetComponent<Rigidbody2D>().gravityScale;
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log(_indexAttack + "e" + attacks.Count);
        if (Input.GetKeyDown(keyCodeAttack))
        {
            Attack(attacks[_indexAttack]);
            
            if(_indexAttack < attacks.Count - 1)
                _indexAttack ++;
        }
        if (Input.GetKeyDown(keyCodeLick)) Lick();

        stateMachine.Update();
        xInput = Input.GetAxisRaw("Horizontal");
        QuicklyFall();
        if (invunerableTime > 0) invunerableTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(keyCodeAttack))
        {
            Attack("Attack");
        }
        if (Input.GetKeyDown(keyCodeLick))
        {
            Lick();
        }
        stateMachine.FixedUpdate();
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, maxFallSpeed, maxRiseSpeed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(_tagEnemy))
        {

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

    #region Movement
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

    public void Jump()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpStrength);


        if(jumpAudioSource != null) jumpAudioSource.PlayRandomSoundWithVariation();
    }
    #endregion

    #region Ataque
    public void Attack(string animation)
    {
        animator.SetTrigger(animation);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemies in hitEnemies)
        {
            enemies.GetComponent<Enemy>()?.TakeDamage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void Lick()
    {
        animator.SetTrigger("Lick");
    }

    #endregion

    #region Combat

    public override void Knockback(Transform knockbackOrigin, float strength)
    {
        base.Knockback(knockbackOrigin, strength);
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
    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    #endregion

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

}
