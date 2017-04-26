using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public int speed = 10;
    public int rotationSpeed = 150;
    public Animator wheels;

    public Text textSpeed;

    private bool hasAnimator;
    float accell;
    float steer;

    int m_speed;
    int m_rotationSpeed;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_speed = speed;
        m_rotationSpeed = rotationSpeed;

       // wheels = GetComponent<Animator>();

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

        int conversion = (int)(accell * 6 * speed);

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

    public void MultiplySpeed(int multiplyValue)
    {
        speed = speed * multiplyValue;
        rotationSpeed = rotationSpeed * multiplyValue;
    }

    public void ResetSpeed()
    {
        speed = m_speed;
        rotationSpeed = m_rotationSpeed;
    }
}