using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicAim : MonoBehaviour
{
    GameObject aim;
    Animator anim;

    [SerializeField] int aimIndex;

    private void Awake()
    {
        gameObject.SetActive(false);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (aimIndex)
        {
            case 1:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    ShootToAim();
                } break;
            default: break;
        }
    }

    public void ShootToAim()
    {
        anim.SetTrigger("Shoot");
    }
}
