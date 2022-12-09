using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    // MOVEMENT 
    public float speed = 4;

    // MOVEMENT ADDONS
    public float timeSlowing = 0.5f;  //Slow Down
    float speedSlowTimer;
    bool isSlowing;
    public float timeBoosting = 4.0f;  //Speed Boost
    float speedBoostTimer;
    bool isBoosting;
    
    //  HEALTH 
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public ParticleSystem hitParticle;
    
    // PROJECTILE 
    public GameObject projectilePrefab;
    public int ammo {get { return currentAmmo; } }
    public int currentAmmo;

    //  AUDIO 
    public AudioClip hitSound;
    public AudioClip shootingSound;
    public AudioClip QuestComplete;
    public AudioSource backgroundManager;
    public AudioClip WinSound;
    public AudioClip LoseSound;

    // EFFECTS
    public ParticleSystem healthEffect;
    public ParticleSystem potionEffect;
    
    //  HEALTH 
    public int health
    {
        get { return currentHealth; }
    }
    
    // MOVEMENT 
    Rigidbody2D rigidbody2d;
    Vector2 currentInput;
    
    //  HEALTH 
    int currentHealth;
    float invincibleTimer;
    bool isInvincible;
   
    //  ANIMATION 
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    
    //  SOUNDS 
    AudioSource audioSource;

    // TEXT
    public GameObject WinTextObject;
    public GameObject LoseTextObject;
    public static int level = 1; //Level
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI fixedText;
    private int scoreFixed = 0;
    bool gameOver;
    bool winGame;

    public TextMeshProUGUI HoneyText;
    private int scoreHoney = 0;

    
    void Start()
    {
        //  MOVEMENT 
        rigidbody2d = GetComponent<Rigidbody2D>();
                
        //  HEALTH 
        invincibleTimer = -1.0f;
        currentHealth = maxHealth;
        
        //  ANIMATION 
        animator = GetComponent<Animator>();
        
        //  AUDIO 
        audioSource = GetComponent<AudioSource>();

        // FIXED
        fixedText.text = "Robots Fixed: " + scoreFixed.ToString() + "/4";

        HoneyText.text = "Honey Jars: " + scoreHoney.ToString() + "/3";

        // AMMO
        AmmoText();
        
        // WIN LOSS
        WinTextObject.SetActive(false);
        LoseTextObject.SetActive(false);
        gameOver = false;
        winGame = false;

        // EFFECTS
        healthEffect.Stop();
        potionEffect.Stop();
    }

    void Update()
    {
        // HEALTH 
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        // MOVEMENT 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
                
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        currentInput = move;

        // SPEED BOOST
         if (isBoosting == true)
        {
            speedBoostTimer -= Time.deltaTime; 
            speed = 7;

            if (speedBoostTimer < 0)
            {
                isBoosting = false;
                speed = 4;
            }
        }

        // SLOW SPEED

         if (isSlowing == true)
        {
            speedSlowTimer -= Time.deltaTime;
            speed = 1;

            if (speedSlowTimer < 0)
            {
                isSlowing = false;
                speed = 4;
            }
        }

        //  ANIMATION 

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //  PROJECTILE 

        if (Input.GetKeyDown(KeyCode.C))
          {
            
            
            if (currentAmmo > 0)
            {
                ChangeAmmo(-1);
                LaunchProjectile();
                AmmoText();
            }
          }
        
        //  DIALOGUE 
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, 1 << LayerMask.NameToLayer("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                        if (scoreFixed >= 4)
                            {
                                SceneManager.LoadScene("Level 2");
                                level = 2;
                            }
                        else
                        {
                            character.DisplayDialog();
                         } 
                } 
            }
        }

        // RESTART
        if (Input.GetKeyDown(KeyCode.R))
        {
           if (gameOver == true)
           {
                // this loads the currently active scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
           }

            if (winGame == true)
            {
                SceneManager.LoadScene("Level 1");
                level = 1;
            }
        }
 
}

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        
        position = position + currentInput * speed * Time.deltaTime;
        
        rigidbody2d.MovePosition(position);
    }

    //  HEALTH 
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        { 
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            
            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(hitSound);

            Instantiate(hitParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        if(currentHealth == 0)
        {
            LoseTextObject.SetActive(true);
            gameOver = true;

            Destroy(gameObject.GetComponent<SpriteRenderer>());

            speed = 0;

            audioSource.clip = LoseSound;
            audioSource.Play();
        }
            
        
        UIHealthBar.Instance.SetValue(currentHealth / (float)maxHealth);
    }
    
    // MMOVEMENT ADDON
     public void SpeedSlow(int amount)
    {
        if (amount < 0)
        {
            speedSlowTimer = timeSlowing;
            isSlowing = true;
        }
    }
    
    public void SpeedBoost(int amount)
    {
        if (amount > 0)
        {
            speedBoostTimer = timeBoosting;
            isBoosting = true;
        }
    }
    
    //  PROJECTICLE & AMMO COUNT
    void LaunchProjectile()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        
        animator.SetTrigger("Launch");
        audioSource.PlayOneShot(shootingSound);
    }
    
    public void ChangeAmmo(int amount)
    {
        
        currentAmmo = Mathf.Abs(currentAmmo + amount);
        Debug.Log(" 5 " + currentAmmo);
    }

    public void AmmoText()
    {
        ammoText.text = " 5 " + currentAmmo.ToString();
    }
        // HONEY QUEST
    public void HoneyCount(int amount)
    {
        scoreHoney += amount;
        HoneyText.text = "Honey Jars: " + scoreHoney.ToString() + "/3";

        Debug.Log("Honey Jars: " + scoreHoney);
        if (scoreHoney ==3 & level ==2)
        {
            WinTextObject.SetActive(true);
            audioSource.PlayOneShot(QuestComplete);
            winGame = true;

            transform.position = new Vector3(-5f, 0f, -100f);
            speed = 0;

            backgroundManager.Stop();
            PlaySound(WinSound);

            speed = 0;
        }

    }

    public void FixedRobots(int amount)
    {
        scoreFixed += amount;
        fixedText.text = "Robots Fixed: " + scoreFixed.ToString() + "/4";

        Debug.Log("Robots Fixed: " + scoreFixed);

        //move to level 2 via Jambi ONLY if at Level 1
        if (scoreFixed == 4 & level == 1)
        {
            WinTextObject.SetActive(true);
            audioSource.PlayOneShot(QuestComplete);
        }

        //win game via Jambi ONLY if on level 2
        if (scoreFixed == 4 & level == 2)
        {
            WinTextObject.SetActive(true);
            audioSource.PlayOneShot(QuestComplete);
            winGame = true;

            transform.position = new Vector3(-5f, 0f, -100f);
            speed = 0;

            backgroundManager.Stop();
            PlaySound(WinSound);

            speed = 0;
        }
    }
    
    //  SOUND 

    //Allow to play a sound on the player sound source. used by Collectible
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
