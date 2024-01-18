using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upwitch : MonoBehaviour
{
    public GameObject uppos;
    public GameObject bossroom;
    public GameObject magicpos;
    public GameObject magic;
    public GameObject player;
    
    int storyStep = 0;
    int storyStep1 = 0;
    float storyTime = 1;
    bool setting = false;
    bool stayState = false;
    Player_Health GetPlayer_Health;
    FollowCamera camera;

    [SerializeField] UIContral uiContral;
    [SerializeField] GameObject powerUPWindow;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.GetComponent<FollowCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            GetPlayer_Health = player.GetComponent<Player_Health>();
        }

        storyTime += Time.deltaTime;
        if (setting)
        {
            player.transform.position = new Vector3(uppos.transform.position.x, player.transform.position.y, uppos.transform.position.z);
            camera.camin();
        }

        if(stayState)       //�浹 ���� ���� �ִٸ�
            HandleInput();

    }

    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && storyTime > 0.5)
        {
            player = other.gameObject;
            //HandleInput();
            stayState = true;   //�ν� �������� �ִ� ���¸� ��Ÿ���� ���� -> ������
            if (storyStep > 0 && storyStep <= 6)
            {
                
                other.gameObject.transform.forward = -transform.forward;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && storyTime > 0.5)
        {
                stayState = false;
        }
    }
    float a = 0;
    void HandleInput()
    {
        if (!GetPlayer_Health.BossHunting)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (storyStep == 2)
                {
                    uiContral.PowerUpUIContral();
                    uiContral.ConversationRemove();
                }

                else
                {
                    storyTime = 0;
                    story1();
                    storyStep++;
                }
            }
            
            //else if (Input.GetKeyDown(KeyCode.G) && storyStep > 0)
            //{
            //    storyStep--;
            //    story1();
            //}

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (storyStep1 == 2)
                {
                    Debug.Log("aaa");
                    uiContral.PowerUpUIContral();
                    uiContral.ConversationRemove();
                }

                else
                {
                    storyTime = 0;
                    story2();
                    storyStep1++;
                }
            }
            
            //else if (Input.GetKeyDown(KeyCode.G))
            //{
            //    storyStep1--;
            //    story2();
            //}

        }
    }
    public void story1()
    {
        switch (storyStep)
        {
            case 0:
                npctalk("��, ���谡��! ����� �г븦 ��ġ������? ���� ��ȥ�� �������� �� ���� �� �־�.");
                setting = true;
                break;
            case 1:
                npctalk("���� ������ �� ��ȥ���� �� ���� ���� ��ȭ���ٰ�. ����� ���� Ȥ������ �״ϱ� ���̾�.");
                break;
            case 2:
                npctalk("�� ���谡�μ��� ��ȥ�� �̹� ����������, ������ ���ֹ��� ��ġ�� ������ ��Ű�� �־�");
                break;
            case 3:
                npctalk("�� �༮�� ����������, ������ �� ������ �ž�.");
                break;
            case 4:
                npctalk("��ġ�� ������ �ִ� ������ ���� ���εǾ� �־�. ���� ��� ��� ������ �����ؾ� ��.");
                break;
            case 5:
                npctalk("4���� ���� ��� å���� ������ ������ ��, ��ġ�� ��ġ��� �� ��ȥ���� �� ���� ���� �� ��ȭ�����ٰ�.");
                break;
            case 6:
                npctalk("�̹����� �༮�� ��ġ�� ����� ���� �����ž�. ������ �� ��¦ �� �ٰ����ٴ� ������ ���. ����� ����, ���谡��!");
                break;
            case 7:
                uiContral.ConversationRemove();
                Instantiate(magic, magicpos.transform.position, magicpos.transform.rotation);
                setting = false;
                Invoke("onMagic", 0.25f);
                break;
            case 12:
                npctalk("������ ���� ��ȥ�� ���� ȲȦ��. �� ��ä�� �̹� ���� ��� �־�.");
                Invoke("ASD", 2F);
                break;
            case 13:
                npctalk("�� ������ �� ��ȥ�� �󸶳� ������ ���ϱ�? �����Լ� �������� �������� ����̻��̾�.");
                Invoke("ASD", 2F);
                break;
            case 14:
                npctalk("Ȥ�� ��� �� ȥ�㸻�� �����? �ƴ϶�� ������. ��·��, ������ ���� �̾߱��ϰ� ���� �� ������ ���غ�.");
                Invoke("ASD", 2F);
                break;

            default:
                npctalk("���� ������ �ϰ� ���� �����־�?");
                Invoke("ASD", 2F);
                break;

        }
    }
    void story2()
    {
        switch (storyStep1)
        {
            case 0:
                npctalk("���� ��ġ�� óġ�߱�! �� ��ȥ�� �����ϰ� ������ �־�.");
                setting = true;
                break;
            case 1:
                npctalk("���� ���� ��ȥ���� �ѹ� �� ��ȭ���ٰ�.");
                break;
            case 2:
                npctalk("��, ��ġ�� ��� óġ�߾�. ������ �̷��Ա��� �س��� ������. �� ���迡 �Բ��� �� �־� �⻵.");
                break;
            case 3:
                npctalk("�� ������ ��ȥ�� ���� ���谡�μ� �ʸ� �ϰ� ���󰡰ھ�.");
                break;
            case 4:
                npctalk("���� ������ ������ ����, ���谡��! �ű⿡�� ������ ������ �ʸ� ��ٸ��� �����ž�.");
                break;
            case 5:
                npctalk("������ ������, �װ����� ��ٸ��� ���� �����Ӹ��� �ƴ� �ž�.");
                break;
            case 6:
                uiContral.ConversationRemove();
                Instantiate(bossroom, magicpos.transform.position, magicpos.transform.rotation);
                setting = false;
                break;
            default:
                break;
        }
    }
    void npctalk(string npcText)
    {
        uiContral.Conversation(npcText);
    }

    public void AddStoryStep()
    {
        if (!GetPlayer_Health.BossHunting)
        {
            story1();
            ++storyStep;
        }
        else
        {
            story2();
            ++storyStep1;
        }
    }
    void ASD()
    {
        uiContral.ConversationRemove();   
    }
}
