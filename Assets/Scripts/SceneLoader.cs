using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {


    public void loadMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public void loadTutorial()
    {
        SceneManager.LoadScene("tutorial");
    }

    public void startGame()
    {
        SceneManager.LoadScene("game_scene");
    }

    public void gameOver()
    {
        SceneManager.LoadScene("game_over");
    }

    public void greatSuccess()
    {
        SceneManager.LoadScene("great_success");
    }

    public void loadSettings()
    {
        SceneManager.LoadScene("settings");
    }

    public void quitGame()
    {
        Application.Quit();
    }


}
