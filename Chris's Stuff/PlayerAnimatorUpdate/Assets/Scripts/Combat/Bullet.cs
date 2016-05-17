using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public GameObject _player;
    public GameObject levelSelect;

    public int movingSpeed;
    public int bulletDamage;
    public int xMin;
    private int armor;

    public Vector2 destination;
	//public BoxCollider2D _collider;

    // Use this for initialization
    void Start()
    {
        destination = new Vector2(xMin, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            MovePhase();
        }

		if (levelSelect.GetComponent<Transitions>().levelSelect > 1 && levelSelect.GetComponent<Transitions>().levelSelect < 3)
            destination = new Vector2(500, 0);
		else
			destination = new Vector2(xMin, transform.position.y);

    }

    void MovePhase()
    {
        if (destination.x < transform.position.x)
        {
            transform.position += transform.right * -movingSpeed * Time.deltaTime;
        }
        else
            this.gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D playerC)
    {
        if (playerC.gameObject.tag == "Player")
        {
            armor = _player.GetComponent<CombatScript>().armor;
            _player.GetComponent<PlayerReceivesDamage>().InitiateCBT(bulletDamage.ToString()).GetComponent<Animator>().SetTrigger("Hit"); //changed playerReceivesDamge
			this.gameObject.SetActive(false);

			if (armor < bulletDamage)
			{
				_player.GetComponent<CombatScript>().health -= (bulletDamage - armor);
			}
        }
    }
}