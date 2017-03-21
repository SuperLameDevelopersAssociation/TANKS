using UnityEngine;
using System.Collections;
using TrueSync;

public class NetworkedMouseLook : TrueSyncBehaviour
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
    public Transform _camera;

    // Use this for initialization
    public override void OnSyncedStart () {
        m_CharacterTargetRot = transform.localRotation;
        m_CameraTargetRot = _camera.localRotation;
    }

    public override void OnSyncedInput()
    {
        FP yRot = Input.GetAxis("Mouse X") * XSensitivity;
        FP xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        TrueSyncInput.SetFP(4, yRot);
        TrueSyncInput.SetFP(5, xRot);
    }

    // Update is called once per frame
    public override void OnSyncedUpdate () {
        FP yRot = TrueSyncInput.GetFP(4);
        FP xRot = TrueSyncInput.GetFP(5);

        m_CharacterTargetRot *= Quaternion.Euler(0f, (float)yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-(float)xRot, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_CharacterTargetRot,
                smoothTime * (float)TrueSyncManager.DeltaTime);
            _camera.localRotation = Quaternion.Slerp(_camera.localRotation, m_CameraTargetRot,
                smoothTime * (float)TrueSyncManager.DeltaTime);
        }
        else
        {
            transform.localRotation = m_CharacterTargetRot;
            _camera.localRotation = m_CameraTargetRot;
        }
        UpdateCursorLock();
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
}
