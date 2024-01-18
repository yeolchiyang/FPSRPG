using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour
{
    [SerializeField] UIContral uiContral;
    public int storyStep = 0;

    int skeletonHunting = 10;
    float TextTime = 1;
    bool stayState = false;
    
    void Start()
    {
        // �ʱ�ȭ�� �ٸ� ������ �ʿ��ϸ� �̰��� �߰�
    }

    void Update()
    {
        TextTime = TextTime + Time.deltaTime;
        // Update �Լ��� ��ȭ�� ������ ���ٸ� �� ���·� ����

        if (stayState)       //�浹 ���� ���� �ִٸ�
            CheckConversation();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            stayState = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stayState = false;
        }
    }

    public void CheckConversation()
    {
        //if (Input.GetKeyDown(KeyCode.G) && storyStep > 0)
        //{
        //    storyStep--;
        //    HandleStory();
        //}

        if (Input.GetKeyDown(KeyCode.F) && TextTime >= 0.5f)
        {
            TextTime = 0;
            storyStep++;
            HandleStory();


            if (storyStep > 5)
            {
                uiContral.ConversationRemove();
                Destroy(gameObject);
            }
        }
    }

    void HandleStory()
    {
        switch (storyStep)
        {
            case 0:
                DisplayDialog("����, ��ø� �̾߱� �� ����. ������ ���� ���谡�α���?");
                break;
            case 1:
                DisplayDialog("� ��, �� �������� ���� ���� ������ ������ �־�.");
                break;
            case 2:
                DisplayDialog("������, �� ������ ȹ���Ϸ��� �켱 �� ���� �ӹ��� �����ؾ���. ������ ���� ���� �Ƿ��� �� ������ ���̴±�.");
                break;
            case 3:
                DisplayDialog("�ϴ� ������ ������ ���Ϳ� ��� ���� ���� ��ġ�� ���� ��ȥ�� ���� �ܷ�����. �׷��� �ʴ� �� ������ �ž�.");
                break;
            case 4:
                DisplayDialog("���� ���͸� " + skeletonHunting + "���� ���� ������ ������ ���� ��Ÿ�� �ž�.");
                break;
            case 5:
                DisplayDialog("���� ����� �濡�� �ʸ� ��ٸ���. �����, ����� ���� ���� ������ ���� �ž�. ����� ����, ���谡!");
                break;

            default:
                break;
        }
    }

    void DisplayDialog(string dialogText)
    {
        uiContral.Conversation(dialogText);
    }

    public void PlayerStop()
    {
        HandleStory();
    }
}
