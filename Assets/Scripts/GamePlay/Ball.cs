using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool isSelected;
    public bool isEmty
    {
        get
        {
            return spriteRenderer.sprite == null ? true : false; 
        }
    }
    public void DeletSprite()
    {
        gameObject.SetActive(false);
    }
}
