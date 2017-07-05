using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HanumanController : MonoBehaviour
{

    public AnimClips[] AnimClipsList;

    public float forwardMovementSpeed = 3.0f;

    public Transform groundCheckTransform;

    private bool grounded;

    public LayerMask groundCheckLayerMask;

    Animator animator;

    private bool dead = false;

    public Texture2D coinIconTexture;

    public AudioClip coinCollectSound;

    public ParallaxScroll parallax;

    public GameObject ImpactSprite;

    public Text ScoreText;

    public Text LivesText;

    public static int currentScore;

    GameState gameState;

    public float speedTimer = 30;

    public bool isInvincible = false;

    public AnimatorOverrideController HanumanSilverController;

    public AnimatorOverrideController HanumanGoldController;

    public AnimatorOverrideController HanumanBasicController;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        ImpactSprite.SetActive(false);
        currentScore = 0;
        forwardMovementSpeed = 3;
    }

    void FixedUpdate()
    {
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

            Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
            newVelocity.x = forwardMovementSpeed;
            GetComponent<Rigidbody2D>().velocity = newVelocity;
        }
        DisplayCoinsCount();

        UpdateGroundedStatus();

        parallax.offsetX = transform.position.x;
        //parallax.offsetY = transform.position.y;

        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefsStorage.ClearLocalStorageData(true);
        }
    }

    void UpdateGroundedStatus()
    {
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

        animator.SetBool("grounded", grounded);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Coins"))
            StartCoroutine(CollectCoin(collider));
        else
            HitByObstacle(collider);

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

            dead = true;
            StartCoroutine(FlashImpactSprite());

            animator.SetBool("dead", true);
            gameState = GameState.GameOver;
            Debug.Log("Dead");
            StartCoroutine(ShowGameOver());
        }
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
