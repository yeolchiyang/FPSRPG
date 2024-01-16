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
                DisplayDialog("하, 너에게 지다니. 이런 초짜 모험가와의 싸움에서 내가 패배하다니.");
                break;

            case 1:
                DisplayDialog("내 힘을 잃었어... 네 영혼을 강탈하지 못한 게 너무 분하구나!");

                break;

            case 2:
                DisplayDialog("그래, 도망쳐. 그러나 나의 분노와 저주를 피하기는 힘들 거다.");
                break;

            case 3:
                DisplayDialog("넌 나의 원한을 피해 떠날지언정, 내가 너를 계속 저주하리라!");

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
