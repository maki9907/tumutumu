using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    //ballの生成
    //ballのプレハブの生成

    [SerializeField] GameObject ballParefab = default;
    //画像の設定
    [SerializeField] Sprite[] ballSprite = default;


    private void Start()
    {
        //StartCoroutine(Spawn(40));
    }
    public IEnumerator Spawn(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-0.2f, 0.2f), 8f);
            GameObject ball = Instantiate(ballParefab, pos, Quaternion.identity);
            //画像の設定
            int ballID = Random.Range(0, ballSprite.Length);
            ball.GetComponent<SpriteRenderer>().sprite = ballSprite[ballID] ;
            ball.GetComponent<Ball>().id = ballID;
            yield return new WaitForSeconds(0.04f);
        }
    }
}
