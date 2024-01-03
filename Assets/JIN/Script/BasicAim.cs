using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicAim : MonoBehaviour
{
    GameObject aim;
    Animator anim;

    private void Start()
    {
        gameObject.SetActive(false);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootToAim();
        }
    }

    void ShootToAim()
    {
        anim.SetTrigger("Shoot");
    }
}
