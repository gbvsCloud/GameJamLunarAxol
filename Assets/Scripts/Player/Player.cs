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
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private GameManager gameManager;

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
            transform.DOMove(_currentTransform, 0f);
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
            gameManager.ReturnToLastCheckpoint();
        }

        /*else if (collision.transform.CompareTag("WallTiles"))
        {
            ClimbWall();*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Checkpoint"))
        {
            gameManager.NewCheckPoint(collision.transform);
        }
    }


    private void ClimbWall()
    {
        rigidBody.gravityScale = 0;
        _currentTransform = transform.position;
        climb = true;
    }

}
