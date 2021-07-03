using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Playerator pa;
    [SerializeField]
    public Damage.Profile attack;

    private void OnCollisionEnter2D(Collision2D other)
    {
        GroundSlicer tmpGS = GroundSlicer.Instance;
        if (other.transform.tag == "Enemy")
        {
            if (tmpGS.CompareSlices(tmpGS.getSlices(other.transform.position.y), tmpGS.getSlices(pa.transform.position.y)))
            {
                EnemyController tmpP = other.gameObject.GetComponent<EnemyController>();
                attack.dir = (other.transform.position - pa.transform.position).normalized;
                tmpP.TakeDamage(attack);
            }
        }
    }
}
