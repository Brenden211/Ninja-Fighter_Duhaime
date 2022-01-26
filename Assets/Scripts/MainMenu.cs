using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Main Level");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Main Level");
    }
}
