using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class datesave : MonoBehaviour
{
    GameObject player;
    Player_Health playerHealth;
    float playerHp = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<Player_Health>();
        playerHp = playerHealth.currentHp;

        
    }
    void Start()
    {  
        playerHealth.currentHp = playerHp; 
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
