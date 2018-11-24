using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField]
    private int _itemScore = 0;

    [SerializeField]
    private GameObject childObject = null;

    private List<rotateOption> _rotateChildList = new List<rotateOption>();

    // Use this for initialization
    public void Initialize()
    {
        _itemScore = (int)Random.Range(1f, 5f);
        for (int i = 0; i < _itemScore; i++)
        {
            rotateOption rotateOption = GameObject.Instantiate(childObject).GetComponent<rotateOption>();
            rotateOption.transform.parent = this.gameObject.transform;
            rotateOption.transform.localPosition = Vector3.right * 0.5f;
            rotateOption.Init(i);

            _rotateChildList.Add(rotateOption);
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Player player = collider.gameObject.GetComponent<Player>();

            if (player.GetCharge() + _itemScore <= 10)
            {
                for (int i = 0; i < _rotateChildList.Count; i++)
                {
                    _rotateChildList[i].SetMultiSpeed(2.0f);
                }
            }
            player.SetNextSpeed(_itemScore);
            player.SetThunder();
        }
    }
}
