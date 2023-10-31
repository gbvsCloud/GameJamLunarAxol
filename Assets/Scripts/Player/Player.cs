
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : EntityBase
{   
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    public PlayerMovement playerMovement;

    //privates
    [SerializeField] private float _currentGravity;
    private string _tagEnemy = "Enemy";

    void Start()
    {
        Init(3);
        _currentGravity = GetComponent<Rigidbody2D>().gravityScale;
    }

    protected override void Update()
    {
        base.Update();
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
            playerMovement.canRun = false;
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
