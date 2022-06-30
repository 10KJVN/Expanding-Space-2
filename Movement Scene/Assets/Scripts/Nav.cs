using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nav : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToMovement()
    {
        SceneManager.LoadScene("Movement");
    }

    public void GoToShooting()
    {
        SceneManager.LoadScene("Shooting");
    }

    public void GoToUi()
    {
        SceneManager.LoadScene("Ui");
    }
}