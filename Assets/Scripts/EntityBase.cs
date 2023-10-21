using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    protected int maxHealth;
    [SerializeField]
    protected int health;
    public bool isDead;

    public void Init(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
        isDead = false;
    }


    public void TakeDamage()
    {
        health--;
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
