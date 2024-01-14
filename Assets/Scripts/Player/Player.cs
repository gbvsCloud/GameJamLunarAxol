using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : EntityBase
{   
    [SerializeField]
    public GameManager gameManager;
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
    [Header("Tipos de Ataques")]


    #region AudioSource
    [SerializeField]
    private RandomSound damageSound;
    [SerializeField]
    private RandomSound jumpAudioSource;
    #endregion 
    
    #region AttackArea 
    [Header("Attack Area")] 
    [SerializeField]
    public GameObject[] attacks;
    public int currentAttack = 0;
    public float attackOvertime = 0;
    public bool attacking = false;

    #endregion

    #region MovementVariables
    [Header("Movement Area")] 
    public SuperJumpEcho echo;
    public bool isGrounded = true;
    public float xInput;
    float speed = 6.5f;
    float jumpStrength = 12f;
    public bool usingSuperJump;
    private float fallGravityAcceleration = 2f;
    private float maxFallSpeed = -25;
    private float maxRiseSpeed = 50;
    public bool falling = false;
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
        _currentGravity = GetComponent<Rigidbody2D>().gravityScale;     
        Init(GameManager.RetrievePlayerHealth());


    }

    protected override void Update()
    {
        base.Update();

        stateMachine.Update();
        xInput = Input.GetAxisRaw("Horizontal");
        QuicklyFall();
        if (invunerableTime > 0) invunerableTime -= Time.deltaTime;

        if(rigidBody.velocity.y < 0)
            falling = true;
        if(attackOvertime > 0){
            attackOvertime -= Time.deltaTime;
        }else if(attackOvertime <= 0 && !attacking){
            currentAttack = 0;
        }
        
    }

    private void FixedUpdate()
    {
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
                Knockback(collision.transform, 20);
                collision.transform.GetComponent<Enemy>().HitPlayer(transform);
                invunerableTime = 1.5f;

            }

        }
        else if (collision.transform.CompareTag("TornTiles"))
        {
            
            TakeDamage();
            gameManager?.ReturnToLastCheckpoint();
            rigidBody.gravityScale = 1;
            stateMachine.SwitchState(States.DEAD, this);
            invunerableTime = 1.5f;
        }
    }
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.CompareTag(_tagEnemy))
        {
            
            if (invunerableTime <= 0)
            {
                TakeDamage();
                damageSound.PlayRandomSoundWithVariation();
                Knockback(other.transform, 20);
                other.transform.GetComponent<Enemy>().HitPlayer(transform);
                invunerableTime = 1.5f;
            }

        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Checkpoint")){
            Debug.Log("New checkpoint");
            gameManager?.NewCheckPoint(other.transform);
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
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
    }
    public void Crouch()
    {
        spriteRenderer.flipX = !(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x);

    }

    public void Jump()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpStrength);
        falling = false;

        if(jumpAudioSource != null) jumpAudioSource.PlayRandomSoundWithVariation();
    }
    #endregion

    #region Ataque


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

    IEnumerator IAttack(){
        
        if(currentAttack == 0){
            yield return new WaitForSeconds(0.1f);
            GameObject attack = Instantiate(attacks[0]);
            if(!spriteRenderer.flipX)
                attack.transform.position = new Vector2(transform.position.x + 1.2f, transform.position.y - 0.6f);
            else
                attack.transform.position = new Vector2(transform.position.x - 1.2f, transform.position.y - 0.6f);
            attack.GetComponent<RapierAttack>().player = this;
        }else if(currentAttack == 1){
            yield return new WaitForSeconds(0.15f);
            GameObject attack = Instantiate(attacks[1]);
            if(!spriteRenderer.flipX)
                attack.transform.position = new Vector2(transform.position.x + 1f, transform.position.y - 0.3f);
            else
                attack.transform.position = new Vector2(transform.position.x - 1f, transform.position.y - 0.3f);
            attack.GetComponent<RapierAttack>().player = this;

        }else if(currentAttack == 2){
            yield return new WaitForSeconds(0.15f);
            GameObject attack = Instantiate(attacks[2]);
            if(!spriteRenderer.flipX)
                attack.transform.position = new Vector2(transform.position.x + 1f, transform.position.y - 0.3f);
            else
                attack.transform.position = new Vector2(transform.position.x - 1f, transform.position.y - 0.3f);
            attack.GetComponent<RapierAttack>().player = this;

        }

    }

    public void Attack()
    {
        StartCoroutine(IAttack());
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        gameManager.CloseEye();
        damageSound.PlayRandomSound();
        if (health <= 0)
        {
            rigidBody.gravityScale = 1;

        }

    }
    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(2);
        GameManager.playerHealth = 3;
        SceneManager.LoadScene("GameOver");
    }

    #endregion

    public void SuperJump(float superJumpCharge)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcao = (mousePos - (Vector2)transform.position).normalized;
    
        falling = false;    

        if (superJumpCharge < 0.85f)    
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
