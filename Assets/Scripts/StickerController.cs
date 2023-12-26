using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerController : MonoBehaviour
{
    [HideInInspector] public bool placed = false;
    private AudioSource audioSrc;
    [SerializeField] private AudioClip stickerSFX;

    private void Start()
    {
        audioSrc = GameObject.Find("GameManager").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!placed)
        {
            FollowMouse();
        }

        if (Input.GetMouseButtonDown(0) && BoxClicked())
        {
            audioSrc.volume = 1f;
            audioSrc.clip = stickerSFX;
            audioSrc.Play();
            audioSrc.volume = 0.7f;
            Debug.Log("Sticker placed");
            placed = true;
        }
    }

    void FollowMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    bool BoxClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("WrappedBox"))
        {
            return true;
        }

        return false;
    }
}
