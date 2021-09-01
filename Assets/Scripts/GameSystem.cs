using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [SerializeField] BallGenerator ballGenerator;
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
        Debug.Log("start drag");
        //マウスによるオブジェクトの判定
        //Raycastを使う
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit   = Physics2D.Raycast(mousePosition, Vector2.zero);
        //ヒットしたオブジェクトがあり、それがBallスクリプトを持っているかの判定
        if (hit && hit.collider.GetComponent<Ball>())
        {
            Debug.Log("hit obect");
        }
    }

    void OnDragging()
    {
        Debug.Log("draggin now");

    }

    void OnDragEnd()
    {
        Debug.Log("end drag");
    }
}
