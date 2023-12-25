using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftWrapping : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject boxPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddBox()
    {
        if (!gameManager.addedOpenBox)
        {
            gameManager.addedBox = Instantiate(boxPrefab, gameManager.objectPosition.position, Quaternion.identity);
            gameManager.addedOpenBox = true;
        }
        else
        {
            //replace old box with new box
            Destroy(gameManager.addedBox);
            gameManager.addedBox = Instantiate(boxPrefab, gameManager.objectPosition.position, Quaternion.identity);
        }
    }
}
