using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

    //縦
    //private Vector3 _offset = new Vector3(0f, 65.7f, -12.4f);

    //縦
    private Vector3 _offset = new Vector3(0f, 43.229267f, -40.35427f);

    //横
    //private Vector3 _offset = new Vector3(0f, 11.799998f, -7.3f);

    [SerializeField]
    private Player _player = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 pos = _player.gameObject.transform.position + _offset;
        pos.x = 0f;
        gameObject.transform.position = pos;
	}
}
