using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Toggle showInstructionsCheckbox;

    void Start()
    {
        if (!PlayerPrefs.HasKey("ShowInstructions") || PlayerPrefs.GetInt("ShowInstructions") == 0)
        {
            gameManager.timerRunning = false;
            this.gameObject.SetActive(true);
        }
        else
        {
            gameManager.timerRunning = true;
            this.gameObject.SetActive(false);
        }
    }

    public void ChangeInstructionsPreferences()
    {
        if (showInstructionsCheckbox.isOn)
        {
            PlayerPrefs.SetInt("ShowInstructions", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ShowInstructions", 0);
        }
    }

    public void ContinueToGame()
    {
        gameManager.timerRunning = true;
        this.gameObject.SetActive(false);
    }
}
