using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //AudioSource -> スピーカー
    //AudioClip -> CD

    [SerializeField] AudioSource audioSourceBGM = default;
    [SerializeField] AudioClip[] audioClipsBGM = default;

    [SerializeField] AudioSource audioSourceSE = default;
    [SerializeField] AudioClip[] audioClipsSE = default;

    public enum BGM
    {
        Title,
        Main
    }

    public enum SE  //soundeffect
    {
        Touch,
        Destroy
    }

    //シングルトン化
    public static SoundManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(BGM bgm)
    {
        audioSourceBGM.clip = audioClipsBGM[(int)bgm];
        audioSourceBGM.Play();
    }

    public void PlaySE(SE se)
    {
        audioSourceSE.PlayOneShot(audioClipsSE[(int)se]);
    }
}
