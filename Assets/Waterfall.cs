using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    public float timeBetweenSprites;
    public int currentSprite = 0;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(StartWaterfall());
    }

    IEnumerator StartWaterfall(){
        spriteRenderer.sprite = sprites[currentSprite];
        yield return new WaitForSeconds(timeBetweenSprites);
        currentSprite++;
        if(currentSprite >= sprites.Length)
            currentSprite = 0;
        StartCoroutine(StartWaterfall());

    }


}
