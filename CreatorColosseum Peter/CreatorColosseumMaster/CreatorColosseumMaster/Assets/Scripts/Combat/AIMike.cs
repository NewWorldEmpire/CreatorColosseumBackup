using UnityEngine;
using System.Collections;

public class AIMike : MonoBehaviour {

    public GameObject levelSelect;

    public float movingSpeed;
    public float fireFreq;
    private float xPos;
    private float lastShot;

    private int randomY;
    public int yMin;
    public int yMax;
    public int xBulletOffset = 3;

    public Vector2 destination;

    public bool xReached;
    public bool yReached = true;
    public bool isDead = false;

    public Transform bulletSpawn;

    // Use this for initialization
    void Start()
    {
        xPos = transform.position.x;
        destination = new Vector2(transform.position.x, transform.position.y); //where he starts
    }

    // Update is called once per frame
    void Update()
    {
        if (!yReached)
        {
            MovePhase(destination);
            GetComponent<Animator>().SetBool("TankIdle", true);
            GetComponent<Animator>().SetBool("Shoot", false);
        }
        else
        {
            if (Time.time > lastShot + fireFreq)
            {
                ShootPhase();
                destination = CreateDestination();
            }
            else
            {
                GetComponent<Animator>().SetBool("TankIdle", true);
                GetComponent<Animator>().SetBool("Shoot", false);
            }
        }

        if (this.gameObject.GetComponent<EnemiesReceiveDamage>().hp <= 0 && !isDead)
        {
            levelSelect.GetComponent<Transitions>().levelSelect++;
            isDead = true;
        }

    }

    //----------------CreateDestination-----------------
    Vector2 CreateDestination()
    {
        if (yReached)
        {
            randomY = Random.Range(yMin, yMax);

            destination = new Vector2(xPos, randomY);
            yReached = false;
        }

        return destination;
    }

    //====================MOVING PHASE=====================
    void MovePhase(Vector2 destination)
    {
        //moving up and down towards destination
        if ((destination.y - 1) > transform.position.y)
        {
            transform.position += transform.up * movingSpeed * Time.deltaTime;
            yReached = false;
        }
        else if ((destination.y + 1) < transform.position.y)
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
        if ((destination.x) > transform.position.x)
        {
            transform.position += transform.right * movingSpeed * Time.deltaTime;
            xReached = false;
        }
        else if ((destination.x) < transform.position.x)
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
    //------------------ShootPhase()---------
    void ShootPhase()
    {
        GetComponent<Animator>().SetBool("Shoot", true);
        GetComponent<Animator>().SetBool("TankIdle", false);

        lastShot = Time.time;

        GameObject obj = EnemyFluttershyPooling.current.GetPooledObject();

        if (obj == null)
        {
            return;
        }

        obj.transform.position = bulletSpawn.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
    }
}