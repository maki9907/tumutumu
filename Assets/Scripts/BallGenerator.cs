using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    //ballの生成
    //ballのプレハブの生成

    [SerializeField] GameObject[] ballParefab = default;
    [SerializeField] GameObject bombPrefab = default;
    //画像の設定
    //[SerializeField] Sprite[] ballSprite = default;
    

    private void Start()
    {
        //StartCoroutine(Spawn(40));
    }
    public IEnumerator Spawn(int count)
    {
        for(int i = 0; i < count; i++)
        {
            //ボールの降ってくる位置を指定
            Vector2 pos = new Vector2(Random.Range(-0.2f, 0.2f), 8f);
            //ボールの種類を決定
            int ballID = Random.Range(0, ballParefab.Length); // 0～4までは通常のボールのプレハブ、-1はボムのプレハブ

            //もしボムならボールIDが-1に、それ以外なら普通のボールを生成する
            if (Random.Range(0, 100) < 10) //3%の確率でtrueが返る
            {
                ballID = -1;
                GameObject bomb = Instantiate(bombPrefab, pos, Quaternion.identity);
                bomb.GetComponent<Ball>().id = ballID;
            }
            else
            {
                GameObject ball = Instantiate(ballParefab[ballID], pos, Quaternion.identity);
                //ball.GetComponent<SpriteRenderer>().sprite = ballSprite[ballID] ;
                ball.GetComponent<Ball>().id = ballID;
            }

            yield return new WaitForSeconds(0.04f);
        }
    }
}
