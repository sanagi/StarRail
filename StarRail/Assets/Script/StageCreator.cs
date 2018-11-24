using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreator : SingletonMonoBehaviour<StageCreator> {
    //生成するプレハブ
    [SerializeField]
    private GameObject _railParent = null;

    //次に生成するレールの中心
    private const float CREATE_RAIL_ROOT_Z = 220f;

    //生成したコースの個数
    private int _railParentNum = 0;

    //存在しているコースのリスト
    public List<RailManager> _railManagers = new List<RailManager>();

    public void Init()
    {
        for(int i = 0; i < _railManagers.Count; i++)
        {
            GameObject.Destroy(_railManagers[i].gameObject);
        }
        _railManagers.Clear();
        _railParentNum = 0;
        CreateNextArea();
        CreateNextArea();
    }

	// Use this for initialization
	public void CreateNextArea () {
        Debug.Log("crea");
        for (int i = 0; i < 1; i++)
        {
            RailManager railManager = GameObject.Instantiate(_railParent).GetComponent<RailManager>();
            railManager.gameObject.transform.position = new Vector3(0f, 0f, _railParentNum * CREATE_RAIL_ROOT_Z);

            railManager.Init();

            _railManagers.Add(railManager);

            _railParentNum++;
        }
    }

    public void DestroyOldArea()
    {
        //あきらかに通り過ぎたレールを破棄
        RailManager rail = _railManagers[0];
        _railManagers.Remove(rail);
        GameObject.Destroy(rail.gameObject);
    }
}
