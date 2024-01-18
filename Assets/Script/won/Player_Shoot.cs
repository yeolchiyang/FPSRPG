using UnityEngine;

public class Player_Shoot : MonoBehaviour
{

    public RaycastHit hitInfo;
    private void Start()
    {
        
    }
    float Range = 100f;
    private void Update()
    {
       
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        hitInfo = new RaycastHit();//hit 오브젝트
        int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

        if (Physics.Raycast(ray, out hitInfo,Range, layerMask))
        {
              
        }
        
    }
}
