using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int id;
    [SerializeField]
    private int activeBombs;
    [SerializeField]
    private int maxBombs = 2;
    [SerializeField]
    private int bombRange = 2;

    [SerializeField]
    private string username;
    
    private bool[] inputs;
    [SerializeField]
    private bool immune;

    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth = 3;
    [SerializeField]
    private float immuneTimer;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private List<Sprite> sprites;

    public int Id { get => id; }
    public string Username { get => username; }
    public int ActiveBombs { get => activeBombs; set => activeBombs = value; }
    public int Health { get => health; }
    public int BombRange { get => bombRange; set => bombRange = value; }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;

        inputs = new bool[5];

        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer.sprite = sprites[id - 1];
    }


    private void Update()
    {
        if (health > 0)
        {
            if (inputs[4])
            {
                if (activeBombs < maxBombs)
                {
                    bool bombPlaced = GridManager.instance.PlaceBomb(transform.position, id);

                    if (bombPlaced)
                        activeBombs++;
                }
            }

            if (immune)
            {
                immuneTimer += Time.deltaTime;

                if (immuneTimer > 2f)
                {
                    immune = false;
                    immuneTimer = 0f;
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1.0f);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (health > 0)
        {
            Vector2 _inputDirection = Vector2.zero;

            if (inputs[0])
                _inputDirection.y += 1;
            if (inputs[1])
                _inputDirection.x -= 1;
            if (inputs[2])
                _inputDirection.y -= 1;
            if (inputs[3])
                _inputDirection.x += 1;

            Move(_inputDirection);
        }
    }

    private void Move(Vector2 inputDirection)
    {
        rb.MovePosition(rb.position + inputDirection * moveSpeed * Time.fixedDeltaTime);

        ServerSend.PlayerPosition(this);
    }

    public void SetInputs(bool[] _inputs)
    {
        inputs = _inputs;
    }

    public void TakeDamage(int _damage)
    {
        if (health <= 0 || immune)
            return;

        health -= _damage;

        if (health <= 0)
        {
            health = 0;

            spriteRenderer.enabled = false;
        }
        else
        {
            immune = true;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        }
        ServerSend.PlayerHealth(this);
    }
}
