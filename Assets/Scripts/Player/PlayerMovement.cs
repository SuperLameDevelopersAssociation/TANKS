using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public GameObject _camera;
    public int speed = 10;
    public int rotationSpeed = 150;
    public Animator wheels;

    private bool hasAnimator;
    float accell;
    float steer;

    void Start()
    {
        if (_camera == null)
            _camera = transform.FindChild("Camera").gameObject;

        if (isLocalPlayer)
            _camera.SetActive(true);

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

    [ClientCallback]
    void Update()
    {
        if (!isLocalPlayer)
            return;

        accell = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");

        accell *= speed * Time.deltaTime;
        steer *= rotationSpeed * Time.deltaTime;

        CmdMove(accell, steer);

        //if (hasAnimator)
        //{
        //    if (accell != 0)
        //        wheels.SetBool("IsMoving", true);
        //    else
        //        wheels.SetBool("IsMoving", false);
        //}
    }

    [Command]
    public void CmdMove(float accellVal, float steerVal)
    {
        transform.Translate(0, 0, accellVal, Space.Self);
        transform.Rotate(0, steerVal, 0);
    }
}