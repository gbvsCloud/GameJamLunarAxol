using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityVariation : MonoBehaviour
{

    public float minOpacity, maxOpacity;
    SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate() {
        float rand = Random.Range(0f, 1f);

        if(rand == 0 && spriteRenderer.color.a < maxOpacity){
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + Time.deltaTime);
        }else if (rand == 0 && spriteRenderer.color.a > minOpacity){
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - Time.deltaTime);
        }


    }
}
