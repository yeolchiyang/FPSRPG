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
                npctalk("오, 모험가야! 엘리트1을 잡았다고? 멋진 일이야. 네 영혼이 강해지는 걸 느낄 수 있어.");
                break;
            case 1:
                npctalk("이제 강해진 네 영혼으로 네 장비를 더욱 강화해줄게. 세계는 더욱 어려워질 테니까 말이야.");
                break;
            case 2:
                npctalk("네 모험가로서의 영혼은 이미 강해졌지만, 아직은 엘리트2가 보물을 지키고 있어");
                break;
            case 3:
                npctalk("엘리트2는 강력하지만, 강해진 넌 괜찮을 거야.");
                break;
            case 4:
                npctalk("엘리트2는 던전에 있는 고대의 봉인책으로 봉인되어 있어. 책들을 찾아 모아 책장에서 봉인을 해제해야 해.");
                break;
            case 5:
                npctalk("책들을 모아 책장에서 봉인을 해제한 후, 엘리트2를 사냥하면 단련된 비밀의 방에서 네 영혼으로 장비를 강화해줄게.");
                break;
            case 6:
                npctalk("이번에도 엘리트2를 잡으면 비밀의 방이 열릴거야. 보물에 한 발짝 더 다가갔다는 느낌이 드네. 행운을 빌게, 모험가야!");
                break;
            case 12:
                npctalk("강해진 너의 영혼은 정말 황홀해. 그 광채는 이미 눈에 띄고 있어.");
                break;
            case 13:
                npctalk("더 강해진 네 영혼은 얼마나 달콤한 맛일까? 나에게서 느껴지는 에너지가 상상이상이야.");
                break;
            case 14:
                npctalk("혹시 방금 내 혼잣말을 들었니? 아니라면 괜찮아. 어쨌든, 나에게 뭔가 이야기하고 싶은 게 있으면 말해봐.");
                break;

            default:
                npctalk("아직 나에게 하고 싶은 말이있어?");
                break;

        }
    }
    void story2()
    {
        switch (storyStep1)
        {
            case 0:
                npctalk("드디어 엘리트2를 처치했군! 네 영혼은 찬란하게 빛나고 있어.");
                break;
            case 1:
                npctalk("우선 너의 강화된 영혼으로 장비를 한번 더 강화해줄게.");
                break;
            case 2:
                npctalk("엘리트1, 엘리트2를 모두 처치했어. 나는 너무 행복해! 네 영웅의 길에 함께할 수 있어 기뻐.");
                break;
            case 3:
                npctalk("그 찬란한 영혼을 가진 모험가로서 너를 믿고 따라가겠어.");
                break;
            case 4:
                npctalk("이제 보물의 방으로 가자, 모험가야! 거기에는 찬란한 보물과 함께 너를 기다리고 있어.");
                break;
            case 5:
                npctalk("하지만 주의해, 그곳에서 기다리는 것이 보물뿐만이 아닐 거야.");
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
