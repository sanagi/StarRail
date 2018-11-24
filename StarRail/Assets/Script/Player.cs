using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Player : MonoBehaviour
{
    public static readonly float MAX_X = 15f;
    public static readonly float MIN_X = -15f;
    public static readonly float ONE_X = 7.5f;

    public static readonly float PLUS_DISTANCE = 100f;
    private readonly Vector3 STARTPOS = new Vector3(0f, -5.5f, -1.0f* PLUS_DISTANCE);
    private readonly Vector3 PLAYERMOV = Vector3.forward;

    private const float DELETE_DISTANCE = 220;

    private const float MAX_SPEED = 60f;
    private const float MIN_SPEED = 30.0f;
    private const float OVER_SPEED = 90f;

    private const float PAUSE_TIME = 1.5f;
    private const float OVER_TIME = 5.0f;
    private const float ACCEL_TIME = 0.6f;
    private const float SHAKE_TIME = 0.25f;


    private float _nextDistance = 100f;
    private float _playerSpeed = 0;
    private float _nextPlayerSpeed = MIN_SPEED;
    private float _accelValue = 30.0f;
    private float _deaccelValue = 40.0f;

    private float _allDistance = 0f;
    private float _segmentDistance = 0f;
    private float _deleteDistance = 0f;

    private int _charge = 0;

    private float _pauseTimer = 0f;
    private float _overTimer = 0f;
    private float _shakeTimer = 0f;

    [SerializeField]
    private ParticleSystem _thunderEffect = null;

    [SerializeField]
    private ParticleSystem _accelEffect = null;

    [SerializeField]
    private ParticleSystem _overEffect = null;

    [SerializeField]
    private ParticleSystem _smokeEffect = null;

    [SerializeField]
    private CameraShaker _cameraShaker = null;
    private CameraShakeInstance m_instance;

    public enum State
    {
        STOP,
        PLAY,
        OVER,
        PAUSE,
        ACCEL,
        WAIT,
        DEACCEL
    }

    private State _state = State.STOP;

    // Use this for initialization
    public void Init()
    {
        gameObject.transform.position = STARTPOS;

        _accelEffect.gameObject.SetActive(false);
        _thunderEffect.gameObject.SetActive(false);
        _overEffect.gameObject.SetActive(false);
        _smokeEffect.gameObject.SetActive(false);

        _state = State.WAIT;
        _playerSpeed = 0f;

        InputManager.Instance.SetPlayer(this);

        _nextDistance = 100f;
        _segmentDistance = 0f;
        _deleteDistance = 0f;

        ChangeCharge(0);

    }

    public void PlayerStart()
    {
        _state = State.ACCEL;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState == GameState.PLAY)
        {
            switch (_state)
            {
                case State.STOP:
                    break;
                case State.PAUSE:
                    _shakeTimer += Time.deltaTime;
                    if (_shakeTimer > SHAKE_TIME)
                    {
                        m_instance.StartFadeOut(0.1f);
                    }

                    _pauseTimer += Time.deltaTime;
                    if (_pauseTimer > PAUSE_TIME)
                    {
                        _state = State.ACCEL;
                        _pauseTimer = 0f;
                        _overTimer = 0f;
                        _shakeTimer = 0f;
                        ChangeCharge(0);
                    }
                    break;

                case State.ACCEL:
                case State.DEACCEL:
                case State.OVER:
                case State.PLAY:
                    if (_state == State.ACCEL)
                    {
                        _playerSpeed += _accelValue * Time.fixedDeltaTime;

                        if (_playerSpeed >= _nextPlayerSpeed)
                        {
                            _state = State.PLAY;
                            _playerSpeed = _nextPlayerSpeed;
                        }
                    }

                    if (_state == State.DEACCEL)
                    {
                        _playerSpeed -= _deaccelValue * Time.fixedDeltaTime;

                        if (_playerSpeed <= _nextPlayerSpeed)
                        {
                            _state = State.PLAY;
                            _playerSpeed = _nextPlayerSpeed;
                        }
                    }

                    if (_state == State.OVER)
                    {
                        _overTimer += Time.deltaTime;
                        if (_overTimer > OVER_TIME)
                        {
                            _state = State.DEACCEL;
                            ChangeCharge(0);
                            _nextPlayerSpeed = MIN_SPEED;
                            _overTimer = 0f;

                            _overEffect.Stop();
                            _overEffect.gameObject.SetActive(false);

                            _accelEffect.Stop();
                            _accelEffect.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        if (_playerSpeed > MAX_SPEED)
                        {
                            _playerSpeed = MAX_SPEED;
                        }
                    }

                    gameObject.transform.position += PLAYERMOV * _playerSpeed * Time.deltaTime;

                    _allDistance = gameObject.transform.position.z;

                    _segmentDistance += (PLAYERMOV * _playerSpeed * Time.deltaTime).z;
                    _deleteDistance += (PLAYERMOV * _playerSpeed * Time.deltaTime).z;
                    if (_segmentDistance > _nextDistance)
                    {
                        StageCreator.Instance.CreateNextArea();
                        _segmentDistance = 0f;
                        if (_nextDistance == 100)
                        {
                            _nextDistance = 220f;
                        }
                        else
                        {
                            _nextDistance = 110f;
                        }
                    }

                    if (_deleteDistance > DELETE_DISTANCE)
                    {
                        StageCreator.Instance.DestroyOldArea();
                        _deleteDistance = 0f;
                    }

                    break;
            }

            UIManager.Instance.SetSpeed(_playerSpeed);
            UIManager.Instance.SetDistance(_allDistance);
        }
    }

    public void ChangeCharge(int charge)
    {
        _charge = charge;

        if (_charge == 10)
        {
            _playerSpeed = OVER_SPEED;
            _state = State.OVER;
            SetOverEffect();
        }
        else if (_charge > 10)
        {
            _state = State.PAUSE;

            _overEffect.Stop();
            _overEffect.gameObject.SetActive(false);

            _accelEffect.Stop();
            _accelEffect.gameObject.SetActive(false);

            SetSmokeEffect();

            m_instance = _cameraShaker.StartShake(2, 2, 0.1f);
            m_instance.DeleteOnInactive = false;

            _playerSpeed = 0;
            _nextPlayerSpeed = MIN_SPEED;
            _charge = 0;
        }
        else
        {
            if (charge != 0)
            {
                SetAccelEffect();
            }
        }
        UIManager.Instance.SetBattery(_charge);
    }

    public void SetNextSpeed(int score)
    {
        float multiSpeed = score * 0.1f + 1.0f;
        _nextPlayerSpeed *= multiSpeed;
        if (_nextPlayerSpeed > MAX_SPEED)
        {
            _nextPlayerSpeed = MAX_SPEED;
        }
        _state = State.ACCEL;
        ChangeCharge(_charge + score);
    }

    public void SetSpeed_ChangeRail()
    {
        if (_state != State.OVER)
        {
            _playerSpeed = _playerSpeed - _playerSpeed / 10.0f;
            _state = State.ACCEL;
        }
    }

    public float GetSpeed()
    {
        return _playerSpeed;
    }

    public void Stop()
    {
        _state = State.STOP;
        _overEffect.Stop();
        _overEffect.gameObject.SetActive(false);
        _accelEffect.loop = false;
        if (m_instance != null)
        {
            m_instance.StartFadeOut(0.25f);
        }
    }

    public float GetDistance()
    {
        return _allDistance;
    }

    public State GetState()
    {
        return _state;
    }

    public int GetCharge()
    {
        return _charge;
    }

    public void SetThunder()
    {
        _thunderEffect.gameObject.SetActive(true);
        _thunderEffect.emissionRate = _charge * 100;
        _thunderEffect.Play();
    }

    public void SetAccelEffect()
    {
        _accelEffect.gameObject.SetActive(true);
        _accelEffect.loop = false;
        _accelEffect.Play();
    }

    public void SetOverEffect()
    {
        _accelEffect.gameObject.SetActive(true);
        _accelEffect.loop = true;
        _accelEffect.Play();

        _overEffect.gameObject.SetActive(true);
        _overEffect.Play();
    }

    public void SetSmokeEffect()
    {
        _smokeEffect.gameObject.SetActive(true);
        _smokeEffect.Play();
    }
}
