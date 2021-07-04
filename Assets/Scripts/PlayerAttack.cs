using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Playerator pa;
    [SerializeField]
    public Damage.Profile attack;
    BoxCollider2D bc;

    float ataktimer = 0.5f;
    float truetimer = 0;


    IEnumerator Stomp()
    {
        bc.enabled = true;
        yield return new WaitForSeconds(0.05f);
        bc.enabled = false;
        yield break;
    }

    private void Start() {
        bc = GetComponent<BoxCollider2D>();
        if (transform.tag == "Stomp")
            bc.enabled = false;
    }

    private void Update()
    {
        if (transform.tag == "Stomp")
        {
            truetimer += Time.deltaTime;
            if (truetimer > ataktimer)
            {
                StartCoroutine("Stomp");
                truetimer = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        float tmp = attack.dmg;
        GroundSlicer tmpGS = GroundSlicer.Instance;

        if (other.transform.tag == "Enemy")
        {
            EnemyController tmpP = other.gameObject.GetComponent<EnemyController>();

            if (gameObject.tag == "Stomp" && tmpP.Level < pa.transform.localScale.x / 2 && pa.transform.GetComponent<TopDownCharacterController>().movement.sqrMagnitude > 0)
            {
                Debug.Log("hit et stomp" + other.tag);
                attack.dmg = tmp * pa.powerMult;
                attack.dir = (other.transform.position - pa.transform.position).normalized;
                tmpP.TakeDamage(attack);
                attack.dmg = tmp;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        float tmp = attack.dmg;
        GroundSlicer tmpGS = GroundSlicer.Instance;
        //Debug.Log("hit : " + other.transform.tag);
        if (other.transform.tag == "Boss")
        {
            BossScript tmpB = other.gameObject.GetComponent<BossScript>();
            if (tmpGS.CompareSlices(tmpGS.getSlices(other.transform.position.y, tmpB.spread), tmpGS.getSlices(pa.transform.position.y, pa.spread)))
            {

                attack.dmg = tmp * pa.powerMult;
                attack.dir = (other.transform.position - pa.transform.position).normalized;
                tmpB.TakeDamage(attack);
                attack.dmg = tmp;

            }
        }
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



