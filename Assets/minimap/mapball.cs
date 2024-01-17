using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mapball : MonoBehaviour
{
    public string targetSceneName = "Scene1";

    //public GameObject objectToActivate;

    void Start()
    {
        // 현재 활성화된 씬을 가져옴
        Scene currentScene = SceneManager.GetActiveScene();

        // 현재 씬의 이름이 목표 씬의 이름과 같은지 확인
        if (currentScene.name == targetSceneName)
        {
            // 씬이 "Scene1"일 때만 게임 오브젝트를 활성화
            this.gameObject.SetActive(true);
        }
        else
        {
            // 다른 씬이 활성화되어 있을 경우 게임 오브젝트를 비활성화
            this.gameObject.SetActive(false);
        }
    }
}
