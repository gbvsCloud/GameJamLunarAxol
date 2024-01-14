
using UnityEngine;

public class BigFrogFoot : MonoBehaviour
{
    public BigFrog bigFrog;
    
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Map")){
            bigFrog.isGrounded = true;
        }
    }
}
