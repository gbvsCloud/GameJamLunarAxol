using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : EntityBase
{
    private string tagEnemy = "Enemy";
    
    [SerializeField]
    private SpriteRenderer sRenderer;


    private float currentGravity;
    void Start()
    {
        Init(3);
        currentGravity = GetComponent<Rigidbody2D>().gravityScale;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }


    public float GetGravity()
    {
        return currentGravity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(tagEnemy))
        {
            TakeDamage();
        }else if (collision.transform.CompareTag("TornTiles"))
        {
            TakeDamage();
        }
    }

    

}
