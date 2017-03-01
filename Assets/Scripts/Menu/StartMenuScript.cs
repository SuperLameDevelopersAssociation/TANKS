using UnityEngine;
using UnityEditor.SceneManagement;

public class StartMenuScript : MonoBehaviour
{

    // Use this for initialization
    public void Lobby()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    // Use this for initialization
    public void ExitGame()
    {
        Application.Quit();
    }


}
