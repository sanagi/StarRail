using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField]
    private Image _chargeSlider = null;

    [SerializeField]
    private Text _speedText = null;

    [SerializeField]
    private Text _time = null;

    [SerializeField]
    private Text _distance = null;

    [SerializeField]
    private GameObject _title = null;

    [SerializeField]
    private GameObject _speedMator = null;

    [SerializeField]
    private GameObject _result = null;

    public void SetTime(float time)
    {
        if (time < 0)
        {
            time = 0;
        }
            _time.text = string.Format("{0:00.0}", time);
    }

    public void SetDistance(float distance)
    {
        distance += Player.PLUS_DISTANCE;
        _distance.text = string.Format("{0:0000}",distance);
    }

    public void SetSpeed(float speed)
    {
        _speedText.text = string.Format("{0:00.0}", speed);
    }

	// Update is called once per frame
	public void SetBattery (int value) {
        _chargeSlider.fillAmount = value * 0.1f;
	}


    public void SetTitle()
    {
        _distance.gameObject.SetActive(false);
        _time.gameObject.SetActive(false);
        _result.SetActive(false);

        //タイトル
        _title.gameObject.SetActive(true);
    }

    public void SetPlay()
    {
        _distance.gameObject.SetActive(true);
        _time.gameObject.SetActive(true);


    }

    public void SetCount()
    {
        //タイトル
        _title.gameObject.SetActive(false);
        _result.SetActive(false);

        Debug.Log("2");

        _speedMator.gameObject.SetActive(true);
    }

    public void EndGame()
    {
        _distance.gameObject.SetActive(false);
        _time.gameObject.SetActive(false);
        _speedMator.SetActive(false);
    }

    public void SetResult()
    {
        _result.SetActive(true);
    }
}
