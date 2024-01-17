using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allbook : MonoBehaviour
{
    booktrigger GetBooktrigger;
    public GameObject book;
    // Start is called before the first frame update
    void Start()
    {
        GetBooktrigger = book.GetComponent<booktrigger>();
        Invoke("Calltrigger", 2F);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Calltrigger()
    {
        GetBooktrigger.bookstart();
    }
}
