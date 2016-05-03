using UnityEngine;
using System.Collections;

public class Transitions : MonoBehaviour
{
    public GameObject map;
    public GameObject canvas;
    public GameObject player;
    public GameObject cam;

	public Sprite[] backgroundArray;
	public Sprite[] foregroundArray;

	public GameObject background;
	public GameObject foreground;

    public GameObject mikeEnemies;
    public GameObject sethEnemies;
    public GameObject emilEnemies;
    //public GameObject finalBoss;

	public GameObject mikeEnemiesList;
	public GameObject sethEnemiesList;
	public GameObject emilEnemiesList;

    private float wait;

	public int	levelSelect = 1;
	public  bool sethTransition;
	private bool emilTransition;
	private bool mikeTime = true;

    public bool hasDied;
    void Start ()
    {
		background.GetComponent<SpriteRenderer>().sprite = backgroundArray[0];
		foreground.GetComponent<SpriteRenderer>().sprite = foregroundArray[0];
		mikeEnemies.SetActive (true);
		sethEnemies.SetActive (false);
		emilEnemies.SetActive (false);
		sethTransition = true;
		emilTransition = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (levelSelect == 2 && sethTransition)
        {
			StartTransition(sethEnemies, backgroundArray[1], foregroundArray[1]);
        }
		else if (levelSelect == 3 && emilTransition)
        {
			StartTransition(emilEnemies, backgroundArray[2], foregroundArray[2]);
        }
		//else if (levelSelect == 3)
        //{
            //StartTransition(emilBoss, sceneThree, sceneFour);
        //}
    }
  

    void StartTransition(GameObject enemyList, Sprite nextBackground, Sprite nextForeground)
    {
		print (sethTransition);

		if (mikeTime) 
		{
			wait = Time.time;
			print (wait + ": wait");
			mikeTime = false;
		}

        Debug.Log("Peter is only ok");
		//print (Time.time + ": time");
        map.SetActive(true);
        canvas.SetActive(false);
        player.SetActive(false);
        
		if(Time.time > wait + 5)
        {
			background.GetComponent<SpriteRenderer>().sprite = nextBackground;
			foreground.GetComponent<SpriteRenderer>().sprite = nextForeground;
			enemyList.SetActive (true);
			EndTransition();
			sethTransition = false;
		}
    }

    void EndTransition()
    {
		canvas.SetActive (true);
		player.SetActive (true);
		map.SetActive (false);
	}

 }

