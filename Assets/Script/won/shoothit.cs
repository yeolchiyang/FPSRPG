using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class shoothit : MonoBehaviour
{
    Skeleton skeleton;
    public float weaponDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
    }
    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            skeleton = collision.gameObject.GetComponent<Skeleton>();
            skeleton.SetDamaged(weaponDamage);
        }

    }
}
