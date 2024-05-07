using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    private float inputX = 0.0f;
    private float inputY = 0.0f;
    private bool jumpPressed = false;
    private bool canJump = false;
    private bool canRun = true;
    private Rigidbody rb;

    public float moveForce = 1.0f;
    public float airSpeedMult = 1.0f;
    public float jumpPressedForce = 10.0f;
    public float maxLateralSpeed = 10.0f;
    private float init_drag;

    public Transform rig;

    private Vector3 platform_offset;
    private bool attached_to_platform = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        init_drag = rb.drag;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            jumpPressed = true;
            canRun = false;
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(rig.forward);
        Vector3 inputVector = new Vector3(inputX, 0.0f, inputY);
        //rb.MovePosition(transform.position + inputVector * Time.fixedDeltaTime * moveForce);
        transform.Rotate(0f, rig.rotation.y, 0f);
        Vector3 forceVector = inputVector * Time.fixedDeltaTime * moveForce;
        if (!canRun)
        {
            forceVector *= airSpeedMult;
        }
        rb.AddForce(forceVector, ForceMode.Impulse);

        Vector3 lateralVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (lateralVelocity.magnitude >= maxLateralSpeed)
        {
            rb.velocity = (new Vector3(lateralVelocity.normalized.x, 0f, lateralVelocity.normalized.z) * maxLateralSpeed) + new Vector3(0f, rb.velocity.y, 0f);
        }

        if (jumpPressed)
        {
            jumpPressed = false;
            canJump = false;
            Debug.Log("Jump");
            Vector3 jumpPressedVector = Vector3.up * jumpPressedForce;
            rb.velocity += jumpPressedVector;

        }

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ground" || col.gameObject.tag == "platform")
        {
            canJump = true;
            canRun = true;

        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "platform")
        {
            attached_to_platform = true;
            //platform_offset = (col.gameObject.transform.position - transform.position);
        }
        if (col.gameObject.tag == "platform_under")
        {
            rb.isKinematic = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "platform")
        {
            attached_to_platform = false;
            rb.drag = init_drag;
        }
        if (col.gameObject.tag == "platform_under")
        {
            rb.isKinematic = false;
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "platform")
        {

            if (col.gameObject.GetComponentInParent<Platform_Suspended_Script>().is_moving())
            {
                rb.drag = 100000000 * init_drag;
                Debug.Log("moving");
            }
            else
            {
                rb.drag = init_drag;
            }
        }

    }
    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "platform" && attached_to_platform)
        {
            //transform.position = (transform.position + platform_offset);
        }
    }
}
