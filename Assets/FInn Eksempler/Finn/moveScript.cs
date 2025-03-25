using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveScript : MonoBehaviour
{
    public enum state { idle, walk, running}
    public state states= state.idle;

    public CharacterController controller;
    Animator animator;
    public float speed = 3f;
    public float rotSpeed =100;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Rotate( new Vector3(0, h, 0) * Time.deltaTime * 100);
        controller.Move(transform.forward * v * Time.deltaTime * speed);
        
        
        switch(states)
        {
            case state.idle:
                animator.SetInteger("state",0);
                if(h!=0||v!=0)
                {
                    states = state.walk;
                }
                break;
            case state.walk:
                animator.SetInteger("state", 1);
                if(h==0&&v==0)
                {
                    states= state.idle;
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    states = state.running;
                }
                transform.Rotate(new Vector3(0, h, 0) * Time.deltaTime * 100);
                controller.Move(transform.forward * v * Time.deltaTime * speed);

                break;
            case state.running:
                animator.SetInteger("state", 2);
                if(v==0 && h==0)
                {
                    states = state.walk;
                }
                else if(Input.GetKeyDown(KeyCode.Space))
                {
                    //states = state.jump;
                }
                transform.Rotate(new Vector3(0, h, 0) * Time.deltaTime * 100);
                controller.Move(transform.forward * v * Time.deltaTime * speed*2);

                break;
        }    
    }
}
