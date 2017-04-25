using UnityEngine;
using System.Collections;

public class MenuCursor : MonoBehaviour {

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
