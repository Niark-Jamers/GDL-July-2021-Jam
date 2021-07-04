using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    Rigidbody2D rb;
    Animator animator;
    public GameObject foodPrefab;

    public float Level = 1;
    public int spread = 3;

    [Header("Health")]
    public float maxHealth = 100;
    public float health = 100;
    public Image healthFillBar;
    public bool dead = false;


    [HideInInspector]
    public enum enemyState { hover, attackMove, attacking, hitStun };

    [Header("Movement")]
    public float speed = 6f;
    public float hoverDistance = 10f;
    float baseHoverDist;
    public float hoverVariance = 1f;
    Transform playerPos;

    public enemyState currentState = enemyState.hover;
    Vector2 hoverPos;
    // Start is called before the first frame update

    Vector2 hitPosition;
    float hitStun;
    [Header("Attack")]
    public GameObject attackGO;
    public GameObject bloodFX;
    public GameObject bloodDeath;
    public GameObject bloodSplat;
    public AudioClip hitClip;
    public AudioClip deathClip;

    public float range = 2;
    float baseRange;
    public float hitStunDepletionSpeed = 1;
    public float attackModeTimer = 5f;
    public float attackTrueTimer = 0f;

    public float attackWindUp = 1f;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerPos = GameManager.Instance.playerPosition;
        attackGO.SetActive(false);
        baseHoverDist = hoverDistance;
        baseRange = range;
        SetHoverPos();
    }

    Vector3 GetSpawnPos()
    {
        return transform.GetChild(0).transform.position;
    }

    public void TakeDamage(Damage.Profile hit)
    {
        AudioManager.PlaySFX(hitClip);
        var a = GameObject.Instantiate(bloodFX, GetSpawnPos(), Quaternion.identity);
        a.GetComponent<SpriteRenderer>().flipX = playerPos.position.x > transform.position.x;
        health -= hit.dmg;
        hitStun += hit.hitStun;
        if (hit.knockback > 0)
            rb.AddForce(hit.dir * hit.knockback, ForceMode2D.Impulse);
        if (health <= 0)
        {
            Die();
            var b = GameObject.Instantiate(bloodDeath, GetSpawnPos(), Quaternion.identity);
            b.GetComponent<SpriteRenderer>().flipX = playerPos.position.x > transform.position.x;
        }
    }


    IEnumerator SpawnFood(float i = 0)
    {
        yield return new WaitForSeconds(i);
        GameObject.Instantiate(foodPrefab, GetSpawnPos(), Quaternion.identity);
        yield break;
    }

    public void Killme(float i = 0)
    {
        Destroy(this.gameObject, i);
    }

    public void Die()
    {
        if (deathClip != null)
            AudioManager.PlaySFX(deathClip);
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        dead = true;
        GameObject.Instantiate(bloodSplat, GetSpawnPos(), Quaternion.identity);
        
        if (animator != null)
            animator.SetTrigger("Death");
        StartCoroutine("SpawnFood", 1.5f);
        Killme(2);
        // AudioManager.PlayOnShot(deathClip);
    }
    // Update is called once per frame

    void DoHitStun()
    {
        if (hitStun > 0)
        {
            currentState = enemyState.hitStun;
            hitStun -= Time.deltaTime * hitStunDepletionSpeed;
            if (hitStun <= 0)
                currentState = enemyState.hover;
        }
    }

    void Scaledistance()
    {
        hoverDistance = baseHoverDist * playerPos.localScale.x;
        range = baseRange + playerPos.localScale.x / 2;
    }

    void Update()
    {
        healthFillBar.fillAmount = health / maxHealth;
        if (dead)
            return;
        Scaledistance();
        DoHitStun();
        if (currentState != enemyState.attackMove && currentState != enemyState.attacking)
            attackTrueTimer += Time.deltaTime;
        if (attackTrueTimer > attackModeTimer)
        {
            currentState = enemyState.attackMove;
            attackTrueTimer = 0;
        }
    }

    void SetHitPosition()
    {
        hitPosition = (Vector2)playerPos.position + (rb.position - (Vector2)playerPos.position).normalized * range;
        hitPosition.y = playerPos.position.y;
    }

    void SetSafeHoverPos()
    {
        int i = 1;
        if (playerPos.position.x - transform.position.x > 0)
            i = -1;
        hoverPos = (Vector2)playerPos.position + Vector2.right * i * hoverDistance;
        hoverPos += new Vector2(Random.Range(-hoverVariance, hoverVariance), Random.Range(-hoverVariance, hoverVariance));
    }

    void SetHoverPos()
    {
        hoverPos = (Vector2)playerPos.position + (rb.position - (Vector2)playerPos.position).normalized * hoverDistance;
        hoverPos += new Vector2(Random.Range(-hoverVariance, hoverVariance), Random.Range(-hoverVariance, hoverVariance));
    }

    void hoverMovement()
    {
        if (((Vector2)rb.position - hoverPos).magnitude < 0.05f)
            SetHoverPos();
        Vector2 dir = (hoverPos - rb.position).normalized;
        float roty = 0;
        if (dir.x > 0)
            roty = 180;
        transform.rotation = Quaternion.Euler(0, roty, 0);
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
    }

    IEnumerator EndAttack()
    {

        yield return new WaitForSeconds(1f);
        attackGO.SetActive(false);
        currentState = enemyState.hover;
        yield break;
    }

    void StartAttack()
    {
        attackGO.SetActive(true);
        StartCoroutine("EndAttack");
    }

    void AttackMovement()
    {
        SetHitPosition();
        Vector2 dir = (hitPosition - rb.position).normalized;
        float roty = 0;
        if (dir.x > 0)
            roty = 180;
        transform.rotation = Quaternion.Euler(0, roty, 0);
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
        if (((Vector2)rb.position - hitPosition).magnitude < 0.2f)
        {
            currentState = enemyState.attacking;
            StartAttack();
        }
    }

    private void FixedUpdate()
    {
        if (dead)
            return;
        switch (currentState)
        {
            case enemyState.hover:
                {
                    hoverMovement();
                    break;
                }
            case enemyState.attackMove:
                {
                    AttackMovement();
                    break;
                }
            default:
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.tag == "Wall")
            SetSafeHoverPos();
    }
    private void OnDrawGizmos()
    {
        if (currentState == enemyState.hover)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hoverPos, 1);
        }
        if (currentState == enemyState.attackMove)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitPosition, 1);
        }
        if (currentState == enemyState.attacking)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 1);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + (Vector3)Vector2.right * range, .3f);
        Gizmos.DrawSphere(transform.position + (Vector3)Vector2.left * range, .3f);
    }
}
