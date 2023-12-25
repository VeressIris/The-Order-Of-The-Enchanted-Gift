using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftWrapping : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject tapePrefab;
    [SerializeField] private Sprite matchingWrappedBoxSprite;

    public void AddBox()
    {
        if (gameManager.addedBox == null)
        {
            gameManager.addedBox = Instantiate(boxPrefab, gameManager.objectPosition.position, Quaternion.identity);
        }
        else
        {
            //replace old box with new box
            Destroy(gameManager.addedBox);
            gameManager.addedBox = Instantiate(boxPrefab, gameManager.objectPosition.position, Quaternion.identity);
        }

        if (gameManager.addedBox.tag != gameManager.objectToWrap.tag)
        {
            //DO SOMETHING
            Debug.Log("WRONG SIZE");
        }
        else
        {
            Debug.Log("RIGHT SIZE");
            gameManager.addedCorrectBox = true;
        }
    }

    public void AddDuctTape()
    {
        Instantiate(tapePrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }

    public void WrapBox()
    {
        if (gameManager.addedCorrectBox)
        {
            gameManager.addedBox.GetComponent<SpriteRenderer>().sprite = matchingWrappedBoxSprite;
        }
    }
}
