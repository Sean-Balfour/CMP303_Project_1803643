using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string username;
    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth = 3;
    [SerializeField]
    private bool immune;
    [SerializeField]
    private float immuneTimer;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<Sprite> sprites;

    public string Username { get => username; set => username = value; }
    public int Health { get => health; set => health = value; }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[id - 1];
    }

    private void Update()
    {
        if (immune)
        {
            immuneTimer += Time.deltaTime;

            if (immuneTimer >= 2f)
            {
                immune = false;
                immuneTimer = 0;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1.0f);
            }
        }
    }

    public void SetHealth(int _health)
    {
        health = _health;

        immune = true;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        spriteRenderer.enabled = false;
    }
}
