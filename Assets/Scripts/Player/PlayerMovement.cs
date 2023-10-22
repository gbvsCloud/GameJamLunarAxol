using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform attack;
    public float handleAttack = 1.33f;

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private LineRenderer lineRenderer;

    float direction;
    float speed = 6.5f;
    float jumpStrength = 17;
    public bool isGrounded = false;

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

    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        Jump();

        if(direction == 1)
        {
            if (spriteRenderer.flipX == true)
                attack.transform.DOMoveX(transform.position.x + handleAttack, 0f);

            animator.SetBool("Run", true);
            spriteRenderer.flipX = false;
        }
        else if(direction == -1)
        {
            if (spriteRenderer.flipX == false)
                attack.transform.DOMoveX(transform.position.x - handleAttack, 0f);
            animator.SetBool("Run", true);
            spriteRenderer.flipX = true;
        }
        else
            animator.SetBool("Run", false);

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
                superJumpDelay += Time.deltaTime;
                chargingSuperJump = true;

                if(superJumpDelay >= 0.36f)
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
                if (player.climb)
                {
                    player.climb = false;
                    player.GetComponent<Rigidbody2D>().gravityScale = player.GetGravity();
                }
                rigidBody.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
                jumpAudioSource.PlayRandomSound();
            }

            if (superJumpDelay >= 0.35f && !(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space)))
            {
                if(player.climb)
                {
                    player.climb = false;
                    player.GetComponent<Rigidbody2D>().gravityScale = player.GetGravity();
                }
                SuperJump();
                superJumpDelay = 0;
                chargingSuperJump = false;
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
            rigidBody.AddForce(30 * superJumpDelay * direcao, ForceMode2D.Impulse);
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

        if (!usingSuperJump && Input.GetKey(KeyCode.S))
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
        else if(!usingSuperJump && !Input.GetKey(KeyCode.S))
        {
            rigidBody.velocity = new Vector2(direction * speed, rigidBody.velocity.y);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        usingSuperJump = false;
    }

}
