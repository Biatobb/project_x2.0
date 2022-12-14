using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed = 1f;
    public Transform startPos;
    private bool move;

    Vector3 nextPos;
    void Start()
    {
        nextPos = startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(move)
        { 
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed*Time.deltaTime);
            
            if (transform.position==pos1.position)
            {
                nextPos=pos2.position;
                move=false;
            }
            if(transform.position==pos2.position)
            {
                nextPos = pos1.position;
                move = false;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position,pos2.position);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer==3)
        {
            collision.transform.parent = this.transform;
            Debug.Log("lift");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            collision.transform.parent = null;
            Debug.Log("lift ex");
        }
    }  
    
    public void Use()
    {
        move = true;
    }
    private void Move()
    {

    }
}


