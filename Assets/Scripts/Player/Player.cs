using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
public class Player : EntityBase
{
    public bool climb = false;
    
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    public PlayerMovement playerMovement;

    //privates
    private float _currentGravity;
    private string _tagEnemy = "Enemy";
    private Vector2 _currentTransform;

    void Start()
    {
        Init(3);
        _currentGravity = GetComponent<Rigidbody2D>().gravityScale;
        playerMovement = GetComponent<PlayerMovement>();
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
            rigidBody.gravityScale = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Checkpoint"))
        {
            gameManager.NewCheckPoint(collision.transform);
        }
    }

    public override void Death()
    {
        playerMovement.stateMachine.SwitchState(StateMachine.States.DEAD, this);
        StartCoroutine(DeathAnimation());
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        gameManager.CloseEye();
    }

    IEnumerator DeathAnimation()
    {
        
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

}
