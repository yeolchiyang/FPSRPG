using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2BM : MonoBehaviour
{
    AudioSource Background;
    [SerializeField]
    AudioClip Battlesound;
    AudioClip Back;

    [SerializeField] GameObject hydra;
    HYDRABoss boss;
    // Start is called before the first frame update
    void Start()
    {
        Background = GetComponent<AudioSource>();
        Background.Play();
        Back = Background.clip;
        boss = GetComponent<HYDRABoss>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            Background.Stop();
            Background.clip = Battlesound;
            Background.Play();
        }
        else if (Input.GetKey(KeyCode.B))
        {
            Background.Stop();
            Background.clip = Back;
            Background.Play();
        }
    }
}
