using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
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

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    // Update is called once per frame
    void Update()
    {
        updateBars();
    }

    
}
