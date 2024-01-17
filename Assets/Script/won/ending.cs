using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ending : MonoBehaviour
{
    public int storyStep = 0;
    float TextTime = 0;
    [SerializeField] UIContral uiContral;
    GameObject canvas;
    public bool endingtextstart = false;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        uiContral = canvas.GetComponent<UIContral>();
    }

    // Update is called once per frame
    void Update()
    {
        TextTime += Time.deltaTime;
        if (TextTime > 2f && endingtextstart)
        {
           
            if (storyStep == 0 && Input.GetKeyDown(KeyCode.F)) 
            {
                HandleStory(storyStep);
                storyStep++;
                TextTime = 0;
            }
            else if (storyStep == 3 && Input.GetKeyDown(KeyCode.F))
            {
                storyStep++;
                HandleStory(storyStep);
                TextTime = 0;
                
            }
            else if (storyStep > 0 && storyStep <3)
            {

                TextTime = 0;
                HandleStory(storyStep);
                storyStep++;

            }
        }
    }


    void HandleStory(int storyStep)
    {
        switch (storyStep)
        {
            case 0:
                DisplayDialog("��, �ʿ��� ���ٴ�. �̷� ��¥ ���谡���� �ο򿡼� ���� �й��ϴٴ�.");
                break;

            case 1:
                DisplayDialog("�� ���� �Ҿ���... �� ��ȥ�� ��Ż���� ���� �� �ʹ� ���ϱ���!");

                break;

            case 2:
                DisplayDialog("�׷�, ������. �׷��� ���� �г�� ���ָ� ���ϱ�� ���� �Ŵ�.");
                break;

            case 3:
                DisplayDialog("�� ���� ������ ���� ����������, ���� �ʸ� ��� �����ϸ���!");

                break;
            case 4:
                SceneManager.LoadScene("Scene3_empty");
                break;

            default:
                break;

        }
    }
    void DisplayDialog(string dialogText)
    {
        uiContral.Conversation(dialogText);
    }
}
