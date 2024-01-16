using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContral : MonoBehaviour
{
    int currentSetArmIndex;

    //GameObject Boss;
    public GameObject Player;
    

    Player_Health player;

    [SerializeField] upwitch witch;
    [SerializeField] GameObject GameMenuWindow;
    [SerializeField] GameObject PowerUpUI;
    [SerializeField] GameObject conversation;

    bool gameMenuEnable = false;
    bool powerUpEnabled = false;
    
    [SerializeField] GameObject[] Aims;

    [SerializeField] UnityEngine.UI.Image[] Arms;
    [SerializeField] UnityEngine.UI.Text PlayerLv;
    [SerializeField] UnityEngine.UI.Text ForceSoulCount;
    [SerializeField] UnityEngine.UI.Text conversationText;

    List<GameObject> aims = new List<GameObject>();

    private void Start()
    {
        player = Player.GetComponent<Player_Health>();
        //Boss boss = Boss.GetComponent<Boss>();
        for (int i = 0; i < Aims.Length; ++i) {
            aims.Add(Aims[i]);
        }

        GameMenuWindow.SetActive(false);
        PowerUpUI.SetActive(false);
        //conversation.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        setAim(0);
    }

    void Update()
    {
        PlayerLv.text = "" + player.lv;
        ForceSoulCount.text = "" + player.forceSoul;

        if (Input.GetKeyDown("1"))  //basic gun
        {
            if (aims[currentSetArmIndex] != null)
                resetAim();
            setAim(0);
        }

        if (Input.GetKeyDown("2"))  //aim2
        {
            if (aims[currentSetArmIndex] != null)
                resetAim();
            setAim(1);
        }

        if (Input.GetKeyDown("3"))  //aim3
        {
            if (aims[currentSetArmIndex] != null)
                resetAim();
            setAim(2);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuWindowContral();
        }
    }

    void setAim(int armIndex)
    {
        if (!aims[armIndex].activeSelf)
        {
            aims[armIndex].SetActive(!aims[armIndex].activeSelf);
            currentSetArmIndex = armIndex;
            Arms[armIndex].color = Color.red;
        }
    }

    void resetAim()
    {
        aims[currentSetArmIndex].SetActive(false);
        Arms[currentSetArmIndex ].color = new Color(0.17f, 0.17f, 0.17f);
    }

    public void PowerUpUIContral()
    {
        powerUpEnabled = !powerUpEnabled;

        if (powerUpEnabled == true)
        {
            PowerUpUI.SetActive(powerUpEnabled);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            PowerUpUI.SetActive(powerUpEnabled);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            witch.AddStoryStep();
        }
    }

    public void MenuWindowContral()
    {
        gameMenuEnable = !gameMenuEnable;

        if (gameMenuEnable == true)
        {
            GameMenuWindow.SetActive(gameMenuEnable);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else
        {
            GameMenuWindow.SetActive(gameMenuEnable);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }

    public void Conversation(string str)
    {
        if (!conversation.activeSelf)
        {
            conversation.SetActive(true);
        }

        conversationText.text = str;
    }

    public void ConversationRemove()
    {
        conversation.SetActive(false);
    }
}
