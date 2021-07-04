using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public BossScript bs;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        bs.fighting = true;
    }
}
