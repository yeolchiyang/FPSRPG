using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOOKSTARRT : MonoBehaviour
{
    Inventory inventory;
    GameObject invenObj;
    public GameObject book;
    // Start is called before the first frame update
    void Start()
    {
        invenObj = GameObject.Find("Inventory");
        inventory = invenObj.GetComponent<Inventory>();
        book.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && inventory.bookCount >= 4)
        {
            book.SetActive(true);
        }
    }
}
