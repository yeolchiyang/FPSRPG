using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Title : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("Scene1");
        }
    }
}
