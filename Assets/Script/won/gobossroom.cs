using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gobossroom : MonoBehaviour
{
    GameObject player;
    Player_Health health;
    Inventory inventory;
    GameObject CANVAS1;
    GameObject CANVAS2;
    Status_Inventory inventoryStatus;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        CANVAS1 = GameObject.Find("Inventory");
        CANVAS2 = GameObject.Find("StatusArea");
        health = player.GetComponent<Player_Health>();
        inventory = CANVAS1.GetComponent<Inventory>();
        inventoryStatus = CANVAS2.GetComponent<Status_Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            datesave();
            Invoke("sceneChange", 1F);
        }
    }
    public void sceneChange()
    {
        // "YourSceneName" æ¿¿∏∑Œ ¿Ãµø
        SceneManager.LoadScene("Scene2");
    }
    void datesave()
    {
        //health.SAVE();
        Debug.Log("Current Health: " + health.currentHp);
        PlayerPrefs.SetFloat("player_CurretHp", health.currentHp);
        PlayerPrefs.SetFloat("player_CurretExp", health.currentExp);
        PlayerPrefs.SetFloat("player_CurretMp", health.currentMp);
        PlayerPrefs.SetInt("player_forceSoul", health.forceSoul);
        PlayerPrefs.SetInt("player_Lv", health.lv);
        PlayerPrefs.SetInt("player_W1", inventoryStatus.status[0]);
        PlayerPrefs.SetInt("player_W2", inventoryStatus.status[1]);
        PlayerPrefs.SetInt("player_W3", inventoryStatus.status[2]);
        PlayerPrefs.SetInt("player_Vitality", inventoryStatus.status[3]);
        PlayerPrefs.SetInt("player_Armor", inventoryStatus.status[4]);
        PlayerPrefs.SetInt("player_hillPortion", inventory.hillPortionCount);
        PlayerPrefs.SetInt("player_debinePortion", inventory.debinePortionCount);
        Debug.Log(PlayerPrefs.GetFloat("player_CurretHp"));
    }
}
