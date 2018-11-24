using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDestroy : MonoBehaviour {
    private float _particleTimer = 0f;
    private float _particleTime = 0f;
	// Use this for initialization
	void Start () {
        _particleTime = gameObject.GetComponent<ParticleSystem>().main.startLifetimeMultiplier + 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        _particleTimer += Time.deltaTime;
        if (_particleTimer > _particleTime)
        {
            GameObject.Destroy(this.gameObject);
        }
	}
}
