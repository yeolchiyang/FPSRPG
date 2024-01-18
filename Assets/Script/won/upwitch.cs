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

        if(stayState)       //충돌 영역 내에 있다면
            HandleInput();

    }

    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && storyTime > 0.5)
        {
            player = other.gameObject;
            //HandleInput();
            stayState = true;   //인식 범위내에 있는 상태를 나타내는 변순 -> 진선윤
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
                npctalk("오, 모험가야! 고대의 분노를 해치웠구나? 너의 영혼이 강해지는 걸 느낄 수 있어.");
                setting = true;
                break;
            case 1:
                npctalk("이제 강해진 네 영혼으로 네 힘를 더욱 강화해줄게. 세계는 더욱 혹독해질 테니까 말이야.");
                break;
            case 2:
                npctalk("네 모험가로서의 영혼은 이미 강해졌지만, 아직은 저주받은 리치가 보물을 지키고 있어");
                break;
            case 3:
                npctalk("그 녀석은 강력하지만, 강해진 넌 괜찮을 거야.");
                break;
            case 4:
                npctalk("리치는 던전에 있는 마법의 고서로 봉인되어 있어. 고서를 모두 모아 봉인을 해제해야 해.");
                break;
            case 5:
                npctalk("4개의 고서를 모아 책장의 봉인을 해제한 후, 리치를 해치우면 네 영혼으로 네 힘를 더욱 더 강화시켜줄게.");
                break;
            case 6:
                npctalk("이번에도 녀석을 해치면 비밀의 방이 열릴거야. 보물에 한 발짝 더 다가갔다는 느낌이 드네. 행운을 빌게, 모험가야!");
                break;
            case 7:
                uiContral.ConversationRemove();
                Instantiate(magic, magicpos.transform.position, magicpos.transform.rotation);
                setting = false;
                Invoke("onMagic", 0.25f);
                break;
            case 12:
                npctalk("강해진 너의 영혼은 정말 황홀해. 그 광채는 이미 눈에 띄고 있어.");
                Invoke("ASD", 2F);
                break;
            case 13:
                npctalk("더 강해진 네 영혼은 얼마나 달콤한 맛일까? 나에게서 느껴지는 에너지가 상상이상이야.");
                Invoke("ASD", 2F);
                break;
            case 14:
                npctalk("혹시 방금 내 혼잣말을 들었니? 아니라면 괜찮아. 어쨌든, 나에게 뭔가 이야기하고 싶은 게 있으면 말해봐.");
                Invoke("ASD", 2F);
                break;

            default:
                npctalk("아직 나에게 하고 싶은 말이있어?");
                Invoke("ASD", 2F);
                break;

        }
    }
    void story2()
    {
        switch (storyStep1)
        {
            case 0:
                npctalk("드디어 리치를 처치했군! 네 영혼은 찬란하게 빛나고 있어.");
                setting = true;
                break;
            case 1:
                npctalk("이제 너의 영혼으로 한번 더 강화해줄게.");
                break;
            case 2:
                npctalk("고르, 리치를 모두 처치했어. 솔직히 이렇게까지 해낼줄 몰랐어. 네 모험에 함께할 수 있어 기뻐.");
                break;
            case 3:
                npctalk("그 찬란한 영혼을 가진 모험가로서 너를 믿고 따라가겠어.");
                break;
            case 4:
                npctalk("이제 보물의 방으로 가자, 모험가야! 거기에는 찬란한 보물이 너를 기다리고 있을거야.");
                break;
            case 5:
                npctalk("하지만 조심해, 그곳에서 기다리는 것이 보물뿐만이 아닐 거야.");
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
