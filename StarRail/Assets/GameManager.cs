using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    TITLE,
    COUNT,
    PLAY,
    END,
    RESULT
}

public class GameManager : SingletonMonoBehaviour<GameManager> {

    private GameState _gameState = GameState.TITLE;
    public GameState GameState {
        get { return _gameState; }
    }

    [SerializeField]
    private Player _player = null;

    [SerializeField]
    private Timer _timer = null;

    private bool _titleInit = false;
    private bool _countInit = false;
    private bool _playInit = false;
    private bool _endInit = false;
    private bool _resultInit = false;

    private void Start()
    {
        SetTitle();
    }
	
    public void ChangeState(GameState gameState)
    {
        _gameState = gameState;
        switch (gameState)
        {
            case GameState.TITLE:
                _titleInit = false;
                SetTitle();
                break;
            case GameState.COUNT:
                _countInit = false;
                SetCount();
                break;
            case GameState.PLAY:
                _playInit = false;
                SetPlay();
                break;
            case GameState.END:
                _endInit = false;
                SetEnd();
                break;
            case GameState.RESULT:
                _resultInit = false;
                SetResult();
                break;
        }
    }

    private void SetTitle()
    {
        if (_titleInit) return;

        _player.Init();
        StageCreator.Instance.Init();
        UIManager.Instance.SetTitle();

        _titleInit = true;
    }

    private void SetCount()
    {
        if (_countInit) return;

        UIManager.Instance.SetCount();

        _countInit = true;
        ChangeState(GameState.PLAY);
    }

    private void SetPlay()
    {
        if (_playInit) return;

        _timer.Init();
        _player.PlayerStart();
        UIManager.Instance.SetPlay();

        _playInit = true;
    }
    private void SetEnd()
    {
        if (_endInit) return;

        UIManager.Instance.EndGame();

        ChangeState(GameState.RESULT);

        _endInit = true;
    }
    private void SetResult()
    {
        if (_resultInit) return;

        UIManager.Instance.SetResult();

        _resultInit = true;
    }

    public void Retry()
    {
        _player.Init();
        StageCreator.Instance.Init();

        ChangeState(GameState.COUNT);
    }
}
