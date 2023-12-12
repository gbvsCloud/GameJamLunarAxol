using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    IEnumerator stunI;

    public override void Knockback(Transform knockbackOrigin, float strength)
    { 
        if(stunI != null) StopCoroutine(stunI);
        stunI = StunDuration();
        StartCoroutine(stunI);

    }
    public override void Patrol()
    {
        if(stunned){
            rigidBody.velocity = Vector2.zero;
            return; 
        } 
        
        if(goingRight)
        {
            rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector2(-speed, rigidBody.velocity.y);
        }
    }
    IEnumerator StunDuration(){
        stunned = true;
        yield return new WaitForSeconds(1f);
        stunned = false;
    }

}
