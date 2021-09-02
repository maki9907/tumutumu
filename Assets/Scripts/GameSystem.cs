using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    [SerializeField] BallGenerator ballGenerator = default;
    [SerializeField] List<Ball> removeBalls = default;
    Ball currentBall;

    bool isDragging;

    int score;
    [SerializeField] Text scoreText = default;
    [SerializeField] GameObject pointEfectPrefab;

    Vector3 ballPosition;

    void Start()
    {
        StartCoroutine(ballGenerator.Spawn(ParamsSO.Entity.initBallCount));
        score = 0;
    }

    void AddScore(int point)
    {
        score += point;
        scoreText.text = score.ToString();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //もしもボムだったら周囲のツムを消しつつ爆発する。
            OnDragBegin();
        }else if (Input.GetMouseButtonUp(0))
        {
            OnDragEnd(); 
        }else if (isDragging)
        {
            OnDragging(); 
        }
    }

    void OnDragBegin()
    {
        //Debug.Log("start drag");
        //マウスによるオブジェクトの判定
        //Raycastを使う
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit2D = Physics2D.Raycast(mousePosition, Vector2.zero);

        //ヒットしたオブジェクトがあり、それがBallスクリプトを持っているかの判定
        if (hit2D && hit2D.collider.GetComponent<Ball>())
        {
            //Debug.Log("hit obect");
            //Ballクラスでオブジェクトを持ってくる
            Ball ball = hit2D.collider.GetComponent<Ball>();

            //もしもボムなら周囲を含めて爆発する
            if(ball.IsBomb())
            {
                Explosion(ball);
            }
            else
            {
                AddRemoveBall(ball);
                isDragging = true;
            }           
        }
    }

    void OnDragging()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit2D = Physics2D.Raycast(mousePosition, Vector2.zero);

        //ヒットしたオブジェクトがあり、それがBallスクリプトを持っているかの判定
        if (hit2D && hit2D.collider.GetComponent<Ball>())
        {
            //Debug.Log("hit obect");
            //Ballクラスでオブジェクトを持ってくる
            Ball ball = hit2D.collider.GetComponent<Ball>();

            //ballオブジェクト間の距離が1.5f以下のときにリストにballを追加していく
            float distance = Vector2.Distance(ball.transform.position, currentBall.transform.position);
            if(distance < ParamsSO.Entity.distance)
            {
                //ballオブジェクトのid(種類)が同じ時かつオブジェクトのタグがBallタグのときのみリストに追加する
                //IsBombを使ったらボムの判定しかできないためアイテムが増えた際に不便
                if(ball.id == currentBall.id && ball.CompareTag("Ball"))
                {
                    AddRemoveBall(ball);
                }
            }
        }
    }

    void OnDragEnd()
    {
        //もしballが3つ以上つながっていたらballを消す
        if(removeBalls.Count >= 3)
        {
            for(int i = 0; i < removeBalls.Count; i++)
            {
                Destroy(removeBalls[i].gameObject);
                removeBalls[i].Explosion();
            }
            //消した分だけツムを追加する
            StartCoroutine(ballGenerator.Spawn(removeBalls.Count));

            //消したツム×100ポイントを追加する
            int point = removeBalls.Count * ParamsSO.Entity.point;
            AddScore(point);
            SpawnPointEfect(removeBalls[removeBalls.Count -1].transform.position, point);
        }
        else
        {
            for(int i = 0; i < removeBalls.Count; i++)
            {
                Ball ball = removeBalls[i];
                //色をもとに戻す
                RefreshColor(ball);
            }

        }       
        //リストの中身を空にする
        removeBalls.Clear();
        isDragging = false;
    }

    void AddRemoveBall(Ball ball)
    {
        currentBall = ball;
        if (!removeBalls.Contains(ball))
        {
            removeBalls.Add(ball);
            //オブジェクトがBallタグを持っていたときのみballの色を薄くする
            if (ball.CompareTag("Ball"))
            {
                Color ballColor = ball.GetComponent<SpriteRenderer>().color;
                ballColor.a = 0.5f;
                ball.GetComponent<SpriteRenderer>().color = ballColor;
            }
            ball.transform.localScale = Vector3.one * 1.4f;
        }
    }

    void RefreshColor(Ball ball)
    {
        //Ballタグを持っていた場合のみballの色を濃くする
        if (ball.CompareTag("Ball"))
        {
            Color ballColor = ball.GetComponent<SpriteRenderer>().color;
            ballColor.a = 1.0f;
            ball.GetComponent<SpriteRenderer>().color = ballColor;
        }
        ball.transform.localScale = Vector3.one;
    }

    void Explosion(Ball bomb)
    {
        List<Ball> explosionList = new List<Ball>();
        //ボムの周囲にあるボールの情報を集める
        Collider2D[] colliders = Physics2D.OverlapCircleAll(bomb.transform.position, 2);

        for(int i = 0; i < colliders.Length; i++)
        {
            //ボールだったら爆破するリストに追加する
            Ball ball = colliders[i].GetComponent<Ball>();
            if (ball)
            {
                explosionList.Add(ball);
            }
        }

        
        //爆破する
        for (int i = 0; i < explosionList.Count; i++)
        {
            Destroy(explosionList[i].gameObject);
            //このExplosion関数はエフェクト発生の関数
            explosionList[i].Explosion();
        }
        StartCoroutine(ballGenerator.Spawn(explosionList.Count));
        int score = explosionList.Count * ParamsSO.Entity.point;
        AddScore(score);
        SpawnPointEfect(bomb.transform.position, score);
        explosionList.Clear();
    }

    void SpawnPointEfect(Vector2 position, int score)
    {
        Instantiate(pointEfectPrefab, position,Quaternion.identity);
    }
}
