using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField]
    private int id;
    [SerializeField]
    private bool exploding;
    [SerializeField]
    private bool exploded;

    public void Initialize(int _id)
    {
        id = _id;
    }

    public void Explode()
    {
        exploding = true;

        Destroy(gameObject);
    }
}
