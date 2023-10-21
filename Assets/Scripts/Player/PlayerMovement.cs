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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            Debug.Log("apertou");
            rBody.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
        }
        if(direction == 1)
        {
            sRenderer.flipX = false;
        }
        if(direction == -1)
        {
            sRenderer.flipX = true;
        }


    }

    private void FixedUpdate()
    {
        rBody.velocity = new Vector2(direction * speed, rBody.velocity.y);
    }

}
