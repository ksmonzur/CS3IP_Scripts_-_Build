using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Platform_Suspended_Script : MonoBehaviour
{
    private Animator anim;
    public GameObject btn_par;
    private List<Floor_Button_Script> btn_list;

    private Vector3 init_rb_pos;
    public Vector3 rb_pos;
    private Rigidbody rb;
    enum State { static_up, static_down, moving_up, moving_down, broken, fix };
    private State state = State.static_up;
    //public Component btn_script;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        init_rb_pos = rb.position;

        btn_list = new List<Floor_Button_Script>();
        Transform[] t_arr = btn_par.GetComponentsInChildren<Transform>();
        foreach (Transform t in t_arr)
        {
            Floor_Button_Script btn_script = t.gameObject.GetComponent<Floor_Button_Script>();
            if (btn_script != null)
            {
                btn_list.Add(btn_script);
            }
        }
        foreach (Floor_Button_Script btn in btn_list)
        {
            Debug.Log(btn.gameObject.name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.moving_down)
        {
            anim.SetBool("lower", true);
            anim.SetBool("raise", false);
        }
        else if (state == State.moving_up)
        {
            anim.SetBool("lower", false);
            anim.SetBool("raise", true);
        }
        else
        {
            anim.SetBool("lower", false);
            anim.SetBool("raise", false);
        }

        if (state == State.broken)
        {
            anim.SetTrigger("break");
        }
        else
        {
            anim.ResetTrigger("break");
        }
        if (state == State.fix)
        {
            anim.SetTrigger("fix");
        }
        else
        {
            anim.ResetTrigger("fix");
        }
    }
    //Update is called a fixed number of times each second
    void FixedUpdate()
    {
        foreach (Floor_Button_Script btn in btn_list)
        {
            if (btn.num_f_updates_pressed == 1)
            {
                if (btn.btn_type == Floor_Button_Script.Btn_Type.incorrect)
                {
                    if (state != State.broken)
                    {
                        if (state == State.static_down || state == State.moving_down)
                        {
                            state = State.moving_up;
                        }
                        else
                        {
                            state = State.broken;
                        }
                    }
                }
                else if (btn.btn_type == Floor_Button_Script.Btn_Type.reset)
                {
                    if (state == State.broken)
                    {
                        state = State.fix;
                    }
                    else if (state == State.static_down || state == State.moving_down)
                    {
                        state = State.moving_up;
                    }
                }
                else // correct button
                {
                    if (state == State.static_up || state == State.moving_up)
                    {

                        state = State.moving_down;
                    }
                }
            }
        }

        rb.MovePosition(init_rb_pos + rb_pos);
        Debug.Log(state);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && (state == State.static_down || state == State.moving_down))
        {
            state = State.moving_up;
        }
    }
    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Player" && state == State.moving_down)
        {
            state = State.moving_up;
        }
    }
    public void reset_anim()
    {
        //anim.SetBool("lower", false);
        //anim.SetBool("raise", false);
        if (state == State.moving_down)
        {
            state = State.static_down;
        }
        else
        {
            state = State.static_up;
        }



    }

    public bool is_moving()
    {
        if (state == State.moving_down || state == State.moving_up)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
