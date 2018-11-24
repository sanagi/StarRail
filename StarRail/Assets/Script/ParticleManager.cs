using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//パーティクル辞書
public enum ParticleEnum
{
    WARP
}

public class ParticleManager : SingletonMonoBehaviour<ParticleManager> {
    [SerializeField]
    private List<GameObject> particleList = new List<GameObject>();
    public Dictionary<ParticleEnum, GameObject> ParticleDictionary = new Dictionary<ParticleEnum, GameObject>();

    /// <summary>
    /// リストから読む形だけど本来はアセバンから読むようにしたい
    /// </summary>
    void Start()
    {
        for(int i=0;i< particleList.Count; i++)
        {
            ParticleDictionary.Add((ParticleEnum)i, particleList[i]);
        }
    }

    // Update is called once per frame
    public void PlayOneShot (ParticleEnum particleEnum,Vector3 pos) {
        GameObject particle = GameObject.Instantiate(ParticleDictionary[particleEnum]) as GameObject;
        particle.transform.position = pos;
	}
}
