using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject MainMenu;
    private bool optionsOpen = false;
    [SerializeField] private Animator transitionAnim;

    private void Start()
    {
        Time.timeScale = 1;
        optionsOpen = false;
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void Play()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadScene(int scene)
    {
        transitionAnim.SetTrigger("start");
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(scene);
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
