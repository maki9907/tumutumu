using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//これはボールプレハブにアタッチしてあるスクリプト

public class Ball : MonoBehaviour
{
    public int id;

    [SerializeField] GameObject explosionPrefab = default;

    public void Explosion()
    {
        GameObject explosion =  Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(explosion, 0.2f);
    }

    public bool IsBomb()
    {
        if(id == -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
