using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
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
    public static int playerHealth = 3;
    public void Start()
    {
        Init();
        


    }


    int GetEyeName(string nomeDoObjeto)
    {
        string stringNumber = new string(nomeDoObjeto.Where(char.IsDigit).ToArray());

        if (int.TryParse(stringNumber, out int number))
        {
            return number;
        }
        return 0;
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

    public List<Image> eyes;
    public Sprite openEye, closedEye;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        player = FindObjectOfType<Player>()?.GetComponent<Player>();
        eyes.Clear();
        checkpoints.Clear();
        player.gameManager = this;

        Checkpoint[] checkpointsScript = FindObjectsOfType<Checkpoint>();
        foreach(Checkpoint c in checkpointsScript){
            checkpoints.Add(c?.GetComponent<Transform>());
        }
        
        // Obter todos os objetos com uma determinada tag
        GameObject[] eye = GameObject.FindGameObjectsWithTag("Eye");

        // Ordenar os objetos com base nos números nos nomes
        GameObject[] eyesInOrder = eye?.OrderBy(objeto => GetEyeName(objeto.name)).ToArray();
        foreach(GameObject e in eyesInOrder){
            eyes.Add(e?.GetComponent<Image>());
        }
        if(eyes.Count > 0) CheckEyes();
    }
    public override void Awake()
    {

        
        base.Awake();
        
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
        lastCheckpoint = checkpoint;

        
    }

    public void ReturnToLastCheckpoint()
    {
        StartCoroutine(AnimationDeath());
    }

    private IEnumerator AnimationDeath()
    {
        player.stateMachine.SwitchState(Player.States.DEAD, player);
        yield return new WaitForSeconds(0.5f);
        if(player.health > 0){
        if(lastCheckpoint != null) 
            player.transform.position = lastCheckpoint.position;
            player.stateMachine.SwitchState(Player.States.IDLE, player);
            player.GetComponent<SpriteRenderer>().flipX = lastCheckpoint.GetComponent<Checkpoint>().lookRight;
        }
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
        playerHealth = player.health;
        eyes[player.health].sprite = closedEye;
        
    }

    public void CheckEyes(){
        if(playerHealth == 1){
            eyes[2].sprite = closedEye;
            eyes[1].sprite = closedEye;
        }else if(playerHealth == 2){
            eyes[2].sprite = closedEye;   
        }
    }
    public static int RetrievePlayerHealth(){
        return playerHealth;
    }

}

