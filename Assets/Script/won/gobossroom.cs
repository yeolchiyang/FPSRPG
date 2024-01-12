using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gobossroom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            sceneChange();
        }
    }
    public void sceneChange()
    {
        // "YourSceneName" æ¿¿∏∑Œ ¿Ãµø
        SceneManager.LoadScene("Scene2");
    }
}
