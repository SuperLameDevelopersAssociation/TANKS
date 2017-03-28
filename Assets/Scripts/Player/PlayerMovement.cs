using UnityEngine;
using System.Collections;
using TrueSync;

public class PlayerMovement : TrueSyncBehaviour
{
    public GameObject _camera;
    public int speed = 10;
    public int rotationSpeed = 150;
    Animator wheels;

    private int speedStore;
    private int rotationStore;
    private bool hasAnimator;

    void Start()
    { }

    public override void OnSyncedStart()
    {
        //tsTransform.position = new TSVector(TSRandom.Range(-50, 50), 0, TSRandom.Range(-50, 50));

        if (_camera == null)
            _camera = transform.FindChild("Camera").gameObject;

        if (TrueSyncManager.LocalPlayer == owner)
            _camera.SetActive(true);

        wheels = GetComponent<Animator>();

        if (wheels == null)
            hasAnimator = false;
        else
            hasAnimator = true;

        speedStore = speed;
        rotationStore = rotationSpeed;
            
    }

    public override void OnSyncedInput()
    {
        FP accell = Input.GetAxis("Vertical");
        FP steer = Input.GetAxis("Horizontal");

        TrueSyncInput.SetFP(0, accell);
        TrueSyncInput.SetFP(1, steer);
    }

    public override void OnSyncedUpdate()
    {
        FP accell = TrueSyncInput.GetFP(0);
        FP steer = TrueSyncInput.GetFP(1);

        accell *= speed * TrueSyncManager.DeltaTime;
        steer *= rotationSpeed * TrueSyncManager.DeltaTime;

        if (hasAnimator)
        {
            //getting the direction and speed of tank for the tread animations
            wheels.SetFloat("velocity", (float)accell * (speed * 10));
            wheels.SetFloat("rotation", (float)steer * (speed * 10));
        }   

        tsTransform.Translate(0, 0, accell, Space.Self);
        tsTransform.Rotate(0, steer, 0);
    }

    public void StopMovement()
    {
        speed = 0;
        rotationSpeed = 0;
    }

    public void RestartMovement()
    {
        speed = speedStore;
        rotationSpeed = rotationStore;
    }
}