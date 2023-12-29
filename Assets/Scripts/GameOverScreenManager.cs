using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreenManager : MonoBehaviour
{
    [SerializeField] private Animator transitionAnim;

    public void Replay()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void QuitToMenu()
    {
        StartCoroutine(LoadScene(0));
    }

    IEnumerator LoadScene(int scene)
    {
        Time.timeScale = 1; // so that the animation plays if the game is paused
        transitionAnim.SetTrigger("start");
        yield return new WaitForSeconds(0.45f);
        SceneManager.LoadScene(scene);
    }
}
