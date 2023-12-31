using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_animation;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody rb;
    private Transform tr;
    private BoxCollider bc;
    public float speed = 10f;
    public float turnspeed = 60f;
    public int JumpPower;
    bool IsJumping = true;
    bool asd = false;
    Player_Anima anima;

    // Start is called before the first frame update
    void Start()
    {
        anima = GetComponent<Player_Anima>();
        IsJumping = false;
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis ("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward*v) + (Vector3.right*h);

        if (CheckHitWall(moveDir))
            moveDir = Vector3.zero;
        if (!asd)
        {
            tr.Translate(moveDir.normalized * speed * Time.deltaTime);
            tr.Rotate(Vector3.up * turnspeed * Time.deltaTime * r);
        }
        else
        {
            
        }
        //tr.Rotate(Random.insideUnitCircle);

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

    bool CheckHitWall(Vector3 moveDir)
    {
        moveDir = transform.TransformDirection(moveDir);  // ���� ���͸� ���� ���ͷ�
        float scope = 1f;

        List<Vector3> rayPositions = new List<Vector3>();
        rayPositions.Add(transform.position + Vector3.up * 0.1f);
        rayPositions.Add(transform.position + Vector3.up * bc.size.y * 0.5f);
        rayPositions.Add(transform.position + Vector3.up * bc.size.y);

        foreach(Vector3 pos in rayPositions)
        {
            if(Physics.Raycast(pos, moveDir, out RaycastHit hit, scope))
            {
                if (hit.collider.CompareTag("Wall"))
                    return true;
            }
        }
        return false;
    }
    public void sss()
    {
        asd = true;
    }
}
