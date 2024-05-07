using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float inputX = 0.0f;
    private float inputY = 0.0f;
    private bool jumpPressed = false;
    private bool canJump = false;
    private Rigidbody rb;

    public float moveForce = 1.0f;
    public float jumpPressedForce = 10.0f;
    public float maxLateralSpeed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        if(Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        Vector3 inputVector = new Vector3(inputX, 0.0f, inputY);
        //rb.MovePosition(transform.position + inputVector * Time.fixedDeltaTime * moveForce);
        Vector3 forceVector = inputVector * Time.fixedDeltaTime * moveForce;
        rb.AddForce(forceVector, ForceMode.Impulse);

        Vector3 lateralVelocity = new Vector3 (rb.velocity.x,0f,rb.velocity.z);
        if (lateralVelocity.magnitude >= maxLateralSpeed)
        {
            rb.velocity = ( new Vector3(lateralVelocity.normalized.x,0f,lateralVelocity.normalized.z) * maxLateralSpeed) + new Vector3(0f,rb.velocity.y,0f);
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
        if(col.gameObject.tag == "ground")
        {
            canJump = true;
        }
    }
}
