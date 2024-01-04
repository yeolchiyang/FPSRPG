using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player_animation
{

    public class Player_Anima : MonoBehaviour
    {
        
        Animator animator;
        // Start is called before the first frame update
        public void Start()
        {

            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            float turnInput = Input.GetAxis("Mouse X");
            animator.SetFloat("HorizontalInput", horizontalInput);
            animator.SetFloat("VerticalInput", verticalInput);
            animator.SetFloat("TurnInput", turnInput);

        }
        public void Player_jump()
        {
            animator.SetTrigger("jump");
        }
       
        public void Player_Hit()
        {
            animator.SetTrigger("hit");
        }
        public void Player_Die()
        {
            animator.SetTrigger("die");
        }
        public void animaTriggerRe()
        {
            animator.ResetTrigger("jump");
            animator.ResetTrigger("hit");
        }

    }
}
