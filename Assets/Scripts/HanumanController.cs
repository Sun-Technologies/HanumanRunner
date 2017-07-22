using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HanumanController : MonoBehaviour
{
    public float flyingForce = 75.0f;

    //public AnimClips[] AnimClipsList;

    public float forwardMovementSpeed = 3.0f;

    public float lavaFactor = 1.0f; 

    public Transform groundCheckTransform;

    public bool grounded;

    public LayerMask groundCheckLayerMask;

    public Animator animator;

    private bool dead = false;

    public Texture2D coinIconTexture;

    public AudioClip coinCollectSound;

    public GameObject[] LevelObjects;

    public ParallaxScroll[] ParallaxObjects;

    public GameObject ImpactSprite;

    public Text ScoreText;

    public Text LivesText;

    public static int currentScore;

    public static int enemiesKilled; 

    GameState gameState;

    public float speedTimer = 30;

    public bool isInvincible = false;

    HanumanGearInfo _hanumanGearInfo;

    public bool hasEquippedGada = false;

    public bool CanKillEnemies = false;

    public Vector2 newVelocity;

    public GameObject gadaButton;

    public int levelType;



    private void Awake()
    {
        if (Application.isEditor)
        {
            Application.runInBackground = true;
        }
    }

    void Start()
    {
        for (int i = 0; i < ParallaxObjects.Length; i++)
        {
            if (i == HanumanGearInfo.levelIndex)
            {
                ParallaxObjects[i].gameObject.SetActive(true);
            }
            else
            {
                ParallaxObjects[i].gameObject.SetActive(false);
            }
        }
        
        ImpactSprite.SetActive(false);
        currentScore = 0;
        enemiesKilled = 0;
        forwardMovementSpeed = 3;
        _hanumanGearInfo = GetComponent<HanumanGearInfo>();
        gadaButton.SetActive(false);
        levelType = (int)_hanumanGearInfo._levelType;
        if (PlayerPrefsStorage.GetIntData(GameData.KEY_GADA_UNLOCKED, 0) == 0)
        {
            hasEquippedGada = false;
        }
        else
        {
            hasEquippedGada = true;
        }

        CanKillEnemies = hasEquippedGada;

        CheckForKillingAbility();
        Debug.Log("Gear type = " + HanumanGearInfo._gearType);
        _hanumanGearInfo.SetAnimController(HanumanGearInfo._gearType);
    }

    void FixedUpdate()
    {
        bool canFly = Input.GetButton("Fire1");

        canFly = canFly && !dead;

        if (canFly)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, flyingForce), ForceMode2D.Force);
        }

        if (!dead)
        {
            speedTimer -= Time.deltaTime;

            if (speedTimer <= 0)
            {
                if (forwardMovementSpeed < 9)
                {
                    forwardMovementSpeed++;
                }
            }

            if (speedTimer < 0)
            {
                speedTimer = 30;
            }
            newVelocity = GetComponent<Rigidbody2D>().velocity;
            newVelocity.x = forwardMovementSpeed;
            GetComponent<Rigidbody2D>().velocity = newVelocity;
        }
        DisplayCoinsCount();

        UpdateGroundedStatus();
        
        ParallaxObjects[HanumanGearInfo.levelIndex].offsetX = transform.position.x;
        //parallax.offsetY = transform.position.y;

        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefsStorage.ClearLocalStorageData(true);
        }
        if (HanumanGearInfo._gearType == GearType.Default)
        {
            gadaButton.SetActive(false);
        }
        else
        {
            gadaButton.SetActive(CanKillEnemies);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoMeleeAttack();
        }
    }

    public void DoMeleeAttack()
    {
        Debug.Log("Attempting to kill some guys");

        if (CanKillEnemies)
        {
            if (grounded)
            {
                Debug.Log("Setting trigger RunAttack");
                animator.SetTrigger("RunAttack");
            }
            else
            {
                Debug.Log("Setting trigger FlyAttack");
                animator.SetTrigger("FlyAttack");
            }
        }
    }

    void UpdateGroundedStatus()
    {
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

        animator.SetBool("grounded", grounded);

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Hit by collider: " + collider.gameObject.name);
        if (collider.gameObject.CompareTag("Coins"))
        {
            StartCoroutine(CollectCoin(collider));
        }
        else if (collider.gameObject.name.Contains("GadaPickUp"))
        {
            StartCoroutine(EquipGada(collider));
        }
        else
        {
            if (collider.gameObject.name != "GadaCollider")
            {
                HitByObstacle(collider);
            }
            else
            {
                Debug.Log("Hit by gada collider. Dafuq brah?");
                Debug.Break();
            }
        }

        if (collider.gameObject.name.Contains("Fireball"))
        {
            Destroy(collider.gameObject);
        }
    }

    void HitByObstacle(Collider2D laserCollider)
    {
        if (!isInvincible)
        {
            if (!dead)
                laserCollider.gameObject.GetComponent<AudioSource>().Play();

            HasDied();
        }
    }

    private void HasDied()
    {
        dead = true;
        StartCoroutine(FlashImpactSprite());

        animator.SetBool("dead", true);
        gameState = GameState.GameOver;
        Debug.Log("Dead");
        StartCoroutine(ShowGameOver());
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        UiManager.instance.SwitchGameState(GameState.GameOver);
    }

    IEnumerator FlashImpactSprite()
    {
        ImpactSprite.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        ImpactSprite.SetActive(false);
    }

    IEnumerator CollectCoin(Collider2D coinCollider)
    {
        coinCollider.gameObject.GetComponent<Animator>().enabled = true;
        currentScore++;
        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position, 0.3f);
        yield return new WaitForSeconds(0.3f);
        if (coinCollider != null)
        {
            Destroy(coinCollider.gameObject);
        }
    }
    public bool isTempGadaOn = false;
    IEnumerator EquipGada(Collider2D gadaCollider)
    {
        float gadaTimeout = 5.0f;
        if (!hasEquippedGada && !isTempGadaOn)
        {
            isTempGadaOn = true;
        }
        
        if (gadaCollider != null)
        {
            gadaCollider.gameObject.transform.position = new Vector3(gadaCollider.gameObject.transform.position.x - 20, gadaCollider.gameObject.transform.position.y, gadaCollider.gameObject.transform.position.z);
        }

        CheckForKillingAbility();
        
        yield return new WaitForSeconds(gadaTimeout);
        isTempGadaOn = false;
    }

    void CheckForKillingAbility()
    {
        Debug.Log("Gear type = " + HanumanGearInfo._gearType);
        if (hasEquippedGada)
        {
            CanKillEnemies = true;
            return;
        }
        switch (HanumanGearInfo._gearType)
        {
            case GearType.Default:
                Debug.Log("No armor available, can't do shit with Gada brah");
                CanKillEnemies = false;
                break;

            case GearType.SilverArmor:
                if (isTempGadaOn)
                {
                    CanKillEnemies = true;
                }
                break;

            case GearType.SilverArmorWithGada:
                CanKillEnemies = true;
                break;

            case GearType.GoldArmor:
                if (isTempGadaOn)
                {
                    CanKillEnemies = true;
                }
                break;

            case GearType.GoldArmorWithGada:
                CanKillEnemies = true;
                break;

        }
    }

    public void KillEnemy()
    {
        //
    }

    void DisplayCoinsCount()
    {
        ScoreText.text = currentScore.ToString();
    }
}

[System.Serializable]
public class AnimClips
{
    public string ClipName;
    public AnimationClip AnimClip;
}
