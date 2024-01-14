using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpEcho : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rBody; 

    public void Init(Vector2 velocity)
    {
        rBody.AddForce(velocity, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
       Destroy(gameObject);
    }


}
