using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject MainMenu;
    private bool optionsOpen = false;

    private void Start()
    {
        optionsOpen = false;
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void OpenCloseOptions()
    {
        if (!optionsOpen)
        {
            optionsOpen = true;
            OptionsMenu.SetActive(true);
            MainMenu.SetActive(false);
            return;
        }

        optionsOpen = false;
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
}
