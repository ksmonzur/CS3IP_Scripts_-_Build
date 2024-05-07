using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_Button_Script : MonoBehaviour
{
    public string player_tag;
    public bool is_pressed = false;
    public int num_f_updates_pressed = 0;
    private Texture released_tex;
    public Texture pressed_tex;
    public Renderer rend;
    private Animator anim;

    public enum Btn_Type { correct, incorrect, reset };
    public Btn_Type btn_type;
    private AudioSource audio;
    public AudioClip press_clip;
    public AudioClip release_clip;
    // Start is called before the first frame update
    void Start()
    {
        //rend = GetComponentInChildren<Renderer>();
        anim = gameObject.GetComponent<Animator>();
        released_tex = rend.material.mainTexture;
        audio = gameObject.GetComponent<AudioSource>();
        audio.clip = press_clip;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_pressed)
        {
            rend.material.SetTexture("_MainTex", pressed_tex);
            anim.SetBool("is_pressed", true);
        }
        else
        {
            rend.material.SetTexture("_MainTex", released_tex);
            anim.SetBool("is_pressed", false);
        }
        //Debug.Log(is_pressed);
        /*string name = gameObject.name;
        string p = is_pressed.ToString();
        Debug.Log(name + ", " + p);*/
    }
    void FixedUpdate()
    {
        if (is_pressed)
        {
            num_f_updates_pressed++;
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == player_tag)
        {
            is_pressed = true;
            audio.clip = press_clip;
            audio.pitch = 1.0f;
            audio.pitch = Random.Range(0.9f, 1.1f);
            audio.Play();
        }

    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == player_tag)
        {
            is_pressed = false;
            num_f_updates_pressed = 0;
            audio.clip = release_clip;
            audio.pitch = 1.0f;
            audio.pitch = Random.Range(0.9f, 1.1f);
            audio.Play();

        }

    }
}
