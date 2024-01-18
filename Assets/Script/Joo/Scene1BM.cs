using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMEnum{
    Normal,
    Lich,
    Tree
}


public class Scene1BM : MonoBehaviour
{
    public static Scene1BM sceneBgm;
    AudioSource Background;
    [SerializeField]
    AudioClip[] sounds;
    AudioClip origin;
    // Start is called before the first frame update
    private void Awake()
    {
        Background = GetComponent<AudioSource>();
        Background.Play();
        origin = Background.clip;
        SetCurrentBackGroundSounds(BGMEnum.Normal);
        if (sceneBgm == null)
        {
            sceneBgm = this;
        }
    }
    


    public void SetCurrentBackGroundSounds(BGMEnum bgmEnum)
    {
        if (bgmEnum == BGMEnum.Lich)
        {
            Debug.Log("Lich 시작~");
            Background.Stop();
            Background.clip = sounds[0];
            Background.Play();
        }
        else if (bgmEnum == BGMEnum.Tree)
        {
            Debug.Log("Tree 시작~");
            Background.Stop();
            Background.clip = sounds[1];
            Background.Play();
        }
        else if (bgmEnum == BGMEnum.Normal)
        {
            Debug.Log("Normal 시작~");
            Background.Stop();
            Background.clip = origin;
            Background.Play();
        }
    }
}
