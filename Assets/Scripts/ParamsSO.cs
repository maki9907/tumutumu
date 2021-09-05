using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ParamsSO : ScriptableObject
{
    [Header("初期のボール落下数")]
    public int initBallCount;

    [Header("ボールを消したときの得点")]
    public int point;

    [Header("ボール間の判定距離")]
    public float distance;
    [Header("時間制限")]
    public int timer;

    //ParamsSOが保存してある場所のパス
    public const string PATH = "ParamsSO";

    //ParamsSOの実体
    private static ParamsSO _entity;
    public static ParamsSO Entity
    {
        get
        {
            //初アクセス時にロードする
            if (_entity == null)
            {
                _entity = Resources.Load<ParamsSO>(PATH);

                //ロード出来なかった場合はエラーログを表示
                if (_entity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }

            return _entity;
        }
    }
}
