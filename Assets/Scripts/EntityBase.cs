using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    protected int maxHealth;
    [SerializeField]
    protected int health;
    public bool isDead;

    protected SpriteRenderer spriteRenderer;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material flashMaterial;

    Coroutine flashCoroutine = null;

    public void Init(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
        isDead = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        flashMaterial = Resources.Load<Material>("Material/FlashMaterial");
    }


    public virtual void TakeDamage()
    {
        health--;
        if(flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(Flash());
    }

    public IEnumerator Flash()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defaultMaterial;
    }

    protected virtual void Update()
    {
        if (health <= 0)
        {
            isDead = true;
            Death();
        }
    }

    public virtual void Death()
    {
        Debug.Log("morreu");
        Destroy(this.gameObject);
    }
}
