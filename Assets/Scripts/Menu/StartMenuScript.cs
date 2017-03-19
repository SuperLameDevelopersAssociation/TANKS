using UnityEngine;

public class StartMenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject menuPanel;

    [SerializeField]
    GameObject creditsScreen;

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

    public void Credits()
    {//let the credits roll, baby!
        gameObject.SetActive(false);
        creditsScreen.SetActive(true);
        creditsScreen.GetComponent<Animator>().Play("Credits");
        creditsScreen.GetComponent<Animator>().speed = 1.0f;
    }
}
