using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform attack;

    [SerializeField]
    private LineRenderer lineRenderer;

    public float handleAttack = 1.33f;

    private Rigidbody2D rigidBody;

    private SpriteRenderer spriteRenderer;

    private Player player;

    private Animator animator;

    float direction;
    float speed = 6.5f;
    float jumpStrength = 20;
    public bool isGrounded = false;
    public bool canRun = false;
    public bool jumping = false;

    public float superJumpDelay = 0;
    public bool chargingSuperJump = false;
    public Vector2 mouseJumpTarget;
    bool usingSuperJump = false;

    [SerializeField]
    private SuperJumpProjection projection;
    [SerializeField]
    private SuperJumpEcho echo;
    [SerializeField]
    private RandomSound jumpAudioSource;

    private void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canRun)
            direction = Input.GetAxisRaw("Horizontal");
        else
            direction = 0;

        Jump();


        
       /* else if (canRun && !jumping)
            player.stateMachine.SwitchState(Player.States.IDLE, tongueManager, player);*/

        if (usingSuperJump)
        {
            if(Vector2.Distance(transform.position, mouseJumpTarget) < 0.5f)
            {
                usingSuperJump = false;
            }
        }
    }

    public void Jump()
    {
        chargingSuperJump = false;
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
            {
                direction = 0;
                superJumpDelay += Time.deltaTime;
                chargingSuperJump = true;
                canRun = false;
                player.GetComponent<Animator>().StopPlayback();

                if (superJumpDelay >= 0.36f)
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direcao = (mousePos - (Vector2)transform.position).normalized;
                    Vector2 velocity;
                    if (superJumpDelay < 1)
                    {
                        velocity = 30 * superJumpDelay * direcao;
                        projection.maxPhysicsIterations = (int)(25 * superJumpDelay);
                    }
                    else
                    {
                        velocity = 30 * direcao;
                        projection.maxPhysicsIterations = 25;
                    }

                    projection.SimulateTrajectory(echo, transform.position, velocity);
                }

            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                player.stateMachine.SwitchState(Player.States.JUMPING, player);
                rigidBody.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
                jumpAudioSource.PlayRandomSoundWithVariation();
            }

            if (superJumpDelay >= 0.35f && !(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space)))
            {
                SuperJump();
                superJumpDelay = 0;
                chargingSuperJump = false;
                canRun = true;
                player.stateMachine.SwitchState(Player.States.JUMPING, player);
            }
        }
        if (!chargingSuperJump) superJumpDelay = 0;

        
    }

    public void SuperJump()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcao = (mousePos - (Vector2)transform.position).normalized;

        if (superJumpDelay < 1)
        {
            //rigidBody.AddForce(30 * superJumpCharge * direcao, ForceMode2D.Impulse);
        }
        else
        {
            rigidBody.AddForce(30 * direcao, ForceMode2D.Impulse);
        }
        jumpAudioSource.PlayRandomSound();

        isGrounded = false;
        usingSuperJump = true;
        mouseJumpTarget = mousePos;
        lineRenderer.positionCount = 0;
    }


    private void FixedUpdate()
    {
        if(!usingSuperJump) rigidBody.velocity = new Vector2(direction * speed, rigidBody.velocity.y);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        usingSuperJump = false;

        if (!collision.transform.CompareTag("TornTiles"))
        {
            if(jumping)
            {
                player.GetComponent<Animator>().SetTrigger("Landing");
                player.stateMachine.SwitchState(Player.States.IDLE, player);
            }
            canRun = true;
        }
        else
        {
            canRun = false;
        }
    }

}
