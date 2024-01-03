using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_animation;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody rb;
    private Transform tr;
    public float speed = 10f;
    public float turnspeed = 80f;
    public int JumpPower;
    bool IsJumping = true;
    Player_Anima anima;



    // Start is called before the first frame update
    void Start()
    {
        anima = GetComponent<Player_Anima>();
        IsJumping = false;
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis ("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward*v) + (Vector3.right*h);

        tr.Translate(moveDir.normalized * speed * Time.deltaTime);
        tr.Rotate(Vector3.up * turnspeed * Time.deltaTime * r);
       if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsJumping)
            {
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
                IsJumping=false;
                anima.Player_jump();
            }
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {

            IsJumping = true;
        }
    }
}
