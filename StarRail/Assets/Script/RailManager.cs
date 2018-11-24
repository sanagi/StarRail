using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailManager : MonoBehaviour {

    [SerializeField]
    private GameObject itemPrefab = null;

    private const float ITEM_POS_Z = -0.15f;
    private const float ITEM_POS_Z2 = 0.4f;

    [SerializeField]
    private List<GameObject> _railList = new List<GameObject>();

	public void Init () {
        CreateItem();
	}

    /// <summary>
    /// ランダムな点数のアイテムを作成(1つのレールだけ穴をあける)
    /// </summary>
    void CreateItem()
    {
        int random1 = (int)Random.Range(0, 4);
        int random2 = (int)Random.Range(0, 4);
        for (int i= 0;i < _railList.Count; i++)
        {
            if (i != random1)
            {
                //1つめ
                Item item = GameObject.Instantiate(itemPrefab).GetComponent<Item>();
                item.transform.SetParent(_railList[i].transform);
                item.gameObject.transform.localPosition = new Vector3(0f, 0f, ITEM_POS_Z);
                item.Initialize();
            }

            if (i != random2)
            {
                //2つめ
                Item item2 = GameObject.Instantiate(itemPrefab).GetComponent<Item>();
                item2.transform.SetParent(_railList[i].transform);
                item2.gameObject.transform.localPosition = new Vector3(0f, 0f, ITEM_POS_Z2);
                item2.Initialize();
            }
        }
    }
}
