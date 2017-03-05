using UnityEngine;
using UnityEditor.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject menuPanel;

    // Use this for initialization
    public void Lobby()
    {//this takes you to nickname
        menuPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Return()
    {//returns to main menu
        gameObject.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void ExitGame()
    {//quits the damn game
        Application.Quit();
    }


}
