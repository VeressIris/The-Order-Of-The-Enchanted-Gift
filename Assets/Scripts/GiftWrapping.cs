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
    [HideInInspector] public bool holdingTape = false;
    [HideInInspector] public bool holdingSticker = false;
    [SerializeField] private GameObject stickerPrefab;
    [SerializeField] private GiftWrapping tapeButtonController;
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private AudioClip boxSFX;
    [SerializeField] private AudioClip[] tapeSFX;
    [SerializeField] private AudioClip wrappedBoxSFX;
    [SerializeField] private AudioClip stickerSFX;
    [SerializeField] private AudioClip wrongSFX;

    public void AddBox()
    {
        if (gameManager.addedCorrectBox || gameManager.gameOver) 
            return;

        StartCoroutine(RaiseObject(0.46f));

        audioSrc.clip = boxSFX;

        if (gameManager.addedBox == null)
        {
            gameManager.addedBox = Instantiate(boxPrefab, gameManager.objectPosition.position, Quaternion.identity);
            audioSrc.Play();
        }
        else
        {
            //replace old box with new box
            Destroy(gameManager.addedBox);
            gameManager.addedBox = Instantiate(boxPrefab, gameManager.objectPosition.position, Quaternion.identity);

            audioSrc.Play();
            
        }
        if (gameManager.addedBox.tag != gameManager.objectToWrap.tag)
        {
            audioSrc.volume = 0.45f;
            audioSrc.clip = wrongSFX;
            audioSrc.Play();
            audioSrc.volume = 0.7f;
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
        if (gameManager.gameOver)
            return;

        if (!holdingTape && (gameManager.closedBox || gameManager.wrappedBox))
        {
            holdingTape = true;
            Instantiate(tapePrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            audioSrc.clip = tapeSFX[Random.Range(0, tapeSFX.Length)];
            audioSrc.Play();
        }
    }

    public void WrapBox()
    {
        if (gameManager.gameOver)
            return;

        if (gameManager.addedCorrectBox)
        {
            if (GetComponent<Image>().sprite == gameManager.wrappingPaperSR.sprite)
            {
                Destroy(GameObject.FindGameObjectWithTag("Tape"));
                tapeButtonController.holdingTape = false;

                gameManager.addedBox.GetComponent<SpriteRenderer>().sprite = matchingWrappedBoxSprite;
                gameManager.addedBox.layer = LayerMask.NameToLayer("WrappedBox");
                gameManager.wrappedBox = true;

                audioSrc.clip = wrappedBoxSFX;
                audioSrc.Play();
            }
            else
            {
                audioSrc.volume = 0.45f;
                audioSrc.clip = wrongSFX;
                audioSrc.Play();
                audioSrc.volume = 0.7f;
                Debug.Log("Wrong wrapping paper!");
            }
        }
    }

    private IEnumerator Move(Vector3 targetPos, float duration, float speed)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;

            if (gameManager.objectToWrap == null)
            {
                break;
            }
            gameManager.objectToWrap.transform.position = Vector3.Lerp(gameManager.objectToWrap.transform.position,
                targetPos, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator RaiseObject(float duration)
    {
        StartCoroutine(Move(new Vector3(0, 0, 0), duration, 5.2f));
        yield return new WaitForSeconds(duration + 0.3f);

        if (gameManager.addedCorrectBox)
        {
            StartCoroutine(Move(gameManager.objectPosition.transform.position, duration, 5.2f));
        
            Animator animator = gameManager.objectToWrap.GetComponent<Animator>();
            animator.Play("FadeOut", 0);
        }
    }

    public void AddSticker()
    {
        if (gameManager.gameOver)
            return;

        GameObject tapeInstance = GameObject.FindGameObjectWithTag("Tape");
        if (!holdingSticker && gameManager.wrappedBox)
        {
            if (stickerPrefab.GetComponent<SpriteRenderer>().sprite == gameManager.stickerSR.sprite)
            {
                if (tapeInstance != null && tapeInstance.GetComponent<TapeController>().placed)
                {
                    audioSrc.clip = stickerSFX;
                    audioSrc.volume = 1f;
                    audioSrc.Play();
                    audioSrc.volume = 0.7f;

                    holdingSticker = true;
                    Instantiate(stickerPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                }
            }
            else
            {
                audioSrc.volume = 0.45f;
                audioSrc.clip = wrongSFX;
                audioSrc.Play();
                audioSrc.volume = 0.7f;
                Debug.Log("Wrong sticker!");
            }
        }
    }
}
