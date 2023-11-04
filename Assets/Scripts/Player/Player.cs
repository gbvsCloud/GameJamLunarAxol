
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : EntityBase
{   
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    public PlayerMovement playerMovement;

    //privates
    [SerializeField] private float _currentGravity;
    private string _tagEnemy = "Enemy";

    [SerializeField] private RandomSound damageSound;


    #region StateMachine
    public enum States
    {
        IDLE,
        RUNNING,
        DEAD,
        SWING,
        JUMPING
    }
    public StateMachine<States> stateMachine;

    public void Awake()
    {
        stateMachine = new StateMachine<States>();
        stateMachine.Init();
        stateMachine.RegisterStates(States.IDLE, new StateIdle());
        stateMachine.RegisterStates(States.RUNNING, new StateRun());
        stateMachine.RegisterStates(States.DEAD, new StateDead());
        stateMachine.RegisterStates(States.SWING, new StateSwing());
        stateMachine.RegisterStates(States.JUMPING, new StateJump());

        stateMachine.SwitchState(States.IDLE, manager, this);
    }

    public TongueManager manager;
    #endregion

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
            damageSound.PlayRandomSoundWithVariation();
            

        }
        else if (collision.transform.CompareTag("TornTiles"))
        {
            TakeDamage();
            gameManager.ReturnToLastCheckpoint();
            rigidBody.gravityScale = 1;
            playerMovement.canRun = false;
            damageSound.PlayRandomSoundWithVariation();
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
        stateMachine.SwitchState(Player.States.DEAD, this);
        StartCoroutine(DeathAnimation());
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        gameManager.CloseEye();
        if (health <= 0)
        {
            rigidBody.gravityScale = 1;
            playerMovement.canRun = false;
        }

    }

    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

}
