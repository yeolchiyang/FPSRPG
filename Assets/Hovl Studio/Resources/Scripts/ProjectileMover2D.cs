using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileMover2D : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;
    Vector3 endpoint;
    Player_Shoot Player_Shoot;
    void Start()
    {
        endpoint = Camera.main.transform.position;
        rb = GetComponent<Rigidbody>();
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            
            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject,5);

	}

    void FixedUpdate ()
    {
        
        if (speed != 0)
        {
            
           // rb.velocity = (transform.forward * speed);
            //transform.position -= gameObject.transform.right * speed * Time.deltaTime;
            //transform.rotation = Quaternion.Euler(0, 0, 0);
        }
	}

    //https ://docs.unity3d.com/ScriptReference/Rigidbody.OnCollisionEnter.html
    void OnCollisionEnter(Collision collision)
    {
        //Lock all axes movement and rotation
        rb.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        //Spawn hit effect on collision
        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            //Destroy hit effects depending on particle Duration time
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }

        //Removing trail from the projectile on cillision enter or smooth removing. Detached elements must have "AutoDestroying script"
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
            }
        }
        //Destroy projectile on collision
        Destroy(gameObject);
    }

    /*IEnumerable Moveshoot()
    {
        Vector3 direction = (endpoint - transform.position).normalized;

        // 이동할 속도 설정
        float moveSpeed = 5f;

        // 오브젝트를 목표 위치까지 일정한 속도로 이동
        while (Vector3.Distance(transform.position, endpoint) > 0.1f)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
            // 위의 코드는 한 프레임에 이동할 거리를 설정하여 부드럽게 이동하도록 합니다.
            // 원하는 정확도에 따라 이동 속도를 조절할 수 있습니다.

            // 프레임마다 업데이트를 대기합니다.
            yield return null;
        }
    }*/
}
