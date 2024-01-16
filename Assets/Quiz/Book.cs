using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    //public GameObject book;
    private Animator anim;
    public GameObject cam;
    FollowCamera FollowCamera;
    Transform bookTr;
    void Start()
    {
        FollowCamera = cam.GetComponent<FollowCamera>();
        //bookTr = book.GetComponent<Transform>();//
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // You can add any camera-related code here if needed
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerCtrl aa = other.gameObject.GetComponent<PlayerCtrl>();
            Debug.Log("AAA");
            anim.SetTrigger("Get");
            other.transform.forward = -gameObject.transform.forward;
            FollowCamera.camin();
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        FollowCamera.camout();
    }

}


