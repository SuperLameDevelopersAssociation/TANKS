using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public int speed = 10;
    public int rotationSpeed = 150;
    public int flipTimer = 30;

    bool isFlipped = true;

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

        if (Vector3.Dot(transform.up, Vector3.down) < 0)
        {
            Move();
            Turn();
        }
        else
        {
            textSpeed.text = "0 km/h ";
            if (isFlipped)
            {
                isFlipped = false;
                StartCoroutine(Unflip());
            }
        }
    }

    void Move()
    {
        int conversion = Math.Abs((int)(accell * 6 * speed));

        textSpeed.text = conversion + " km/h ";

        Vector3 movement = transform.forward * accell * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void Turn()
    {
        float turn = steer * rotationSpeed * Time.deltaTime;
        Quaternion inputRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * inputRotation);
    }

    IEnumerator Unflip()
    {
        //gameObject.transform.rotation = Quaternion.Euler; 
        yield return new WaitForSeconds(flipTimer);
        isFlipped = true;
        if (Vector3.Dot(transform.up, Vector3.down) < 0)
            yield return null;
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        }
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