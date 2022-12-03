using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField] private Transform useZone;
    [SerializeField] private float useZoneRange;
    [SerializeField] private LayerMask useLayers;



    bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius;
    private int extraJumps;
    [SerializeField] private int extraJumpsValue;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;


    private float moveInput; //если 1 двигаемся вправо, если -1 влево

    private int playerObject;
    private int platformObject;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        sr=GetComponent<SpriteRenderer>();
        extraJumps = extraJumpsValue;
        playerObject = LayerMask.NameToLayer("Player");
        platformObject = LayerMask.NameToLayer("Platform");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Use();
        

        if(Input.GetKeyDown(KeyCode.S))
        {
            Physics2D.IgnoreLayerCollision(playerObject, platformObject, true);
            Invoke("IgnoreLayerOff", 0.5f);
        }

    }

    private void Use()
    {
        if(Input.GetKeyDown(KeyCode.E))
           { 
             Collider2D[] useItems=Physics2D.OverlapCircleAll(useZone.position,useZoneRange, useLayers);
             foreach(Collider2D item in useItems)
             {
                item.GetComponent<Door>().Use();
             }
           }

    }
    private void Move()
    {
        moveInput = Input.GetAxis("Horizontal");
        if(moveInput != 0)
        { 
        sr.flipX = moveInput < 0 ? true : false;
        rb.velocity= new Vector2((speed * moveInput), rb.velocity.y);
        }
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && (extraJumps > 0 || isGrounded == true))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            extraJumps--;
        }

        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (useZone == null)
            return;
        Gizmos.DrawWireSphere(useZone.position, useZoneRange);
    }
    void IgnoreLayerOff()
    {
        Physics2D.IgnoreLayerCollision(playerObject, platformObject, false);
    }
}
