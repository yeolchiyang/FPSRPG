using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upwitch : MonoBehaviour
{
    Player_Health Player;
    public GameObject player;
    int storyStep = 0;
    int storyStep1 = 0;
    float storyTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        Player = player.GetComponent<Player_Health>();
    }

    // Update is called once per frame
    void Update()
    {
        storyTime += Time.deltaTime;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F) && storyTime > 0.5 && !Player.BossHunting)
            {
                storyTime = 0;
                story1();
                storyStep++;
            }
            else if (Input.GetKeyDown(KeyCode.G) && storyTime > 0.5 && !Player.BossHunting && storyStep > 0)
            {
                storyStep--;
                storyTime = 0;
                story1();
            }
            if (Input.GetKeyDown(KeyCode.F) && storyTime > 0.5 && Player.BossHunting)
            {
                storyTime = 0;
                story2();
                storyStep1++;
            }
            else if (Input.GetKeyDown(KeyCode.G) && storyTime > 0.5 && Player.BossHunting && storyStep > 0)
            {
                storyStep1--;
                storyTime = 0;
                story2();
            }
        }
    }
    void story1()
    {
        switch (storyStep)
        {
            case 0:
                npctalk("��, ���谡��! ����Ʈ1�� ��Ҵٰ�? ���� ���̾�. �� ��ȥ�� �������� �� ���� �� �־�.");
                break;
            case 1:
                npctalk("���� ������ �� ��ȥ���� �� ��� ���� ��ȭ���ٰ�. ����� ���� ������� �״ϱ� ���̾�.");
                break;
            case 2:
                npctalk("�� ���谡�μ��� ��ȥ�� �̹� ����������, ������ ����Ʈ2�� ������ ��Ű�� �־�");
                break;
            case 3:
                npctalk("����Ʈ2�� ����������, ������ �� ������ �ž�.");
                break;
            case 4:
                npctalk("����Ʈ2�� ������ �ִ� ����� ����å���� ���εǾ� �־�. å���� ã�� ��� å�忡�� ������ �����ؾ� ��.");
                break;
            case 5:
                npctalk("å���� ��� å�忡�� ������ ������ ��, ����Ʈ2�� ����ϸ� �ܷõ� ����� �濡�� �� ��ȥ���� ��� ��ȭ���ٰ�.");
                break;
            case 6:
                npctalk("�̹����� ����Ʈ2�� ������ ����� ���� �����ž�. ������ �� ��¦ �� �ٰ����ٴ� ������ ���. ����� ����, ���谡��!");
                break;
            case 12:
                npctalk("������ ���� ��ȥ�� ���� ȲȦ��. �� ��ä�� �̹� ���� ��� �־�.");
                break;
            case 13:
                npctalk("�� ������ �� ��ȥ�� �󸶳� ������ ���ϱ�? �����Լ� �������� �������� ����̻��̾�.");
                break;
            case 14:
                npctalk("Ȥ�� ��� �� ȥ�㸻�� �����? �ƴ϶�� ������. ��·��, ������ ���� �̾߱��ϰ� ���� �� ������ ���غ�.");
                break;

            default:
                npctalk("���� ������ �ϰ� ���� �����־�?");
                break;

        }
    }
    void story2()
    {
        switch (storyStep1)
        {
            case 0:
                npctalk("���� ����Ʈ2�� óġ�߱�! �� ��ȥ�� �����ϰ� ������ �־�.");
                break;
            case 1:
                npctalk("�켱 ���� ��ȭ�� ��ȥ���� ��� �ѹ� �� ��ȭ���ٰ�.");
                break;
            case 2:
                npctalk("����Ʈ1, ����Ʈ2�� ��� óġ�߾�. ���� �ʹ� �ູ��! �� ������ �濡 �Բ��� �� �־� �⻵.");
                break;
            case 3:
                npctalk("�� ������ ��ȥ�� ���� ���谡�μ� �ʸ� �ϰ� ���󰡰ھ�.");
                break;
            case 4:
                npctalk("���� ������ ������ ����, ���谡��! �ű⿡�� ������ ������ �Բ� �ʸ� ��ٸ��� �־�.");
                break;
            case 5:
                npctalk("������ ������, �װ����� ��ٸ��� ���� �����Ӹ��� �ƴ� �ž�.");
                break;
            default:
                break;
        }
    }
    void npctalk(string npcText)
    {
        Debug.Log(npcText);
    }
}
