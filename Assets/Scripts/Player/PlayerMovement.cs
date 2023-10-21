using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rBody;

    [SerializeField]
    private SpriteRenderer sRenderer;

    float direction;
    float speed = 5;
    float jumpStrength = 20;
    public bool isGrounded = false;

    public float superJumpDelay = 0;
    public bool chargingSuperJump = false;


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
            sRenderer.flipX = false;
        }
        if(direction == -1)
        {
            sRenderer.flipX = true;
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
            superJumpDelay = 0;
            SuperJump();

        }

        if (!chargingSuperJump) superJumpDelay = 0;

        
    }

    public void SuperJump()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcao = (mousePos - (Vector2)transform.position).normalized;

        rBody.AddForce(25 * direcao, ForceMode2D.Impulse);
    }


    private void FixedUpdate()
    {
        rBody.velocity = new Vector2(direction * speed, rBody.velocity.y);
    }

}
