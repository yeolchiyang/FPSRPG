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
        // 초기화나 다른 설정은 필요하면 이곳에 추가
    }

    void Update()
    {
        TextTime = TextTime + Time.deltaTime;
        // Update 함수는 대화와 관련이 없다면 빈 상태로 유지

        if (stayState)       //충돌 영역 내에 있다면
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
                DisplayDialog("하하, 잠시만 이야기 좀 하지. 성격이 급한 모험가로구나?");
                break;
            case 1:
                DisplayDialog("어서 와, 이 던전에는 정말 놀라운 보물이 숨겨저 있어.");
                break;
            case 2:
                DisplayDialog("하지만, 그 보물을 획득하려면 우선 몇 가지 임무를 수행해야해. 하지만 아직 너의 실력은 좀 부족해 보이는군.");
                break;
            case 3:
                DisplayDialog("일단 던전에 나오는 몬스터와 고대 정령 고르를 해치워 너의 영혼을 더욱 단련하자. 그러면 너는 더 강해질 거야.");
                break;
            case 4:
                DisplayDialog("고르는 몬스터를 " + skeletonHunting + "마리 정도 잡으면 복수를 위해 나타날 거야.");
                break;
            case 5:
                DisplayDialog("나는 비밀의 방에서 너를 기다릴게. 참고로, 비밀의 방은 고르을 잡으면 열릴 거야. 행운을 빌어, 모험가!");
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
