using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public int speed = 10;
    public int rotationSpeed = 150;
    private Animator wheels;

    public Text textSpeed;

    private bool hasAnimator;
    float accell;
    float steer;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        wheels = GetComponent<Animator>();

        if (wheels == null)
        {
            hasAnimator = false;
        }
        else
        {
            hasAnimator = true;
        }

    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (NotSoPausedPauseMenu.isOn)
            return;

        accell = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");

        int conversion = (int)(accell * 62);

        textSpeed.text = conversion + " km/h ";

        if (hasAnimator)
        {
            //getting the direction and speed of tank for the tread animations
            wheels.SetFloat("velocity", (float)accell * (speed * 10));
            wheels.SetFloat("rotation", (float)steer * (speed * 10));
        }
       
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        Move();
        Turn();
    }

    void Move()
    {
        Vector3 movement = transform.forward * accell * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void Turn()
    {
        float turn = steer * rotationSpeed * Time.deltaTime;
        Quaternion inputRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * inputRotation);
    }
}