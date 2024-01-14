using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WaterWave : MonoBehaviour
{
    public bool goingLeft;
    float velocity = 2;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    public bool collided = false;

    private void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        if(!goingLeft){
            spriteRenderer.flipX = true;
            boxCollider.offset = new Vector2(2, 0);
        }
        Destroy(gameObject, 6);

    }
    private void Update() {
        if(goingLeft)
            rigidBody.velocity = Vector2.left * velocity;
        else
            rigidBody.velocity = Vector2.right * velocity;
        if(velocity < 300)velocity += velocity * 2 * Time.deltaTime;
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(collided == true) return;

        if(other.CompareTag("Player")){
            other.GetComponent<EntityBase>()?.TakeDamage();
            collided = true;
        }

            
        
        
    }

}
