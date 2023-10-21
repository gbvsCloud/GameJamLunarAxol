using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Player player;

    [SerializeField]
    private LineRenderer lineRenderer;

    float direction;
    float speed = 5;
    float jumpStrength = 20;
    public bool isGrounded = false;

    public float superJumpDelay = 0;
    public bool chargingSuperJump = false;
    public Vector2 mouseJumpTarget;
    bool usingSuperJump = false;

    [SerializeField]
    private SuperJumpProjection projection;
    [SerializeField]
    private SuperJumpEcho echo;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        

        Jump();

        if(direction == 1)
        {
            spriteRenderer.flipX = false;
        }
        if(direction == -1)
        {
            spriteRenderer.flipX = true;
        }

        if(usingSuperJump)
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
                    }
                    else
                    {
                        velocity = 30 * direcao;
                    }

                    projection.SimulateTrajectory(echo, transform.position, velocity);
                }

            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("apertou");
                rigidBody.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
            }


            if (superJumpDelay >= 0.35f && !(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space)))
            {
                Debug.Log("super pulo");
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
