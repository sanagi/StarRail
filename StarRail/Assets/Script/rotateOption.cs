using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateOption : MonoBehaviour
{
    //angleのスピードで、とあるオブジェクトの周りを回転するスクリプト

    //変数
    //一秒当たりの回転角度
    private float _angle = 300f;

    private float _angleInit = 90f;

    //回転の中心をとるために使う変数
    private Vector3 targetPos;


    // Use this for initialization
    public void Init(int num)
    {
        //targetに、"Sample"の名前のオブジェクトのコンポーネントを見つけてアクセスする
        Transform target = gameObject.transform.parent.transform;
        //変数targetPosにSampleの位置情報を取得
        targetPos = target.position;

        //gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        //自分をX軸を中心に0～360でランダムに回転させる
        transform.Rotate(new Vector3(Random.Range(0, 360), 0, 0), Space.World);
        //自分をY軸を中心に0～360でランダムに回転させる
        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0), Space.World);
        //自分をZ軸を中心に0～360でランダムに回転させる
        transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)), Space.World);

        transform.RotateAround(targetPos, Vector3.back, num*_angleInit);
    }

    void Update()
    {
        //	Sampleを中心に自分をz方向に、毎秒angle分だけ回転する。
        transform.RotateAround(targetPos, Vector3.back, _angle * Time.deltaTime);
    }

    public void SetMultiSpeed(float multi)
    {
        _angle *= multi;
    }
}
