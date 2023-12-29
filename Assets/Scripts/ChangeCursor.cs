using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField] private Toggle checkbox;
    [SerializeField] private Texture2D customCursor;

    private void Start()
    {
        if (PlayerPrefs.GetInt("CustomCursor", 1) == 1 || !PlayerPrefs.HasKey("CustomCursor"))
        {
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    public void ModifyCursor()
    {
        if (checkbox.isOn)
        {
            Debug.Log("custom cursor");
            PlayerPrefs.SetInt("CustomCursor", 1);
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Debug.Log("default cursor");
            PlayerPrefs.SetInt("CustomCursor", 0);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
