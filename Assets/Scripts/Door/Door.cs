using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private BoxCollider2D col;
    [SerializeField] private Sprite spriteOpen;
    [SerializeField] private Sprite spriteClose;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }
    public void Use()
    {
        if (col.isTrigger == false)
        {
            col.isTrigger = true;
            col.GetComponent<SpriteRenderer>().sprite = spriteOpen;
        }
        else
        {
            col.isTrigger = false;
            col.GetComponent<SpriteRenderer>().sprite = spriteClose;
        }
    }
}


