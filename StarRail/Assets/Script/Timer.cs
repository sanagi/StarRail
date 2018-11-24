using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    public const float GAMETIME = 60f;

    [SerializeField]
    private Player _player;

    private float _timer = 0f;
	// Use this for initialization
	public void Init () {
        _timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.GameState == GameState.PLAY)
        {
            _timer += Time.deltaTime;
            if (_timer > GAMETIME)
            {
                _player.Stop();
                GameManager.Instance.ChangeState(GameState.END);
                //Debug.Log(_player.GetDistance());
            }
            UIManager.Instance.SetTime(GAMETIME - _timer);
        }
	}
}
