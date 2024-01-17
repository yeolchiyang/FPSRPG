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
                DisplayDialog("어서와, 모험가. 이곳이 바로 보물의 방이야.");
                break;

            case 1:
                DisplayDialog("찬란한 보물이 어디있냐고?");
                break;

            case 2:
                DisplayDialog("그 찬란한 보물은 여기있어, 모험가야. 네 영혼이 바로 나의 소중한 보물이야.");
                break;

            case 3:
                DisplayDialog("네 덕분에 엘리트1, 엘리트2를 처리할 수 있었어. 이제 나는 자유로워진 거야.");
                break;

            case 4:
                DisplayDialog("그래, 이제 찬란하고 아름다운 영혼을 내놔. 네게서 나의 보물을 찾을 수 있다면...");
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
