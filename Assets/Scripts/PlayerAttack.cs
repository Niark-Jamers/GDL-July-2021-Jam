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
        float tmp = attack.dmg;
        GroundSlicer tmpGS = GroundSlicer.Instance;
        if (other.transform.tag == "Enemy")
        {
            EnemyController tmpP = other.gameObject.GetComponent<EnemyController>();
            if (tmpGS.CompareSlices(tmpGS.getSlices(other.transform.position.y, tmpP.spread), tmpGS.getSlices(pa.transform.position.y, pa.spread)))
            {
                attack.dmg = tmp * pa.powerMult;
                attack.dir = (other.transform.position - pa.transform.position).normalized;
                tmpP.TakeDamage(attack);
                attack.dmg = tmp;
            }
        }
    }
}
