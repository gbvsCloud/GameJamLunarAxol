using System;
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
    protected Rigidbody2D rigidBody;
    public bool stunned;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material flashMaterial;

    Coroutine flashCoroutine = null;

    public float knockbackTimer = 0;
    protected float knockbackStunDuration = 0.2f;
    public bool knockbackWorking = false;
    public void Init(int maxHealth)
    {
        stunned = false;
        this.maxHealth = maxHealth;
        health = maxHealth;
        isDead = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
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

    public virtual void Knockback(Transform knockbackOrigin, float strength)
    {
        Vector2 direction = -(knockbackOrigin.transform.position - transform.position).normalized;
        Vector2 knockbackDirection = new Vector2(Mathf.Clamp(direction.x, -1, 1), Mathf.Clamp(direction.y, -1, 1));
        rigidBody.AddForce(new Vector2(knockbackDirection.x, knockbackDirection.y * 1.5f) * strength, ForceMode2D.Impulse);
        knockbackTimer = knockbackStunDuration;
        knockbackWorking = true;
    }
    public IEnumerator Flash()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defaultMaterial;
    }

    public virtual void KnockbackEnd()
    {

    }

    protected virtual void Update()
    {
        if (health <= 0)
        {
            isDead = true;
            Death();
        }
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;        
        }
        if(knockbackTimer <= 0 && knockbackWorking)
        {
            knockbackWorking = false;
            KnockbackEnd();
        }


    }

    public virtual void Death()
    {
        Debug.Log("morreu");
        Destroy(this.gameObject);
    }
}
