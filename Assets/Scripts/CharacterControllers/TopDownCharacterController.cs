using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    public float speed = 1.0f;

    public AudioClip deathClip;

    SpriteRenderer  spriteRenderer;
    public SpriteRenderer  StickManSprite;
    new Rigidbody2D rigidbody2D;
    public bool     dead = false;


    public bool freeMovements = false;
    [Header("ANIMATION")]

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        // player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (dead || freeMovements)
            return;

        var movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rigidbody2D.MovePosition(rigidbody2D.position + movement * speed * Time.fixedDeltaTime);


        if (movement.magnitude > 0)
            StickManSprite.flipX = movement.x < 0;
        anim.SetFloat("Velocity", Mathf.Abs(movement.x));
        anim.SetFloat("VelocityY", movement.y);

    }

    public void Die()
    {
        rigidbody2D.isKinematic = true;
        dead = true;

        anim.SetTrigger("KnockOut");
        // AudioManager.PlayOnShot(deathClip);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (dead)
            return;
        
        // TODO: take hit SFX
    }
}
