using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //���������� ����������
    private Rigidbody2D m_rigidBody;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    //���������� ��� ������������� ���������
    [SerializeField] private Transform useZone;
    [SerializeField] private float useZoneRange;
    [SerializeField] private LayerMask useLayers;

    //���������� ��� ������
    private bool m_isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius;
    private int m_extraJumps;
    [SerializeField] private int extraJumpsValue;
    [SerializeField] private float jumpForce;

    //���������� ��� ��������
    private float m_moveInput; //���� 1 ��������� ������, ���� -1 �����
    [SerializeField] private float speed;

    //��������� ��� ��������
    private int m_playerObject;
    private int m_platformObject;

    //���������� ��� �����
    public int damage = 20;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_extraJumps = extraJumpsValue;
        m_playerObject = LayerMask.NameToLayer("Player");
        m_platformObject = LayerMask.NameToLayer("Platform");
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();
        Use();
        PlatformJump();

        if (Input.GetKeyDown(KeyCode.P))
        {
            Attack();
        }
    }


   
    private void Move() //������������
    {
        m_moveInput = Input.GetAxis("Horizontal");
        if (m_moveInput != 0)
        {
            if (m_moveInput < 0)
            {
                transform.rotation = new Quaternion (transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
            }
            if(m_moveInput > 0)
            {
                transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
            }
            //m_spriteRenderer.flipX = m_moveInput < 0 ? true : false;
            m_rigidBody.velocity = new Vector2((speed * m_moveInput), m_rigidBody.velocity.y);
            m_animator.SetBool("isRun", true);
        }
        else
        {
            m_rigidBody.velocity = new Vector2(0, m_rigidBody.velocity.y);
            m_animator.SetBool("isRun", false);
        }
    }

    void Jump() // ������
    {
        m_isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        m_animator.SetBool("isGround", m_isGrounded);

        if (Input.GetButtonDown("Jump") && (m_extraJumps > 0 || m_isGrounded == true))
        {
            m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, jumpForce);
            m_animator.SetTrigger("jumpIn");
            if (m_isGrounded == false)
            {
                m_extraJumps--;
            }
        }

        if (m_isGrounded && m_rigidBody.velocity.y == 0)
        {
            m_extraJumps = extraJumpsValue;
            m_animator.SetBool("isJump", false);
        }
        if (!m_isGrounded || m_rigidBody.velocity.y != 0)
        {
            m_animator.SetBool("isJump", true);
        }
    }


    void PlatformJump()//������ � ���������
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Physics2D.IgnoreLayerCollision(m_playerObject, m_platformObject, true);
            Invoke("IgnoreLayerOff", 0.5f);
        }
    }
    void IgnoreLayerOff()//���������� ������ ����� (������ ������ ��� ���������)
    {
        Physics2D.IgnoreLayerCollision(m_playerObject, m_platformObject, false);
    }

    private void Use() //����� ������ use() � ��������� �� ������� �����������
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] useItems = Physics2D.OverlapCircleAll(useZone.position, useZoneRange, useLayers);
            foreach (Collider2D item in useItems)
            {
                item.gameObject.SendMessage("Use");
            }
        }

    }

    void Attack()
    {
        m_animator.SetTrigger("attack");

        Collider2D[]hitEnemies=Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }


    //debug
    private void OnDrawGizmosSelected() //��������� ������� (��� ������) groundCheck
    {
        if (groundCheck == null)
            return;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    }
