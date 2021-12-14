using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private Vector2 serverPosition;
    [SerializeField]
    private int id;

    public Vector2 ServerPosition { get => serverPosition; set => serverPosition = value; }
    public int Id { get => id; set => id = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.GameRunning && GameManager.players[Client.instance.myId].Health > 0)
            SendInputToServer();
    }

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space),
        };

        Vector2 _inputDirection = Vector2.zero;

        if (_inputs[0])
            _inputDirection.y += 1;
        if (_inputs[1])
            _inputDirection.x -= 1;
        if (_inputs[2])
            _inputDirection.y -= 1;
        if (_inputs[3])
            _inputDirection.x += 1;

        ClientSend.PlayerMovement(_inputs);
        Move();
    }

    private void Move()
    {
        rb.MovePosition(new Vector2(Mathf.Lerp(transform.position.x, serverPosition.x, 0.5f), Mathf.Lerp(transform.position.y, serverPosition.y, 0.5f)));
    }
}