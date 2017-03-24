using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkedMouseLook : NetworkBehaviour
{
    public int XSensitivity = 2;
    public int YSensitivity = 2;
    public bool clampVerticalRotation = true;
    public int MinimumX = -90;
    public int MaximumX = 90;
    public bool smooth;
    public int smoothTime = 5;
    public bool lockCursor = true;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private bool m_cursorIsLocked = true;

    private Animator _animator;
    private float aimAngle;

    public Transform _camera;

    float xRot;
    float yRot;

    // Use this for initialization
    void Start () {
        m_CharacterTargetRot = transform.localRotation;
        m_CameraTargetRot = _camera.localRotation;

        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update () {
        UpdateCursorLock();

        if (NotSoPausedPauseMenu.isOn)
            return;

        yRot = Input.GetAxis("Mouse X") * XSensitivity;
        xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_CharacterTargetRot, smoothTime * Time.deltaTime);
            _camera.localRotation = Quaternion.Slerp(_camera.localRotation, m_CameraTargetRot, smoothTime * Time.deltaTime);
        }
        else
        {
            transform.localRotation = m_CharacterTargetRot;
            _camera.localRotation = m_CameraTargetRot;
        }
        
        SetCameraAngleInAnimator();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    void SetCameraAngleInAnimator()
    {
        if (_camera.localRotation.eulerAngles.x <= 90f)
            aimAngle = -_camera.localRotation.eulerAngles.x; //when looking down, it goes from 0 upwards
        else
            aimAngle = 360 - _camera.localRotation.eulerAngles.x; //when looking up, it goes from 360 downwards


        _animator.SetFloat("shootAngle", aimAngle);
        //print("The rotation of the camera is " + aimAngle);

    }
}
