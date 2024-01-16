using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ending : MonoBehaviour
{
    public int storyStep = 0;
    float TextTime = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TextTime += Time.deltaTime;
        if (TextTime > 2f)
        {
           
            if (storyStep > 0 && Input.GetKeyDown(KeyCode.F)) 
            {
                HandleStory(storyStep);
                storyStep++;
                TextTime = 0;
            }
            else
            {
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


            default:
                break;

        }
    }
    void DisplayDialog(string dialogText)
    {
        Debug.Log(dialogText);
    }
}
