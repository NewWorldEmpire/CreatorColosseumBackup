using UnityEngine;
using System.Collections;

public class Transitions : MonoBehaviour
{

    public GameObject map;
    public GameObject canvas;
    public GameObject player;
    public GameObject cam;

    public GameObject sceneOne;
    public GameObject sceneTwo;
    public GameObject sceneThree;
    public GameObject sceneFour;

    public GameObject mikeBoss;
    public GameObject sethBoss;
    public GameObject emilBoss;
    public GameObject finalBoss;

    public float wait;

    public bool hasDied;
    void Start ()
    {
        mikeBoss = GameObject.Find("MikeBoss");
        //sethBoss = GameObject.Find("SethBoss");
       // emilBoss = GameObject.Find("EmilBoss");
       // finalBoss = GameObject.Find("FinalBoss");
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (hasDied)
        {
            StartTransition(mikeBoss, sceneOne, sceneTwo);
        }
        if (hasDied)
        {
            StartTransition(sethBoss, sceneTwo, sceneThree);
        }
        if (hasDied)
        {
            StartTransition(emilBoss, sceneThree, sceneFour);
        }
        if (hasDied)
        {
           //Application.LoadLevel("");
        }
    }
  

    void StartTransition(GameObject boss, GameObject oldScene, GameObject newScene)
    {
        wait = Time.time;
        Debug.Log("Called");
        map.SetActive(true);
        canvas.SetActive(false);
        player.SetActive(false);
        if(Time.time > wait + 5)
        {
            oldScene.SetActive(false);
            newScene.SetActive(true);
            boss.SetActive(true);
        }
        EndTransition();
    }

  

    void EndTransition()
    {
        canvas.SetActive(true);
        player.SetActive(true);
        map.SetActive(false);
    }

    public void Died()
    {
        
        hasDied = true;
        Invoke("ChangeBool", 3);
    }
    public void ChangeBool()
        {
            hasDied = false;
        }

    }

