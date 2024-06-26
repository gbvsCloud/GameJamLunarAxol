using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    #region StateMachine
    public enum GameStates
    {
        INTRO,
        GAMEPLAY,
        PAUSE,
        WIN,
        LOSE
    }

    public StateMachine<GameStates> stateMachine;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        stateMachine = new StateMachine<GameStates>();
        stateMachine.Init();
        stateMachine.RegisterStates(GameStates.INTRO, new StateBase());
        stateMachine.RegisterStates(GameStates.GAMEPLAY, new StateBase());
        stateMachine.RegisterStates(GameStates.PAUSE, new StateBase());
        stateMachine.RegisterStates(GameStates.WIN, new StateBase());
        stateMachine.RegisterStates(GameStates.LOSE, new StateBase());

        stateMachine.SwitchState(GameStates.INTRO);
    }

    #endregion

    //[SerializeField]
    //private StateMachine stateMachine;

    [SerializeField]
    private GameObject pauseGroup;

    [SerializeField]
    private List<Transform> checkpoints;

    [SerializeField]
    private Transform lastCheckpoint;

    [SerializeField]
    private Player player;

    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    Button musicButton;
    [SerializeField]
    Button SFXButton;

    [SerializeField]
    Sprite musicUnmutedIMG, musicMutedIMG;
    [SerializeField]
    Sprite SFXUnmutedIMG, SFXMutedIMG;

    bool gamePaused = false;

    bool musicMuted = false;
    bool sfxMuted = false;

    public Image[] eyes;
    public Sprite openEye, closedEye;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                gamePaused = true;
                Time.timeScale = 0;
                if (musicMuted)
                {
                    musicButton.image.sprite = musicMutedIMG;
                }
                else
                {
                    musicButton.image.sprite = musicUnmutedIMG;
                }

                if(sfxMuted)
                {
                    SFXButton.image.sprite = SFXMutedIMG;
                }
                else
                {
                    SFXButton.image.sprite = SFXUnmutedIMG;
                }

                pauseGroup.SetActive(true);
            }
            else
            {
                gamePaused = false;
                Time.timeScale = 1;
                pauseGroup.SetActive(false);
            }
        }
    }

    public void Unpause()
    {
        if (gamePaused)
        {
            gamePaused = false;
            Time.timeScale = 1;
            pauseGroup.SetActive(false);
        }
    }

    public void NewCheckPoint(Transform checkpoint)
    {
        if(checkpoints.IndexOf(checkpoint) > checkpoints.IndexOf(lastCheckpoint))
        {
            lastCheckpoint = checkpoint;
        }

        
    }

    public void ReturnToLastCheckpoint()
    {
        StartCoroutine(AnimationDeath());
    }

    private IEnumerator AnimationDeath()
    {
        player.stateMachine.SwitchState(Player.States.DEAD, player);
        yield return new WaitForSeconds(2);
        if(lastCheckpoint != null) player.transform.position = lastCheckpoint.position;
        player.stateMachine.SwitchState(Player.States.IDLE, player);
    }

    public void InvertMusicState()
    {
        if (!musicMuted)
        {
            musicMuted = true;
            mixer.SetFloat("music", -80);
            musicButton.image.sprite = musicMutedIMG;
        }
        else
        {
            musicMuted = false;
            mixer.SetFloat("music", 20);
            musicButton.image.sprite = musicUnmutedIMG;
        }
    }

    public void InvertSfxState() 
    {
       
        if (!sfxMuted)
        {
            Debug.Log("mutou");
            sfxMuted = true;
            mixer.SetFloat("sfx", -80);
            SFXButton.image.sprite = SFXMutedIMG;
        }
        else
        {
            Debug.Log("desmutou");
            sfxMuted = false;
            mixer.SetFloat("sfx", 20);
            SFXButton.image.sprite = SFXUnmutedIMG;
        }
    }

    public void CloseEye()
    {
        for(int i = 2; i > -1; i--)
        {
            if (eyes[i].sprite == openEye)
            {
                eyes[i].sprite = closedEye;
                return;
            }
        }
    }

}

