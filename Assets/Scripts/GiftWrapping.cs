using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftWrapping : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject boxPrefab;

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
}
