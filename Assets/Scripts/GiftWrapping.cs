using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        StartCoroutine(RaiseObject(0.46f));

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

    private IEnumerator Move(Vector3 targetPos, float duration, float speed)
    {
        float time = 0f;
        while (time < 0.435f)
        {
            time += Time.deltaTime;

            gameManager.objectToWrap.transform.position = Vector3.Lerp(gameManager.objectToWrap.transform.position,
                targetPos, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator RaiseObject(float duration)
    {
        StartCoroutine(Move(new Vector3(0,0,0), duration, 5.2f));
        
        yield return new WaitForSeconds(duration + 0.3f);
        StartCoroutine(Move(gameManager.objectPosition.transform.position, duration, 5.2f));
        
        Animator animator = gameManager.objectToWrap.GetComponent<Animator>();
        animator.Play("FadeOut", 0);

        yield return new WaitForSeconds(duration);
        Destroy(gameManager.objectToWrap);
    }
}
