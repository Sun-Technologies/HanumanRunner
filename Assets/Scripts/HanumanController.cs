using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HanumanController : MonoBehaviour
{

    public float jetpackForce = 75.0f;

    public float forwardMovementSpeed = 3.0f;

    public Transform groundCheckTransform;

    private bool grounded;

    public LayerMask groundCheckLayerMask;

    Animator animator;

    public ParticleSystem jetpack;

    private bool dead = false;

    public Texture2D coinIconTexture;

    public AudioClip coinCollectSound;

    //public AudioSource jetpackAudio;

    //public AudioSource footstepsAudio;

    public ParallaxScroll parallax;

    public Transform scorePos;

    public GameObject ImpactSprite;

    public Text ScoreText;

    public Text LivesText;

    public static int currentScore;

    GameState gameState;

    public float speedTimer = 30;

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
        bool jetpackActive = Input.GetButton("Fire1");

        jetpackActive = jetpackActive && !dead;

        if (jetpackActive)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jetpackForce), ForceMode2D.Force);
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

            Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
            newVelocity.x = forwardMovementSpeed;
            GetComponent<Rigidbody2D>().velocity = newVelocity;
        }
        DisplayCoinsCount();

        UpdateGroundedStatus();

        AdjustJetpack(jetpackActive);

        //AdjustFootstepsAndJetpackSound(jetpackActive);

        parallax.offset = transform.position.x;

        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefsStorage.ClearLocalStorageData(true);
        }
    }

    void UpdateGroundedStatus()
    {
        //1
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

        //2
        animator.SetBool("grounded", grounded);
    }

    void AdjustJetpack(bool jetpackActive)
    {
        jetpack.enableEmission = !grounded;
        jetpack.emissionRate = jetpackActive ? 300.0f : 75.0f;
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
        if (!dead)
            laserCollider.gameObject.GetComponent<AudioSource>().Play();

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
        if (coinCollider.gameObject != null)
        {
            Destroy(coinCollider.gameObject);
        }
    }

    IEnumerator AnimateCoin(Transform obj)
    {
        //Vector3.MoveTowards(obj.position, scorePos.position, Time.deltaTime * 5);
        //Vector3.Slerp(obj.position, scorePos.position, 2);
        yield return new WaitForSeconds(2);
        Destroy(obj.gameObject);
    }

    void DisplayCoinsCount()
    {
        ScoreText.text = currentScore.ToString();
    }

    //void AdjustFootstepsAndJetpackSound(bool jetpackActive)
    //{
    //    footstepsAudio.enabled = !dead && grounded;

    //    jetpackAudio.enabled = !dead && !grounded;
    //    jetpackAudio.volume = jetpackActive ? 1.0f : 0.5f;
    //}
}
