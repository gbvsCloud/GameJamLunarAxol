using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    [Header("Player")]
    public GameObject playerPrefab;

    [Header("Enemies")]
    public List<GameObject> enemies;

    [Header("References")]
    public Transform startPoint;

    [Header("Animation Details")]

    public float duration = 0.5f;

    public float delay = 0.5f;

    public Ease ease = Ease.OutBack;

    private GameObject _currentPlayer;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        _currentPlayer = Instantiate(playerPrefab);
        _currentPlayer.transform.position = startPoint.transform.position;
        _currentPlayer.transform.DOScale(0, duration).SetEase(ease).From().SetDelay(delay);
    }
}