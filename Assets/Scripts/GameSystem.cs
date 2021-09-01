using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [SerializeField] BallGenerator ballGenerator;
    [SerializeField] List<Ball> removeBalls;
    Ball currentBall;

    bool isDragging;

    void Start()
    {
        StartCoroutine(ballGenerator.Spawn(40));
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
            AddRemoveBall(ball);
            isDragging = true;
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
            if(distance < 1.5f)
            {
                //ballオブジェクトのid(種類)が同じ時のみリストに追加する
                if(ball.id == currentBall.id)
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
            }
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
            //ballの色を薄くする
            Color ballColor = ball.GetComponent<SpriteRenderer>().color;
            ballColor.a = 0.5f;
            ball.GetComponent<SpriteRenderer>().color = ballColor;
        }
    }

    void RefreshColor(Ball ball)
    {
        //ballの色を濃くする
        Color ballColor = ball.GetComponent<SpriteRenderer>().color;
        ballColor.a = 1.0f;
        ball.GetComponent<SpriteRenderer>().color = ballColor;
    }
}
