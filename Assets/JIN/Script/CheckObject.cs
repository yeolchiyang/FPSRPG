using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObject : MonoBehaviour
{
    [SerializeField] float range = 4f;

    [SerializeField] UnityEngine.UI.Text fieldItemText;
    [SerializeField] GameObject fieldItemSlot;

    private void Start()
    {
        fieldItemSlot.SetActive(false);
        fieldItemText.text = string.Empty;
    }

    private void Update()
    {
        CheckObj();
    }

    void CheckObj()
    {
        fieldItemSlot.SetActive(false);
        fieldItemText.text = string.Empty;

        Ray ray = new Ray(Camera.main.transform.position,
            Camera.main.transform.forward);
        RaycastHit hitInfo = new RaycastHit();

        if (Physics.Raycast(ray, out hitInfo, range))
        {
            InteractionObject interationObj = hitInfo.collider.gameObject.
                GetComponent<InteractionObject>();
            if (interationObj != null && interationObj.GetItemCount() > 0)
            {
                print(hitInfo.collider.gameObject.name);
                fieldItemSlot.SetActive(true);
                fieldItemText.text = "" + interationObj.GetItemName();

                if (Input.GetKeyDown("f"))
                {
                    interationObj.GetItem();
                }

            }
        }
    }
}
