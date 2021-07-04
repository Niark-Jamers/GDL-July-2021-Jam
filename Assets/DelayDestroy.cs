using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public float delay = 1;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, delay);
    }

    public void DestroyNow() => Destroy(gameObject);
}
