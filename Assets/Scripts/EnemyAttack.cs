using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public EnemyController eC;
    [SerializeField]
    public Damage.Profile attack;

    private void OnCollisionEnter2D(Collision2D other)
    {
        GroundSlicer tmpGS = GroundSlicer.Instance;
        if (other.transform.tag == "Player")
        {
            if (tmpGS.CompareSlices(tmpGS.getSlices(other.transform.position.y), tmpGS.getSlices(eC.transform.position.y)))
            {
                Playerator tmpP = other.gameObject.GetComponent<Playerator>();
                attack.dir = (other.transform.position - eC.transform.position).normalized;
                tmpP.TakeDamage(attack);
            }
        }
    }
}
