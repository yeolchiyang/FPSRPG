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
        // ���� Ȱ��ȭ�� ���� ������
        Scene currentScene = SceneManager.GetActiveScene();

        // ���� ���� �̸��� ��ǥ ���� �̸��� ������ Ȯ��
        if (currentScene.name == targetSceneName)
        {
            // ���� "Scene1"�� ���� ���� ������Ʈ�� Ȱ��ȭ
            this.gameObject.SetActive(true);
        }
        else
        {
            // �ٸ� ���� Ȱ��ȭ�Ǿ� ���� ��� ���� ������Ʈ�� ��Ȱ��ȭ
            this.gameObject.SetActive(false);
        }
    }
}
