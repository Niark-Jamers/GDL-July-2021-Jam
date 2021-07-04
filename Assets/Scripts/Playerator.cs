using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

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

    [Header("GROW")]
    public float growScaling = 0.1f;
    public float growSpeed = 2;
    float growTrueTimer = 0;
    float growdecayTimer = 1;
    float toGrow;
    float targetSize;

    [Header("ANIMATION")]

    public Animator anim;

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

    void Grow(float i, float scale)
    {
        if (scale < this.transform.localScale.x /2)
            i = i / growScaling;
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
            targetSize = targetSize * ( 1.02f * ( 1 + protein/100));
        }
        float tmp = transform.localScale.x;
        tmp = Mathf.Lerp(tmp, targetSize, Time.deltaTime * growSpeed);
        transform.localScale = new Vector3(tmp, tmp, tmp);
    }

    void DoHitStun()
    {
        anim.SetTrigger("Hurt");
        if (hitStun > 0)
        {
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
    // Update is called once per frame
    void Update()
    {
        UpdateBars();
        DoHitStun();
        DoGrow();
        powerMult = this.transform.localScale.x;
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("Punch");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("Kick");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Proteine")
        {
            protein += other.gameObject.GetComponent<Proteine>().power;
            Destroy(other.gameObject);
        }
    }
}
