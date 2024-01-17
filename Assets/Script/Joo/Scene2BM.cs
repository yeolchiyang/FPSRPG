using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Scene2BM : MonoBehaviour
{
    AudioSource Background;
    [SerializeField]
    AudioClip battlesound;
    AudioClip origin;

    [SerializeField] GameObject hydra;
    HYDRABoss boss;

    // Start is called before the first frame update
    void Start()
    {
        Background = GetComponent<AudioSource>();
        Background.Play();
        origin = Background.clip;
        boss = hydra.GetComponent<HYDRABoss>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.IsBattlecheck())
        {
            if (Background.clip != battlesound)
            {
                Background.Stop();
                Background.clip = battlesound;
                Background.Play();
            }
        }
        else
        {
            if (Background.clip != origin)
            {
                Background.Stop();
                Background.clip = origin;
                Background.Play();
            }
        }
    }
}
