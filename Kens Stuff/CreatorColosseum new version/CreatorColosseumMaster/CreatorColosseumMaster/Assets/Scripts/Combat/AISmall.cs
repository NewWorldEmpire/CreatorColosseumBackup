using UnityEngine;
using System.Collections;

public class AISmall : MonoBehaviour {

    static bool oneAttackPlayer;

    static Vector2[] VectorList;
    static bool[] VectorBoolList;

    static public Collider2D _playerCollider;

    //-----validation that it is not outside clamping----
    static public float VectorMinX;
    static public float VectorMaxX;
    static public float VectorMinY;
    static public float VectorMaxY;

    //---clamping vars-----
    static public int xMin = -130;
    static public int xMax = 130;
    static public int yMin = -50;
    static public int yMax = -35;

    [HideInInspector]
    public float playerDistance;
    public float movingSpeed;

    public Vector2 destinationVector;

    //---range vars-----
    public int xRange = 1;
    public int yRange = 1;

    //---phase var----
    public int attackRange = 10;
    public int idleRange = 20;
    public int moveRange = 50;

    public float attackTimer;
    private float permentTimer;

    public Transform targetPlayer;

    public GameObject _player;
    public GameObject _camera;

    private Vector3 vectorDestination;

    public bool playerTouch = false;
    public bool forceAttack = false;
    public bool knowDestination = false;
    public bool xReached = false;
    public bool yReached = false;

    // Use this for initialization
    void Start()
    {
        _playerCollider = _player.GetComponent<Collider2D>();
        permentTimer = attackTimer;

        VectorBoolList = new bool[4]; //init the bool list

        for (int index = 0; index < VectorBoolList.Length; index++)
        {
            VectorBoolList[index] = true;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //---------------clamping--------------
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), 0);

        //---------------Resetting Attack Timer---------
        if (attackTimer > 0)
        {
            attackTimer -= 1 * Time.deltaTime;
        }

        //------calc player Distance-------------
        playerDistance = Vector3.Distance(targetPlayer.position, transform.position);

        //--------figure out what phase based up playerdistance--------
        if (playerDistance < attackRange || forceAttack)
        {
            AttackPhase();
        }
        else if (playerDistance < idleRange)
        {
            IdlePhase();
        }
        else if (playerDistance < moveRange)
        {
            MovePhase(_player.transform.position);
        }
        else
        {
            //do nothing
        }


    }

    void AttackPhase()
    {
        oneAttackPlayer = true;
        knowDestination = false;

        if (playerTouch)
        {
            if (attackTimer < 1)
            {
                if (_player.GetComponent<PlayerReceivesDamage>() != null)
                {
                    _player.GetComponent<PlayerReceivesDamage>().meleeHits++;
                    attackTimer = permentTimer;
                }
            }
        }
        else
            MovePhase(_player.transform.position);
    }

    void IdlePhase()
    {
        if (oneAttackPlayer)
        {
            CalcVectors();

            if (!knowDestination)
            {
                for (int index = 0; index < VectorList.Length; index++)
                {
                    if (VectorBoolList[index])
                    {
                        //print (VectorBoolList [index]);
                        destinationVector = VectorList[index];
                        VectorBoolList[index] = false;

                        knowDestination = true;

                        break;
                    }

                }
            }

            if (!xReached || !yReached)
            {
                MovePhase(destinationVector);
            }
            else
            {
                //do nothing
            }
        }
        else
        {
            _camera.GetComponent<CheckForEnemies>().CheckWhoClosest();

            if (forceAttack)
            {
                AttackPhase();
            }
        }
    }
    //---------------CalcVectors---------------
    static void CalcVectors()
    {
        VectorList = new Vector2[4];

        VectorMinX = _playerCollider.bounds.min.x;
        VectorMinY = _playerCollider.bounds.min.y;
        VectorMaxX = _playerCollider.bounds.max.x;
        VectorMaxY = _playerCollider.bounds.max.y;

        //----------validation that it is within clamping-----
        if (VectorMinX < xMin)
            VectorMinX = xMin;

        if (VectorMinY < yMin)
            VectorMinY = yMin;

        if (VectorMaxX > xMax)
            VectorMaxX = xMax;

        if (VectorMaxY > yMax)
            VectorMaxY = yMax;

        VectorList[0] = new Vector2((VectorMinX - 5), //how far left of the player
                                    VectorMaxY);

        VectorList[1] = new Vector2((VectorMaxX + 5), //how far right of the player
                                    VectorMaxY);

        VectorList[2] = new Vector2((VectorMinX - 5), //how far left of the player
                                    VectorMinY);

        VectorList[3] = new Vector2((VectorMaxX + 5), //how far right of the player
                                    VectorMinY);

    }
    //====================MOVING PHASE=====================
    void MovePhase(Vector2 destination)
    {
        //moving up and down towards destination
        if ((destination.y - yRange) > transform.position.y)
        {
            transform.position += transform.up * movingSpeed * Time.deltaTime;
            yReached = false;
        }
        else if ((destination.y + yRange) < transform.position.y)
        {
            transform.position += transform.up * -movingSpeed * Time.deltaTime;
            yReached = false;
        }
        else
        {
            transform.position += transform.up * 0;
            yReached = true;
        }

        //moving left and right towards destination
        if ((destination.x - xRange) > transform.position.x)
        {
            transform.position += transform.right * movingSpeed * Time.deltaTime;
            xReached = false;
        }
        else if ((destination.x + xRange) < transform.position.x)
        {
            transform.position += transform.right * -movingSpeed * Time.deltaTime;
            xReached = false;
        }
        else
        {
            transform.position += transform.right * 0;
            xReached = true;
        }
    }

    void OnCollisionStay2D(Collision2D playerC)
    {
        if (playerC.gameObject.tag == "Player")
            playerTouch = true;
    }

    void OnCollisionExit2D(Collision2D playerC)
    {
        if (playerC.gameObject.tag == "Player")
            playerTouch = false;
    }
}