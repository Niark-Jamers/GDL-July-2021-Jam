using UnityEngine;
using UnityEngine.UI;

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

    [Header("ANIMATION")]

    public Animator anim;

    Rigidbody2D rb;
    [HideInInspector]
    public enum playerState { Move, attack, hitStun };
    public playerState currentState = playerState.Move;
    TopDownCharacterController tdc;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tdc = GetComponent<TopDownCharacterController>();
    }



    public void TakeDamage(Damage.Profile hit)
    {
        health -= hit.dmg;
        hitStun += hit.hitStun;
        if (hit.knockback > 0)
            rb.AddForce(hit.dir * hit.knockback, ForceMode2D.Impulse);
    }

    void updateBars()
    {
        lifeFillBar.fillAmount = health / maxHealth;
        lifeText.text = health.ToString() + "%";
        proteinFillBar.fillAmount = protein / maxProtein;
    }

    void Grow(int i)
    {

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
        updateBars();
        DoHitStun();
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
