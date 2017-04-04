using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;

public class PlayerMovement : TrueSyncBehaviour
{
    public GameObject _camera;
    public int speed = 10;
    public int rotationSpeed = 150;
    public Animator wheels;
    public Renderer[] tracks;
    public Material forwardMat;
    public Material backMat;
    public bool isTank1;

    List<ScrollText> scrollText;
    private int speedStore;
    private int rotationStore;
    private bool hasAnimator;

    public override void OnSyncedStart()
    {
        tsTransform.position = new TSVector(TSRandom.Range(-50, 50), 0, TSRandom.Range(-50, 50));

        if (_camera == null)
            _camera = transform.FindChild("Camera").gameObject;

        if (TrueSyncManager.LocalPlayer == owner)
            _camera.SetActive(true);

        if (wheels == null)
        {
            Debug.Log(gameObject.name + "does not have a wheel animator");
            hasAnimator = false;
        }
        else
        {
            hasAnimator = true;
        }

        speedStore = speed;
        rotationStore = rotationSpeed;
        scrollText = new List<ScrollText>();
        for(int i = 0; i < tracks.Length; i++)
        {
            scrollText.Add(tracks[i].GetComponent<ScrollText>());
            scrollText[i].enabled = false;
        }
    }

    public override void OnSyncedInput()
    {
        FP accell = Input.GetAxisRaw("Vertical");
        FP steer = Input.GetAxisRaw("Horizontal");

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

        if (hasAnimator)
        {
            if(isTank1)
            {
                wheels.SetFloat("rotation", (float)steer);
                wheels.SetFloat("velocity", (float)accell);
                print("Rotation: " + (float)steer);
                print("velocity: " + (float)accell);

                print("AnimRotation " + wheels.GetFloat("rotation"));
                print("AnimVelocity " + wheels.GetFloat("velocity"));
            }
            else
            {
                if (accell != 0)
                    wheels.SetBool("IsMoving", true);
                else
                    wheels.SetBool("IsMoving", false);
            }
        }
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