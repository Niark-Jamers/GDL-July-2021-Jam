using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    Rigidbody2D rb;
    Animator animator;


    [Header("Health")]
    public float maxHealth = 100;
    public float health = 100;
    public Image healthFillBar;
    public bool dead = false;


    [HideInInspector]
    public enum enemyState { hover, attack, hitStun };

    [Header("Movement")]
    public float speed = 6f;
    public float hoverDistance = 10f;
    public float hoverVariance = 1f;
    Transform playerPos;
    public enemyState currentState = enemyState.hover;
    Vector2 hoverPos;
    // Start is called before the first frame update

    [Header("Fight")]
    [HideInInspector]
    public Damage.Profile currentAttack;
    float hitStun;
    public float hitStunDepletionSpeed = 1;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerPos = GameManager.Instance.playerPosition;
        SetHoverPos();
    }

    public void TakeDamage(Damage.Profile hit)
    {
        health -= hit.dmg;
        hitStun += hit.hitStun;
        if (hit.knockback > 0)
            rb.AddForce(hit.dir * hit.knockback, ForceMode2D.Impulse);
        if (health <= 0)
            Die();
    }

    public void Killme(float i = 0)
    {
        Destroy(this.gameObject, i);
    }

    public void Die()
    {
        rb.isKinematic = true;
        dead = true;

        if (animator != null)
            animator.SetTrigger("Death");
        Killme(2);
        // AudioManager.PlayOnShot(deathClip);
    }
    // Update is called once per frame
    void Update()
    {
        healthFillBar.fillAmount = health / maxHealth;
        if (dead)
            return;
        if (hitStun > 0)
        {
            currentState = enemyState.hitStun;
            hitStun -= Time.deltaTime * hitStunDepletionSpeed;
            if (hitStun <= 0)
                currentState = enemyState.hover;
        }
    }

    void SetHoverPos()
    {
        hoverPos = (Vector2)playerPos.position + (rb.position - (Vector2)playerPos.position).normalized * hoverDistance;
        hoverPos += new Vector2(Random.Range(-hoverVariance, hoverVariance), Random.Range(-hoverVariance, hoverVariance));
    }

    private void FixedUpdate()
    {
        if (dead)
            return;
        if (((Vector2)rb.position - hoverPos).magnitude < 0.05f)
            SetHoverPos();
        Vector2 dir = (hoverPos - rb.position).normalized;
        if (currentState == enemyState.hover)
        {
            rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (dead)
            return;
        GroundSlicer tmpGS = GroundSlicer.Instance;
        if (other.transform.tag == "Player")
        {
            if (tmpGS.CompareSlices(tmpGS.getSlices(playerPos.position.y), tmpGS.getSlices(this.transform.position.y)))
            {
                Playerator tmpP = other.gameObject.GetComponent<Playerator>();
                currentAttack.dir = (other.transform.position - this.transform.position).normalized;
                tmpP.TakeDamage(currentAttack);
            }
        }
        
    }

    private void OnCollisionStay2D(Collision2D other) {
         if (other.transform.tag == "Wall")
            SetHoverPos();
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hoverPos, 1);
    }
}
