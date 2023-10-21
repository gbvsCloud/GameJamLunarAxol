using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rBody;

    [SerializeField]
    private SpriteRenderer sRenderer;

    [SerializeField]
    private PlayerScript playerScript;


    float direction;
    float speed = 5;
    float jumpStrength = 20;
    public bool isGrounded = false;

    public float superJumpDelay = 0;
    public bool chargingSuperJump = false;
    public Vector2 mouseJumpTarget;
    bool usingSuperJump = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.S))
        {
            direction = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            direction = 0;
        }

        Jump();

        if(direction == 1)
        {
            sRenderer.flipX = false;
        }
        if(direction == -1)
        {
            sRenderer.flipX = true;
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
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
        {  
            superJumpDelay += Time.deltaTime;
            chargingSuperJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("apertou");
            rBody.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
        }

        if (superJumpDelay > 0.35f && !(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space)))
        {
            Debug.Log("super pulo");           
            SuperJump();
            superJumpDelay = 0;
            chargingSuperJump = false;
        }

        if (!chargingSuperJump) superJumpDelay = 0;

        
    }

    public void SuperJump()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcao = (mousePos - (Vector2)transform.position).normalized;

        if (superJumpDelay < 1)
        {
            rBody.AddForce(30 * superJumpDelay * direcao, ForceMode2D.Impulse);
        }
        else
        {
            rBody.AddForce(30 * direcao, ForceMode2D.Impulse);
        }
        isGrounded = false;
        usingSuperJump = true;
        mouseJumpTarget = mousePos;
    }


    private void FixedUpdate()
    {    
        if(!usingSuperJump) rBody.velocity = new Vector2(direction * speed, rBody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        usingSuperJump = false;
    }

}
