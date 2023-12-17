using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityVariation : MonoBehaviour
{

    public float minOpacity, maxOpacity;
    public float opacityTarget;
    SpriteRenderer spriteRenderer;
    public float opacity;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
         StartCoroutine(ChangeOpacityTarget());
    }

    private void Update() {
        opacity = spriteRenderer.color.a;

        if(spriteRenderer.color.a < opacityTarget)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + Time.deltaTime / 5);
        else
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - Time.deltaTime / 5);
        
    }

    IEnumerator ChangeOpacityTarget(){
        opacityTarget = Random.Range(minOpacity, maxOpacity) / 255;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        StartCoroutine(ChangeOpacityTarget());
    }
}
