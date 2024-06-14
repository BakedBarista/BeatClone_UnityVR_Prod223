using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwtich : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SwitchToScene(1);
    }

    public void SwitchToScene(int nextSceneIndex)
    {
        SceneManager.LoadSceneAsync(nextSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
