using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

    public void NewGameButtonClicked()
    {
        Application.LoadLevel("main");
    }
}
