using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointEffect : MonoBehaviour
{
    [SerializeField] Text text = default;

    public void Show(int score){
        text.text = score.ToString();
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveUp()
    {
        for(int i = 0; i < 20; i++)
        {
            yield return null;
            transform.Translate(0, 0.1f, 0);
            Color textColor = text.color;
            textColor.a = textColor.a * 0.9f;
            text.color = textColor;
        }
        Destroy(gameObject, 0.2f);
    }
}
 