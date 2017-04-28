using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public int speed = 10;
    public int rotationSpeed = 150;
    public Animator wheels;

    private bool hasAnimator;
    float accell;
    float steer;
    public int flipTimer = 30;
    bool isFlipped = true;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //if (wheels == null)
        //{
        //    Debug.Log(gameObject.name + "does not have a wheel animator");
        //    hasAnimator = false;
        //}
        //else
        //{
        //    hasAnimator = true;
        //}
            
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (NotSoPausedPauseMenu.isOn)
            return;

        accell = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");

        //if (hasAnimator)
        //{
        //    if (accell != 0)
        //        wheels.SetBool("IsMoving", true);
        //    else
        //        wheels.SetBool("IsMoving", false);
        //}
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
            if (isFlipped)
            {
                isFlipped = false;
                StartCoroutine(Unflip());
            }
        }
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
    
    IEnumerator Unflip()
    {
        //gameObject.transform.rotation = Quaternion.Euler; 
        yield return new WaitForSeconds(flipTimer);
        isFlipped = true;
        if (Vector3.Dot(transform.up, Vector3.down) < 0)
            yield return null;
        else
        {
            transform.position = new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z); 
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        }
    }
}