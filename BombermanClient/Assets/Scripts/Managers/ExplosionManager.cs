using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    private int id;

    public void Initialize(int _id)
    {
        id = _id;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
