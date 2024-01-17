using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bosswitch : MonoBehaviour
{
    [SerializeField] UIContral uiContral;
    GameObject canvas;
    public int storyStep = 0;
    float TextTime = 1;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        uiContral = canvas.GetComponent<UIContral>();
    }

    void Update()
    {
        TextTime = TextTime + Time.deltaTime;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            if (Input.GetKeyDown(KeyCode.G) && TextTime >= 0.5f && storyStep > 0)
            {
                storyStep--;
                HandleStory();
            }
            if (Input.GetKeyDown(KeyCode.F) && TextTime >= 0.5f)
            {
                TextTime = 0;
                HandleStory();
                storyStep++;
                if (storyStep > 5)
                {
                    ASD();
                    Destroy(gameObject);
                }
            }

        }
    }

    void HandleStory()
    {
        switch (storyStep)
        {
            case 0:
                DisplayDialog("���, ���谡. �̰��� �ٷ� ������ ���̾�.");
                break;

            case 1:
                DisplayDialog("������ ������ ����ֳİ�?");
                break;

            case 2:
                DisplayDialog("�� ������ ������ �����־�, ���谡��. �� ��ȥ�� �ٷ� ���� ������ �����̾�.");
                break;

            case 3:
                DisplayDialog("�� ���п� ����Ʈ1, ����Ʈ2�� ó���� �� �־���. ���� ���� �����ο��� �ž�.");
                break;

            case 4:
                DisplayDialog("�׷�, ���� �����ϰ� �Ƹ��ٿ� ��ȥ�� ����. �װԼ� ���� ������ ã�� �� �ִٸ�...");
                break;

            default:
                break;
        }
    }

    void DisplayDialog(string dialogText)
    {
        uiContral.Conversation(dialogText);
    }
    void ASD()
    {
        uiContral.ConversationRemove();
    }
}
