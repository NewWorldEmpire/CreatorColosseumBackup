using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour {

    // Animation purposes
    private Animator anim;

    private Vector3 Enemy;
    private Vector2 EnemyDirection;
    private int Ground;
    bool left;

    // The speed of the enemy
    public Vector2 speed = new Vector2(1, 1);

    // Store the movement
    private Vector2 movement;

    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
        Ground = 1 << 12;


        anim.SetBool("Right", true);
        anim.SetBool("Left", true);
    }

    void Update()
    {
        Enemy = GameObject.Find("Player").transform.position;

        // Retrieve axis information
        var horizontal = Enemy.x - transform.position.x;
        var vertical = Enemy.y - transform.position.y;

        EnemyDirection = new Vector2(horizontal, vertical);

        // Movement per direction
        movement = new Vector2(speed.x * horizontal, speed.y * vertical);
        if (horizontal < -.2)  //greater or less than 0 .
        {

            anim.SetBool("WalkLeft", true);
            anim.SetBool("WalkRight", false);
        }
        else if (horizontal > .2)
        {
            anim.SetBool("WalkRight", true);
            anim.SetBool("WalkLeft", false);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {

            anim.SetBool("Attack", true);
            anim.speed = 1f;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("Attack", false);

            anim.speed = 0f;
        }
    }
}