using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour
{
    public int storyStep = 0;
    int skeletonHunting = 10;
    float TextTime = 1;

    void Start()
    {
        // �ʱ�ȭ�� �ٸ� ������ �ʿ��ϸ� �̰��� �߰�
    }

    void Update()
    {
        TextTime = TextTime + Time.deltaTime;
        // Update �Լ��� ��ȭ�� ������ ���ٸ� �� ���·� ����
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            if (Input.GetKeyDown(KeyCode.G) && storyStep > 0)
            {
                storyStep--;
                HandleStory();
            }
            if (Input.GetKeyDown(KeyCode.F) && TextTime >= 1f)
            {
                TextTime = 0;
                storyStep++;
                HandleStory();
                if (storyStep > 4)
                {
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
                DisplayDialog("����, ��ø� �̾߱� �� ����. ������ ���� ���谡�α���?");
                break;

            case 1:
                DisplayDialog("� ��, ���谡��. �� �������� ���� ���� ������ ���� �־�. ������, �� ������ ȹ���Ϸ��� �켱 �� ���� �ӹ��� �����ؾ���.");
                break;

            case 2:
                DisplayDialog("�׷��� ���� ���谡�� �Ƿ��� ���� �� ������ ���̴±�. �ϴ� ������ ������ ���Ϳ� ����Ʈ1�� ����ؼ� ���谡�� ��ȥ�� ���� �ܷ�����. �׷��� �� ������ �ž�.");
                break;

            case 3:
                DisplayDialog("����Ʈ1�� ���͸� " + skeletonHunting + "���� ���� ������ ������ ���� ��Ÿ�� �ž�.");
                break;

            case 4:
                DisplayDialog("���� ����� �濡�� �ʸ� ��ٸ���. �����, ����� ���� ����Ʈ1�� ������ ���� �ž�. ����� ����, ���谡��!");
                break;

            default:
                break;
        }
    }

    void DisplayDialog(string dialogText)
    {
        Debug.Log(dialogText);
    }

    public void PlayerStop()
    {
        HandleStory();
    }
}
