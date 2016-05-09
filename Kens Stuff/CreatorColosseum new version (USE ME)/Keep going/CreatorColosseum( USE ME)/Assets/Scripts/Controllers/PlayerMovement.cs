using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    ///NOTE: If the public float, speed, is too high, the player may experience some serious turbulance! Player may 
    //fly through solid objects or other objects not otherwise meant to be passable.
    public Rigidbody2D player;
    public float speed = 200.0f;
    public float vertSpeed;
    public float sprint = 2;
    public int xMin = -130;
    public int xMax = 130;
    public int yMin = -50;
    public int yMax = -35;
    [HideInInspector]
    public bool isSprinting = false;
    public float stamina = 5;
    public int maxStamina = 5;
    public float staminaRecoveryRate = 0.3f;
    private float staminaRecharge = 5.0f;
    [HideInInspector]
    public float moveSpeed;


    private Vector3 targetPosition;
    private bool isMoving;

    [HideInInspector]
    public float moveX;
    [HideInInspector]
    public float moveY;

    [HideInInspector]
    public bool moveUp;
    [HideInInspector]
    public bool moveDown;
    [HideInInspector]
    public bool moveRight;
    [HideInInspector]
    public bool moveLeft;


    public GameObject self;

    //animation
    public Animator anim;

    void start()
    {
        anim = GetComponent<Animator>();

        stamina = maxStamina;
        if (isSprinting == false)
        {
            moveSpeed = speed;
        }
    }

    // Character controller - the mouse will always override the keypad. Also, this control type does not
    // apply to X and Y cordinates but X and Z coordinates. To change, switch the "forward" function to 
    // "up" and the "back" function to "down." Rotation of camera may also affect the control. 

    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), 0);

        // Only when left mouse button is not clicked, will the WSAD controls work.) 
        if (isSprinting == false)
        {
            moveSpeed = speed;
            //WSAD control
            moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            moveY = Input.GetAxis("Vertical") * vertSpeed * Time.deltaTime;
        }
        else
        {
            moveSpeed = speed * sprint;
            stamina -= 1 * Time.deltaTime;
            staminaRecharge = 0;
            //sprinting and stamina drain
            moveX = Input.GetAxis("Horizontal") * speed * sprint * Time.deltaTime;
            moveY = Input.GetAxis("Vertical") * vertSpeed * sprint * Time.deltaTime;
        }

        transform.Translate(moveX, moveY, 0);

        if (moveX < 0)
        {
            moveRight = false;
            moveLeft = true;
            moveUp = false;
            moveDown = false;
        }

        if (moveX > 0)
        {
            moveRight = true;
            moveLeft = false;
            moveUp = false;
            moveDown = false;
        }

        if (moveY > 0 || moveY < 0)
        {
            if (moveRight == true)
                anim.SetBool("WalkRight", true);
            if (moveLeft == true)
                anim.SetBool("WalkLeft", true);
        }


        if (self.GetComponent<CombatScript>().attackRate <= 0 && self.GetComponent<CombatScript>().casting == false)
        {
            if (moveX > 0)
            {
               moveRight = true;
                moveLeft = false;
                anim.SetBool("WalkRight", true);
                anim.SetBool("WalkLeft", false);
                anim.speed = 1.0f;
            }
            else
            {
                if (moveY == 0)
                    anim.SetBool("WalkRight", false);
                anim.SetBool("Right", true);
            }
            if (moveX < 0)
            {
                moveRight = false;
                moveLeft = true;
                anim.SetBool("WalkLeft", true);
                anim.SetBool("WalkRight", false);
                anim.speed = 1.0f;
            }
            else
            {
                if (moveY == 0)
                    anim.SetBool("WalkLeft", false);
                anim.SetBool("Left", true);
            }
        }

        //sprinting
        if (Input.GetKeyDown("space") && isSprinting == false && stamina > 0)
            isSprinting = true;

        if (Input.GetKeyUp("space") && isSprinting == true)
            isSprinting = false;

        if (stamina <= 0)
            isSprinting = false;

        //stamina must recharge before it can recover
        if (staminaRecharge < 5)
        {
            staminaRecharge += 1 * Time.deltaTime;
            //if staminaRecharge should happen to go above 5
            if (staminaRecharge > 5)
                staminaRecharge = 5;
        }

        //stamina recovery
        if (isSprinting == false && stamina < maxStamina && staminaRecharge == 5)
        {
            stamina += 1 * Time.deltaTime * staminaRecoveryRate;

            //if stamina should happen to go above the max value
            if (stamina > maxStamina)
                stamina = maxStamina;
        }
    }
}