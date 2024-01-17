using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz1 : MonoBehaviour
{
    public static Quiz1 BookQuiz;

    public float shakeSpeed = 55.0f;
    public float shakeAmount = 0.007f;
    private Vector3 originPos;
 

    private void Start()
    {

        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
            float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            float shakeY = Mathf.Sin(Time.time * shakeSpeed * 1.2f) * shakeAmount;
            float shakeZ = Mathf.Sin(Time.time * shakeSpeed * 0.8f) * shakeAmount;

            transform.position = originPos + new Vector3(shakeX, shakeY, shakeZ);
        
    }

   
}
