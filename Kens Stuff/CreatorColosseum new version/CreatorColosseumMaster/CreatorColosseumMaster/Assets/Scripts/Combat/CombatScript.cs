﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatScript : MonoBehaviour 
{
    public GameObject self;
    public MouseScript _mouse;
    public Transform playerPOS;
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public float normalDamage = 7;
    public float rangeDamage = 3;
    [HideInInspector]
    public float playerDamage = 3;
    //[HideInInspector]
    public float iceDamage = 0.02f;
    public float lightDamage = 200.0f;
    public float attackSpeed = 5.0f;
    public int defense;
    public int armor;
    public float dexterity; // chance of hitting
    public float meleeRange = 0.8f;
    public float meleeAdjustment = 0.5f;
    public int maxHealth = 65;
    public float health;
    [Range(0.03f, 0.08f)]
    public float criticalChance;
    [Range(2, 5)]
    public int criticalDamage;
    public float chargeMultiplier = 10.0f;
    [HideInInspector]
    public float chargeDistance;
    public bool melee = true;
    public Rigidbody2D projectile;
    public Rigidbody2D flamePrefab;
    [HideInInspector]
    public Transform target;
    public GameObject smokeChild;
    [HideInInspector]
    public float chargeShot;
    public Color32 startColor;
    public Color32 endColor;
    public Image energyBar;
    public GameObject energy;
    public GameObject restorationPrefab;
    //***calculators**
    float calculator;
    float calculator2;
    float calculator3;
    float calculator4;
    //[HideInInspector]
    public int spells = 0;
    public GameObject shieldChild;
    public int shield;
    public int healthRestore = 25;
    [HideInInspector]
    public bool casting = false;

    //**spellcooldowns
    public float shieldCoolDown;
    public float restoreCoolDown;
    public float iceCoolDown;
    public float lightCoolDown;

    //**Spell Timers**
    float shieldTimer;
    float restoreTimer;
    float iceTimer;

    //Keeps track of the cooldown length
    float shieldCoolDownLength;
    float iceCoolDownLength;
    float lightCoolDownLength;
    float restoreCoolDownLength;

    //**image cooldowns**
    public Image CoolDownImageShield;
    public Image CoolDownImageRestore;
    public Image CoolDownImageIce;
    public Image CoolDownImageLight;

    [HideInInspector]
    public int splash;
    [HideInInspector]
    public float attackRate;  //rate of attack

    //animation
    public Animator anim;

    //**AUDIOS SOURCES**
    [HideInInspector]
    public AudioSource au_bow1;
    [HideInInspector]
    public AudioSource au_arrow1;
    [HideInInspector]
    public AudioSource au_arrow2;
    [HideInInspector]
    public AudioSource au_swing1;
    [HideInInspector]
    public AudioSource au_flame1;
    [HideInInspector]
    public AudioSource au_flame2;
    [HideInInspector]
    public AudioSource au_heal;
    [HideInInspector]
    public AudioSource au_light;

    void Awake()
    {
        health = maxHealth;
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        shieldCoolDownLength = shieldCoolDown;
        shieldCoolDown = 0;
        iceCoolDownLength = iceCoolDown;
        iceCoolDown = 0;
        lightCoolDownLength = lightCoolDown;
        lightCoolDown = 0;
        restoreCoolDownLength = restoreCoolDown;
        restoreCoolDown = 0;

        au_bow1 = gameObject.AddComponent<AudioSource>();
        AudioClip bow1;

        // Resources must be in any folder named Resources.  load as type and cast as type because Unity returns Object by default.
        bow1 = (AudioClip)Resources.Load("Audio/Combat Sounds/Bow", typeof(AudioClip));
        au_bow1.clip = bow1;

        au_arrow1 = gameObject.AddComponent<AudioSource>();
        AudioClip arrow1;

        // Resources must be in any folder named Resources.  load as type and cast as type because Unity returns Object by default.
        arrow1 = (AudioClip)Resources.Load("Audio/Combat Sounds/Arrow 6", typeof(AudioClip));
        au_arrow1.clip = arrow1;

        au_arrow2 = gameObject.AddComponent<AudioSource>();
        AudioClip arrow2;

        // Resources must be in any folder named Resources.  load as type and cast as type because Unity returns Object by default.
        arrow2 = (AudioClip)Resources.Load("Audio/Combat Sounds/Arrow 7", typeof(AudioClip));
        au_arrow2.clip = arrow2;

        au_swing1 = gameObject.AddComponent<AudioSource>();
        AudioClip swing1;

        // Resources must be in any folder named Resources.  load as type and cast as type because Unity returns Object by default.
        swing1 = (AudioClip)Resources.Load("Audio/Combat Sounds/Sword Swish 1", typeof(AudioClip));
        au_swing1.clip = swing1;

        au_flame1 = gameObject.AddComponent<AudioSource>();
        AudioClip flame1;

        flame1 = (AudioClip)Resources.Load("Audio/Spells/magicFlamethrowerSoundEffect", typeof(AudioClip));
        au_flame1.clip = flame1;

        au_flame2 = gameObject.AddComponent<AudioSource>();
        AudioClip flame2;

        flame2 = (AudioClip)Resources.Load("Audio/Spells/magicFlamethrowerSoundEffectTail", typeof(AudioClip));
        au_flame2.clip = flame2;

        au_heal = gameObject.AddComponent<AudioSource>();
        AudioClip heal;

        heal = (AudioClip)Resources.Load("Audio/Spells/magicHealingSpellSoundEffect", typeof(AudioClip));
        au_heal.clip = heal;

        au_light = gameObject.AddComponent<AudioSource>();
        AudioClip light;

        light = (AudioClip)Resources.Load("Audio/Spells/magicLightningSoundEffect", typeof(AudioClip));
        au_light.clip = light;
    }

    // Update is called once per frame
    void Update()
    {
        //turn casting for animations off
        if (casting == true && restoreTimer <= 0)
        {
            restoreTimer = 0;
            casting = false;
            anim.SetBool("PowerUp", false);
        }

        //switching from melee to range
        if (Input.GetKeyUp(KeyCode.Q) && chargeShot <= 0)
        {
            if (melee == false)
            {
                melee = true;
            }
            //switching from range to melee
            else
            {
                melee = false;
            }
        }

        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);
            Application.LoadLevel("GameOverScreen");
            //play animation
            //play sound
        }
        if (health > maxHealth)
            health = maxHealth;

        //setting the scale of objects to range of melee weapon
        up.transform.localScale = new Vector3(0, meleeRange, 0);
        up.transform.localPosition = new Vector3(0, meleeAdjustment, 0);
        down.transform.localScale = new Vector3(0, -meleeRange, 0);
        down.transform.localPosition = new Vector3(0, -meleeAdjustment, 0);
        left.transform.localScale = new Vector3(-meleeRange, 0, 0);
        left.transform.localPosition = new Vector3(-meleeAdjustment, 0, 0);
        right.transform.localScale = new Vector3(meleeRange, 0, 0);
        right.transform.localPosition = new Vector3(meleeAdjustment, 0, 0);

        if (criticalChance > 0.08f)
            criticalChance = 0.08f;

        if (Input.GetMouseButtonDown(0) && attackRate == 0 && restoreTimer <= 0 && iceTimer <= 0) //left click
        {
            if (melee == true)
            {
                //attack whie sprint is not active (normal attack)
                if (!self.GetComponent<PlayerMovement>().isSprinting)
                {
                    playerDamage = normalDamage;
                    splash = 1;
                    attackRate = 5;

                    //the value of this variable,splash, determines how many foes can be hit within a single attack
                    //since it's currently set to 1, only one foe can be hit at a time.
                    //certain spells, such as a multi attack, will require this variable to increase.

                    au_swing1.Play();
                    //attack animation
                    if (self.GetComponent<PlayerMovement>().moveRight == true)
                    {
                        self.GetComponent<PlayerMovement>().anim.SetBool("Attack", true);
                    }

                    if (self.GetComponent<PlayerMovement>().moveLeft == true)
                    {
                        self.GetComponent<PlayerMovement>().anim.SetBool("Attack", true);
                    }
                }

                //attack during sprint (Dash attack)
                if ((self.GetComponent<PlayerMovement>().isSprinting))// && (self.GetComponent<PlayerMovement>().moveX != 0 || self.GetComponent<PlayerMovement>().moveY != 0))
                {
                    chargeDistance = 1.2f;
                    Vector3 playerPOS = self.transform.position;
                    smokeChild.transform.position = playerPOS;
                    playerDamage = normalDamage * chargeMultiplier;
                    splash = 5;
                    self.GetComponent<PlayerMovement>().moveSpeed = self.GetComponent<PlayerMovement>().moveSpeed * 25;

                    if(self.GetComponent<PlayerMovement>().stamina >= 4)
                    {
                        self.GetComponent<PlayerMovement>().stamina = self.GetComponent<PlayerMovement>().stamina - 4;
                    }
                    
                    smokeChild.SetActive(true);
                }
            }
        }

        //charging the bow
        if (Input.GetMouseButton(0) && melee == false && attackRate == 0 && restoreTimer <= 0 && iceTimer <= 0)
        {
            anim.SetBool("AttackSpell", true);

            self.GetComponent<PlayerMovement>().moveSpeed = 0;
            if (chargeShot <= 0)
            {
                au_bow1.Play();
            }

            energy.SetActive(true);
            if (chargeShot < 1.0f)
                chargeShot += 1 * Time.deltaTime;
            if (chargeShot > 1.0f)
                chargeShot = 1.0f;
            playerDamage = rangeDamage * chargeShot * 2;
            calculator = chargeShot / 1.0f;
            SetEnergy(calculator);

            if (!target)
                target = GameObject.FindWithTag("Mouse").transform;

            Vector3 v3Pos;
            float fAngle;

            v3Pos = Camera.main.WorldToScreenPoint(target.transform.position);
            v3Pos = Input.mousePosition - v3Pos;
            fAngle = Mathf.Atan2(v3Pos.y, v3Pos.x) * Mathf.Rad2Deg;
            if (fAngle < 0.0f) fAngle += 360.0f;

            //detecting arrow direction with the variable direction
            if (fAngle <= 135.0F && fAngle > 45.0F)
            {
                self.GetComponent<PlayerMovement>().moveUp = false;
                self.GetComponent<PlayerMovement>().moveDown = false;
                self.GetComponent<PlayerMovement>().moveRight = false;
                self.GetComponent<PlayerMovement>().moveLeft = false;

                down.SetActive(false);
                left.SetActive(false);
                right.SetActive(false);
                up.SetActive(true);
            }

            if (fAngle <= 45.0F || fAngle > 315.0F)
            {
                self.GetComponent<PlayerMovement>().moveRight = false;
                self.GetComponent<PlayerMovement>().moveDown = false;
                self.GetComponent<PlayerMovement>().moveUp = false;
                self.GetComponent<PlayerMovement>().moveLeft = false;

                up.SetActive(false);
                down.SetActive(false);
                left.SetActive(false);
                right.SetActive(true);
            }

            if (fAngle <= 225.0F && fAngle > 135.0F)
            {
                self.GetComponent<PlayerMovement>().moveLeft = false;
                self.GetComponent<PlayerMovement>().moveDown = false;
                self.GetComponent<PlayerMovement>().moveRight = false;
                self.GetComponent<PlayerMovement>().moveUp = false;

                up.SetActive(false);
                down.SetActive(false);
                right.SetActive(false);
                left.SetActive(true);
            }

            if (fAngle <= 315.0F && fAngle > 225.0F)
            {
                self.GetComponent<PlayerMovement>().moveDown = false;
                self.GetComponent<PlayerMovement>().moveUp = false;
                self.GetComponent<PlayerMovement>().moveRight = false;
                self.GetComponent<PlayerMovement>().moveLeft = false;

                up.SetActive(false);
                left.SetActive(false);
                right.SetActive(false);
                down.SetActive(true);
            }
        }

        //using arrows
        if (Input.GetMouseButtonUp(0) && melee == false && attackRate == 0 && restoreTimer <= 0 && iceTimer <= 0)
        {
            anim.SetBool("AttackSpell", false);

            au_bow1.Stop();
            au_arrow1.Play();
            au_arrow2.Play();

            energy.SetActive(false);
            attackRate = chargeShot * 3;
            if (attackRate <= 3)
                attackRate = 3;

            if (!target)
                target = GameObject.FindWithTag("Mouse").transform;

            Vector3 v3Pos;
            float fAngle;

            v3Pos = Camera.main.WorldToScreenPoint(target.transform.position);
            v3Pos = Input.mousePosition - v3Pos;
            fAngle = Mathf.Atan2(v3Pos.y, v3Pos.x) * Mathf.Rad2Deg;
            if (fAngle < 0.0f) fAngle += 360.0f;

            //instantiate projectile
            Rigidbody2D clone;
            clone = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody2D;

            //detecting arrow direction with the variable direction
            if (fAngle <= 135.0F && fAngle > 45.0F)
            {
                //print ("up");
                self.GetComponent<PlayerMovement>().moveUp = true;
                self.GetComponent<PlayerMovement>().moveDown = false;
                self.GetComponent<PlayerMovement>().moveRight = false;
                self.GetComponent<PlayerMovement>().moveLeft = false;

                clone.velocity = (GameObject.Find("Mouse").transform.position - transform.position).normalized * Random.Range(15, 20);
                down.SetActive(false);
                left.SetActive(false);
                right.SetActive(false);
                up.SetActive(true);
            }

            if (fAngle <= 45.0F || fAngle > 315.0F)
            {
                //print ("right");
                self.GetComponent<PlayerMovement>().moveRight = false;
                self.GetComponent<PlayerMovement>().moveDown = false;
                self.GetComponent<PlayerMovement>().moveUp = false;
                self.GetComponent<PlayerMovement>().moveLeft = false;

                clone.velocity = (GameObject.Find("Mouse").transform.position - transform.position).normalized * Random.Range(25, 30);
                up.SetActive(false);
                down.SetActive(false);
                left.SetActive(false);
                right.SetActive(true);
            }

            if (fAngle <= 225.0F && fAngle > 135.0F)
            {
                //print ("left");
                self.GetComponent<PlayerMovement>().moveLeft = false;
                self.GetComponent<PlayerMovement>().moveDown = false;
                self.GetComponent<PlayerMovement>().moveRight = false;
                self.GetComponent<PlayerMovement>().moveUp = false;

                clone.velocity = (GameObject.Find("Mouse").transform.position - transform.position).normalized * Random.Range(25, 30);
                up.SetActive(false);
                down.SetActive(false);
                right.SetActive(false);
                left.SetActive(true);
            }

            if (fAngle <= 315.0F && fAngle > 225.0F)
            {
                //print ("down");
                self.GetComponent<PlayerMovement>().moveDown = false;
                self.GetComponent<PlayerMovement>().moveUp = false;
                self.GetComponent<PlayerMovement>().moveRight = false;
                self.GetComponent<PlayerMovement>().moveLeft = false;

                clone.velocity = (GameObject.Find("Mouse").transform.position - transform.position).normalized * Random.Range(15, 20);
                up.SetActive(false);
                left.SetActive(false);
                right.SetActive(false);
                down.SetActive(true);
            }
        }

        //attack speed
        if (attackRate > 0)
        {
            attackRate -= attackSpeed * Time.deltaTime;
            //prevents moving during attack
            self.GetComponent<PlayerMovement>().moveSpeed = 0;
        }

        if (attackRate == 0)
            self.GetComponent<PlayerMovement>().anim.SetBool("Attack", false);

        if (attackRate < 0)
            attackRate = 0;
        if (attackSpeed > 50)
            attackSpeed = 50;

        if (defense < 1)
            defense = 1;

        //charge recovery
        if (chargeDistance > 1)
            chargeDistance -= chargeDistance * Time.deltaTime;

        if (chargeDistance > 0 && chargeDistance <= 1)
        {
            attackRate = 10;
            chargeDistance = 0;
        }

        //*******MAGIC SPELLS***********

        //casting ice  (icestorm)
        if (iceTimer > 0)
            iceTimer -= 5 * Time.deltaTime;

        if (iceTimer <= 0 && iceCoolDown > 0)
        {
            iceCoolDown -= 1 * Time.deltaTime;
            calculator4 = iceCoolDown / iceCoolDownLength;
            CoolDownice(calculator4);
        }

        if (iceTimer < 0)
        {
            iceTimer = 0;
            anim.SetBool("AttackSpell", false);
        }

        if (!Input.GetMouseButton(1) && au_flame1.isPlaying)
        {
            au_flame1.Stop();
            au_flame2.Play();
        }

        if (Input.GetMouseButton(1) && spells == 0 && iceCoolDown < iceCoolDownLength && chargeShot <= 0 && attackRate <= 0)  //right click
        {
            anim.SetBool("AttackSpell", true);

            if (iceCoolDown < iceCoolDownLength)
                iceCoolDown += 2 * Time.deltaTime;
            calculator4 = iceCoolDown / iceCoolDownLength;
            CoolDownice(calculator4);
            iceTimer = 1;

            //prevent player from moving
            self.GetComponent<PlayerMovement>().moveSpeed = 0;

            if (!target)
                target = GameObject.FindWithTag("Mouse").transform;

            Vector3 v3Pos;
            float fAngle;

            v3Pos = Camera.main.WorldToScreenPoint(target.transform.position);
            v3Pos = Input.mousePosition - v3Pos;
            fAngle = Mathf.Atan2(v3Pos.y, v3Pos.x) * Mathf.Rad2Deg;

            Rigidbody2D clone;
            clone = Instantiate(flamePrefab, transform.position, transform.rotation) as Rigidbody2D;

            if (fAngle < 0.0f)
                fAngle += 360.0f;

            //playing the sound effect
            if (!au_flame1.isPlaying)
                au_flame1.Play();

            casting = true;

            //flame goes in the direction of the mouse
            if (fAngle <= 135.0F && fAngle > 45.0F)
            {
                //up
                self.GetComponent<PlayerMovement>().moveUp = true;
                self.GetComponent<PlayerMovement>().moveDown = false;
                self.GetComponent<PlayerMovement>().moveRight = false;
                self.GetComponent<PlayerMovement>().moveLeft = false;

                clone.velocity = (GameObject.Find("Mouse").transform.position - transform.position).normalized * Random.Range(25, 30);
                down.SetActive(false);
                left.SetActive(false);
                right.SetActive(false);
                up.SetActive(true);
            }

            if (fAngle <= 45.0F || fAngle > 315.0F)
            {
                //right
                self.GetComponent<PlayerMovement>().moveRight = true;
                self.GetComponent<PlayerMovement>().moveDown = false;
                self.GetComponent<PlayerMovement>().moveUp = false;
                self.GetComponent<PlayerMovement>().moveLeft = false;

                clone.velocity = (GameObject.Find("Mouse").transform.position - transform.position).normalized * Random.Range(25, 30);
                up.SetActive(false);
                down.SetActive(false);
                left.SetActive(false);
                right.SetActive(true);
            }

            if (fAngle <= 225.0F && fAngle > 135.0F)
            {
                //left
                self.GetComponent<PlayerMovement>().moveLeft = true;
                self.GetComponent<PlayerMovement>().moveDown = false;
                self.GetComponent<PlayerMovement>().moveRight = false;
                self.GetComponent<PlayerMovement>().moveUp = false;

                clone.velocity = (GameObject.Find("Mouse").transform.position - transform.position).normalized * Random.Range(25, 30);
                up.SetActive(false);
                down.SetActive(false);
                right.SetActive(false);
                left.SetActive(true);
            }

            if (fAngle <= 315.0F && fAngle > 225.0F)
            {
                //down
                self.GetComponent<PlayerMovement>().moveDown = true;
                self.GetComponent<PlayerMovement>().moveUp = false;
                self.GetComponent<PlayerMovement>().moveRight = false;
                self.GetComponent<PlayerMovement>().moveLeft = false;

                clone.velocity = (GameObject.Find("Mouse").transform.position - transform.position).normalized * Random.Range(25, 30);
                up.SetActive(false);
                left.SetActive(false);
                right.SetActive(false);
                down.SetActive(true);
            }
        }

        //Restoration spell (Revivify)
        if (Input.GetMouseButtonDown(1) && spells == 1 && restoreCoolDown <= 0 && chargeShot <= 0 && attackRate <= 0)   //right click
        {
            casting = true;
            anim.SetBool("PowerUp", true);

            au_heal.Play();

            GameObject childObject = Instantiate(restorationPrefab, transform.position, transform.rotation) as GameObject;
            childObject.transform.parent = gameObject.transform;

            health += healthRestore;
            if (health > maxHealth)
                health = maxHealth;
            restoreTimer = 3;
            restoreCoolDown = restoreCoolDownLength;
        }

        if (restoreTimer > 0)
        {
            casting = true;
            restoreTimer -= 1 * Time.deltaTime;
            //prevent player from moving
            self.GetComponent<PlayerMovement>().moveSpeed = 0;
        }
        if (restoreCoolDown > 0)
        {
            restoreCoolDown -= 1 * Time.deltaTime;
            calculator3 = restoreCoolDown / restoreCoolDownLength;
            CoolDownRestore(calculator3);
        }
        if (restoreCoolDown < 0)
            restoreCoolDown = 0;

        //Cold Spell (StormShield)
        if (Input.GetMouseButton(1) && spells == 2 && shieldCoolDown <= 0 && chargeShot <= 0 && attackRate <= 0)  //right click
        {
            casting = true;
            anim.SetBool("PowerUp", true);

            shieldChild.SetActive(true);
            shieldCoolDown = shieldCoolDownLength;
            shieldTimer = 18;
            armor += shield;

            //prevent player from moving
            self.GetComponent<PlayerMovement>().moveSpeed = 0;
            restoreTimer = 1;
            casting = true;
        }
        //turning shield off
        if (shieldChild.activeSelf && shieldTimer != 0 && shieldTimer < 1)
        {
            shieldTimer = 0;
            shieldChild.SetActive(false);
            armor -= shield;
        }
        //shield timer
        if (shieldTimer >= 1)
            shieldTimer -= 1 * Time.deltaTime;
        //shield Cooldown
        if (shieldCoolDown > 0)
        {
            calculator2 = shieldCoolDown / shieldCoolDownLength;
            CoolDownShield(calculator2);
            shieldCoolDown -= 1 * Time.deltaTime;
        }
        if (shieldCoolDown < 0)
            shieldCoolDown = 0;

        //Spell 4 (Shock Wave)
        if (Input.GetMouseButtonDown(1) && spells == 3 && lightCoolDown <= 0 && chargeShot <= 0 && attackRate <= 0)   //right click
        {
            casting = true;
            restoreTimer = 1;            
            anim.SetBool("PowerUp", true);

            _mouse.Lightning();
            lightCoolDown = lightCoolDownLength;
            au_light.Play();
        }
        if (lightCoolDown > 0)
        {
            calculator4 = lightCoolDown / lightCoolDownLength;
            CoolDownLight(calculator4);
            lightCoolDown -= 1 * Time.deltaTime;
        }
        if (lightCoolDown < 0)
            lightCoolDown = 0;

        //**directional combat**

        //facing right
        if (self.GetComponent<PlayerMovement>().moveRight)
        {
            up.SetActive(false);
            down.SetActive(false);
            left.SetActive(false);
            right.SetActive(true);
        }

        //facing left
        if (self.GetComponent<PlayerMovement>().moveLeft)
        {
            up.SetActive(false);
            down.SetActive(false);
            left.SetActive(true);
            right.SetActive(false);
        }

        //facing up
        if (self.GetComponent<PlayerMovement>().moveUp)
        {
            up.SetActive(true);
            down.SetActive(false);
            left.SetActive(false);
            right.SetActive(false);
        }

        //facing down
        if (self.GetComponent<PlayerMovement>().moveDown)
        {
            up.SetActive(false);
            down.SetActive(true);
            left.SetActive(false);
            right.SetActive(false);
        }
    }

    //de-activating smokeChild

    void OnTriggerStay2D(Collider2D target)
    {
        if (target.gameObject.tag == "Smoke" && chargeDistance <= 0)
        {
            smokeChild.SetActive(false);
        }
    }
    //charge shot calculations

    public void SetEnergy(float myEnergy)
    {
        energyBar.transform.localScale = new Vector3(myEnergy, energyBar.transform.localScale.y, energyBar.transform.localScale.z);
    }
    //ice cooldown calculations
    public void CoolDownice(float ice)
    {
        CoolDownImageIce.transform.localScale = new Vector3(CoolDownImageIce.transform.localScale.x, calculator4, CoolDownImageIce.transform.localScale.z);
    }
    //cooldown for restoration
    public void CoolDownRestore(float Restorex)
    {
        CoolDownImageRestore.transform.localScale = new Vector3(CoolDownImageRestore.transform.localScale.x, calculator3, CoolDownImageRestore.transform.localScale.z);
    }
    //cooldown for shield
    public void CoolDownShield(float Sheildx)
    {
        CoolDownImageShield.transform.localScale = new Vector3(CoolDownImageShield.transform.localScale.x, calculator2, CoolDownImageShield.transform.localScale.z);
    }
    //cooldown for shield
    public void CoolDownLight(float Lightx)
    {
        CoolDownImageLight.transform.localScale = new Vector3(CoolDownImageLight.transform.localScale.x, calculator4, CoolDownImageLight.transform.localScale.z);
    }
}