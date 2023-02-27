using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] GameObject credits;

    private void Start()
    {
        credits.SetActive(false);
    }
    // Start is called before the first frame update
    public void LoadScene(string sceneName)
    {
        if (sceneName == "MainMenu")
        {
            if(EnvironmentProps.Instance != null)
            {
                EnvironmentProps.Instance.AddScore(-10000000);
            }
        }
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        bool act = credits.activeSelf;
        credits.SetActive(!act);
    }
}
