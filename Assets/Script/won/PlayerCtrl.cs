using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_animation;

public class PlayerCtrl : MonoBehaviour
{
    GameObject invenObj;
    Status_Inventory inventory;
    private Rigidbody rb;
    private Transform tr;
    private BoxCollider bc;
    public float speed = 10f;
    public float turnspeed = 60f;
    public int JumpPower;
    bool IsJumping = true;
    bool CrowdControl = false;
    Player_Anima anima;
    GameObject player;
    Player_Health player_Health;

    // Start is called before the first frame update
    void Start()
    {
        invenObj = GameObject.Find("StatusArea");
        inventory = invenObj.GetComponent<Status_Inventory>();
        player = GameObject.FindWithTag("Player");
        anima = GetComponent<Player_Anima>();
        IsJumping = false;
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player_Health = player.GetComponent<Player_Health>();
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis ("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward*v) + (Vector3.right*h);

        if (CheckHitWall(moveDir))
            moveDir = Vector3.zero;
        if (!CrowdControl&&player_Health.lief)
        {
            tr.Translate(moveDir.normalized * (speed + inventory.status[3]) * Time.deltaTime);
            tr.Rotate(Vector3.up * turnspeed * Time.deltaTime * r);
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
        moveDir = transform.TransformDirection(moveDir);  // ·ÎÄÃ º¤ÅÍ¸¦ ¿ùµå º¤ÅÍ·Î
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
    public void ccon()
    {
        CrowdControl = true;
    }
    public void ccoff()
    {
        CrowdControl = false;
    }
    public void bosscc()
    {
        CrowdControl = true;
        Invoke("ccoff", 0.5f);
    }
}
