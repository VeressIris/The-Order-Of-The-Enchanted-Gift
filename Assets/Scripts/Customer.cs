using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private Sprite[] wrappingPaperOptions;
    [SerializeField] private Sprite[] stickerOptions;
    public Sprite wrappingPaperImg;
    public Sprite stickerImg;

    void Start()
    {
        wrappingPaperImg = wrappingPaperOptions[Random.Range(0, wrappingPaperOptions.Length)];
        stickerImg = stickerOptions[Random.Range(0, stickerOptions.Length)];
        //GetComponent<Animator>().enabled = false;
    }
}
