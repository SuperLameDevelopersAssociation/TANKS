using UnityEngine;
using System.Collections;
using TrueSync;

public class PlayerMovement : TrueSyncBehaviour
{
    public MouseLook mouseLook = new MouseLook();
    public GameObject _camera;
    public int speed = 10;
    public int rotationSpeed = 150;
    public Animator[] wheels;

    public override void OnSyncedStart()
    {
        tsTransform.position = new TSVector(TSRandom.Range(-50, 50), 0, TSRandom.Range(-50, 50));

        if (_camera == null)
            _camera = transform.GetComponentInChildren<Camera>().gameObject;

        if (TrueSyncManager.LocalPlayer == owner) {
            _camera.SetActive(true);
        }

        if (wheels.Length == 0)
            Debug.LogError("No wheel animators found");
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

        tsTransform.Translate(0, 0, accell, Space.Self);
        tsTransform.Rotate(0, steer, 0);

        foreach (Animator anim in wheels)
        {
            if (accell != 0)
                anim.SetBool("IsMoving", true);
            else
                anim.SetBool("IsMoving", false);
        }
    }
}