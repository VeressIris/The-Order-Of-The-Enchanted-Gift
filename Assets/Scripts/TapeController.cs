using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeController : MonoBehaviour
{
    bool placed = false;

    void Update()
    {
        if (!placed)
        {
            FollowMouse();
        }
        
        if (Input.GetMouseButtonDown(0) && BoxClicked())
        {
            Debug.Log("Tape placed");
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

        if (hit.collider != null && 
            (hit.collider.gameObject.layer == LayerMask.NameToLayer("ClosedBox") || 
            hit.collider.gameObject.layer == LayerMask.NameToLayer("WrappedBox")))
        {
            return true;
        }

        return false;
    }
}
