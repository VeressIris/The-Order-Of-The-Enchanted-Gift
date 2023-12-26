using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    //called inside of animations to destroy the game object after it's done animating
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
