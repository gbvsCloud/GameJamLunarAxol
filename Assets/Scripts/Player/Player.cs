using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : EntityBase
{
    public bool climb = false;
    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    //privates
    private float _currentGravity;
    private string _tagEnemy = "Enemy";
    private Vector2 _currentTransform;

    void Start()
    {
        Init(3);
        _currentGravity = GetComponent<Rigidbody2D>().gravityScale;
    }

    protected override void Update()
    {
        base.Update();
        if (climb)
            transform.DOMove(_currentTransform, 0f);w
    }

    public float GetGravity()
    {
        return _currentGravity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(_tagEnemy))
        {
            TakeDamage();
        }
        else if (collision.transform.CompareTag("TornTiles"))
        {
            TakeDamage();
        }
        //else if (collision.transform.CompareTag("WallTiles"))
        //    ClimbWall();
    }

    private void ClimbWall()
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        _currentTransform = transform.position;
        climb = true;
    }

}
