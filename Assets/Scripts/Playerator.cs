using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Collections;

public class Playerator : MonoBehaviour
{
    [Header("LIFE")]
    public float maxHealth = 100;
    public float health = 100;
    public UnityEngine.UI.Image lifeFillBar;
    public Text lifeText;

    [Header("PROTEIN")]
    public float protein = 0;
    public float maxProtein = 100;
    public float proteinStock;
    public UnityEngine.UI.Image proteinFillBar;


    [Header("FIGHT")]

    float hitStun;
    public float hitStunDepletionSpeed = 1;
    public float powerMult;
    bool isAttacking;

    [Header("GROW")]
    public float growScaling = 0.1f;
    public float growSpeed = 2;
    float growTrueTimer = 0;
    float growdecayTimer = 0.5f;
    float toGrow;
    float targetSize;

    public int spread;
    [Header("ANIMATION")]

    public Animator anim;
    public GameObject punchHitBox;
    public GameObject kickHitBox;

    Rigidbody2D rb;
    [HideInInspector]
    public enum playerState { Move, attack, hitStun };
    public playerState currentState = playerState.Move;
    TopDownCharacterController tdc;
    CinemachineImpulseSource impulseSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tdc = GetComponent<TopDownCharacterController>();
        targetSize = transform.localScale.x;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void TakeDamage(Damage.Profile hit)
    {
        impulseSource.GenerateImpulse(1);
        health -= hit.dmg;
        hitStun += hit.hitStun;
        if (hit.knockback > 0)
            rb.AddForce(hit.dir * hit.knockback, ForceMode2D.Impulse);
    }

    void UpdateBars()
    {
        lifeFillBar.fillAmount = health / maxHealth;
        lifeText.text = health.ToString() + "%";
        proteinFillBar.fillAmount = protein / maxProtein;
    }

    public void Grow(float i, float scale = 10000)
    {
        if (scale < this.transform.localScale.x /2)
            i = i * growScaling;
        toGrow += i;
    }

    void DoGrow()
    {
        
        growTrueTimer += Time.deltaTime;
        if (growTrueTimer > growdecayTimer && toGrow > 0)
        {
            growTrueTimer = 0;
            toGrow--;
            protein--;
            if (protein < 0)
                protein = 0;
            float growPow = ( 1.01f * ( 1 + (protein/maxProtein) / 10));
           // Debug.Log(growPow);
            targetSize = targetSize * growPow;
        }
        float tmp = transform.localScale.x;
        tmp = Mathf.Lerp(tmp, targetSize, Time.deltaTime * growSpeed);
        transform.localScale = new Vector3(tmp, tmp, tmp);
    }

    void DoHitStun()
    {
        if (hitStun > 0)
        {
            anim.SetTrigger("Hurt");
            currentState = playerState.hitStun;
            tdc.freeMovements = true;
            hitStun -= Time.deltaTime * hitStunDepletionSpeed;
            if (hitStun <= 0)
            {
                currentState = playerState.Move;
                tdc.freeMovements = false;
            }
        }
    }

    void Update()
    {
        UpdateBars();
        DoHitStun();
        DoGrow();
        powerMult = this.transform.localScale.x;
        spread = (int)(powerMult + 3);
        
        if (!isAttacking && Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(Punch());
        }
        if (!isAttacking && Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(Kick());
        }
    }

    IEnumerator Kick()
    {
        anim.SetTrigger("Kick");
        isAttacking = true;
        yield return new WaitForSeconds(0.20f);
        kickHitBox.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        kickHitBox.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        isAttacking = false;
    }
    IEnumerator Punch()
    {
        anim.SetTrigger("Punch");
        isAttacking = true;
        yield return new WaitForSeconds(0.10f);
        punchHitBox.SetActive(true);
        yield return new WaitForSeconds(0.10f);
        punchHitBox.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Proteine")
        {
            protein += other.gameObject.GetComponent<Proteine>().power;
            if (protein > 100)
                protein = 100;
            Destroy(other.gameObject);
        }
        if (other.tag == "Food")
        {
            Grow(other.gameObject.GetComponent<Food>().power);
            Destroy(other.gameObject);
        }
    }
}
