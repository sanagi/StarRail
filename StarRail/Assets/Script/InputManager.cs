using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    public enum InputSide
    {
        RIGHT,
        LEFT,
        NONE
    }

    private const float WAITTIME = 0.1f;
    private float _marginTimer = 0f;
    private const float OFFSET_POS_PARTICLE = 0.15f;

    private readonly Vector3 moveRight = new Vector3(Player.ONE_X, 0f, 0);
    private readonly Vector3 moveLeft = new Vector3(-1.0f * Player.ONE_X, 0f, 0f);

    private bool _enable = true;
    private Player _player = null;

    public InputSide inputSide = InputSide.NONE;

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.GameState)
        {
            case GameState.TITLE:
                if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    //カウントダウンスタート
                    GameManager.Instance.ChangeState(GameState.COUNT);
                }
                break;
            case GameState.PLAY:
            case GameState.COUNT:
                if (_enable)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (_player.transform.position.x < Player.MAX_X && _player.GetState() != Player.State.PAUSE && _player.GetState() != Player.State.STOP)
                        {
                            inputSide = InputSide.RIGHT;
                            _player.transform.position += moveRight;

                            if (_player.GetState() != Player.State.OVER)
                            {
                                _player.SetSpeed_ChangeRail();

                                Vector3 particlePos = _player.transform.position + (Vector3.forward * _player.GetSpeed() * OFFSET_POS_PARTICLE) - (Vector3.down * _player.transform.localScale.y / 2f);
                                ParticleManager.Instance.PlayOneShot(ParticleEnum.WARP, particlePos);
                            }
                            _enable = false;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (_player.transform.position.x > Player.MIN_X && _player.GetState() != Player.State.PAUSE && _player.GetState() != Player.State.STOP)
                        {
                            inputSide = InputSide.LEFT;
                            _player.transform.position += moveLeft;

                            if (_player.GetState() != Player.State.OVER)
                            {
                                _player.SetSpeed_ChangeRail();

                                Vector3 particlePos = _player.transform.position + (Vector3.forward * _player.GetSpeed() * OFFSET_POS_PARTICLE) - (Vector3.down * _player.transform.localScale.y / 2f);
                                ParticleManager.Instance.PlayOneShot(ParticleEnum.WARP, particlePos);
                            }
                            _enable = false;
                        }
                    }
                }
                else
                {
                    _marginTimer += Time.deltaTime;
                    if (_marginTimer > WAITTIME)
                    {
                        _enable = true;
                        _marginTimer = 0f;
                    }
                }
                break;
        }
    }
}
