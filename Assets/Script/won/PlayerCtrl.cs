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
    public float turnspeed = 80f;
    public int JumpPower;
    bool IsJumping = true;
    Player_Anima anima;

    public bool shooting = false;
    public void shootingcheck()
    {
        shooting = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        anima = GetComponent<Player_Anima>();
        IsJumping = false;
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    float timer = 0f;
    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis ("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward*v) + (Vector3.right*h);

        if (CheckHitWall(moveDir))
            moveDir = Vector3.zero;

        tr.Translate(moveDir.normalized * speed * Time.deltaTime);
        tr.Rotate(Vector3.up * turnspeed * Time.deltaTime * r);
        if (shooting)
        {
            tr.Rotate(Random.insideUnitSphere);
            if(timer > 0.3f)
            {
                shooting = false;
            }
        }
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
}