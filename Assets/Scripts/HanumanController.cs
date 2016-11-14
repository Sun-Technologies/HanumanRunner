using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HanumanController : MonoBehaviour {

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

	public AudioSource jetpackAudio;
	
	public AudioSource footstepsAudio;

	public ParallaxScroll parallax;

    public Transform scorePos;

    public GameObject ImpactSprite;

    public Text ScoreText;

    public Text LivesText;

    GameState gameState;

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator>();
        ImpactSprite.SetActive(false);
	}

	void FixedUpdate () 
	{
		bool jetpackActive = Input.GetButton("Fire1");
		
		jetpackActive = jetpackActive && !dead;
		
		if (jetpackActive)
		{
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jetpackForce));
		}
		
		if (!dead)
		{
			Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
			newVelocity.x = forwardMovementSpeed;
			GetComponent<Rigidbody2D>().velocity = newVelocity;
		}
		
		UpdateGroundedStatus();
		
		AdjustJetpack(jetpackActive);

		AdjustFootstepsAndJetpackSound(jetpackActive);

		parallax.offset = transform.position.x;
	} 

	void UpdateGroundedStatus()
	{
		//1
		grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
		
		//2
		animator.SetBool("grounded", grounded);
	}

	void AdjustJetpack (bool jetpackActive)
	{
		jetpack.enableEmission = !grounded;
		jetpack.emissionRate = jetpackActive ? 300.0f : 75.0f; 
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Coins"))
			CollectCoin(collider);
		else
			HitByObstacle(collider);
	}
		
	void HitByObstacle(Collider2D laserCollider)
	{
		if (!dead)
			laserCollider.gameObject.GetComponent<AudioSource>().Play();

		dead = true;
        StartCoroutine(FlashImpactSprite());
        UiManager.instance.lives--;
        LivesText.text = "x " + UiManager.instance.lives.ToString();
        PlayerPrefsStorage.SaveData(UiManager.ScoreKey, UiManager.instance.coins);
        PlayerPrefsStorage.SaveData(UiManager.LivesKey, UiManager.instance.lives);
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

	void CollectCoin(Collider2D coinCollider)
	{
		UiManager.instance.coins++;
        Destroy(coinCollider.gameObject);
        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
	}

    IEnumerator AnimateCoin(Transform obj)
    {
        //Vector3.MoveTowards(obj.position, scorePos.position, Time.deltaTime * 5);
        //Vector3.Slerp(obj.position, scorePos.position, 2);
        yield return new WaitForSeconds(2);
        Destroy(obj.gameObject);
    }

	void OnGUI()
	{
		DisplayCoinsCount();

		DisplayRestartButton();
	}

	void DisplayCoinsCount()
	{
        ScoreText.text = UiManager.instance.coins.ToString();
    }

	void DisplayRestartButton()
	{
		//if (dead && grounded)
		//{
  //          //GameOverScreen.SetActive(true);
  //          UiManager.instance.SwitchGameState(GameState.GameOver);
		//}
	}

	void AdjustFootstepsAndJetpackSound(bool jetpackActive)    
	{
		footstepsAudio.enabled = !dead && grounded;
		
		jetpackAudio.enabled =  !dead && !grounded;
		jetpackAudio.volume = jetpackActive ? 1.0f : 0.5f;        
	}

    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void ShareToFb()
    {
        UiManager.instance.PostToFacebook();
    }
}
