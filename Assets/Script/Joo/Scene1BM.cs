using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1BM : MonoBehaviour
{
    AudioSource Background;
    [SerializeField]
    AudioClip[] sounds;
    AudioClip Back;
    // Start is called before the first frame update
    void Start()
    {
        Background = GetComponent<AudioSource>();
        Background.Play();
        Back = Background.clip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            Background.Stop();
            Background.clip = sounds[0];
            Background.Play();
        }
        else if (Input.GetKey(KeyCode.G))
        {
            Background.Stop();
            Background.clip = sounds[1];
            Background.Play();
        }
        else if(Input.GetKey(KeyCode.B))
        {
            Background.Stop();
            Background.clip = Back;
            Background.Play();
        }
    }
}
