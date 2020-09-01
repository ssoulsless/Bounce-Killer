using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int maxBounceCount;

    private EnemyDetector enemyDetector = null;
    private SpikeDetector spikeDetector = null;
    private LaserDetector laserDetector = null;
    private AudioSource deathSound;
    private AudioSource bounceSound;
    private AudioSource laserHit;
    private AudioSource spikeHit;
    private int bounces = 0;

    private bool isSpiked = false;
    private bool isLasered = false;

    void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        deathSound = GameObject.Find("Enemies").GetComponent<AudioSource>();

        if(gameManager.GetCurrentLevelNum() >= 21)  laserHit = GameObject.Find("LaserSystem").GetComponent<AudioSource>();
        if((gameManager.GetCurrentLevelNum() >= 11)&&(gameManager.GetCurrentLevelNum() <= 20)) spikeHit = GameObject.Find("Spikes").GetComponent<AudioSource>();
        bounceSound = this.GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("effects"))
        {
            bounceSound.volume = PlayerPrefs.GetFloat("effects");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!((collision.gameObject.GetComponent<EnemyDetector>())||(collision.gameObject.GetComponent<SpikeDetector>())||(collision.gameObject.GetComponent<LaserDetector>())))
        {
            bounceSound.Play();
            if (++bounces >= maxBounceCount)
            {
                bounces = 0;
                if (gameManager.DestroyCount(this.gameObject) >= gameManager.maxShootCount)
                {
                    if (gameManager.listOfEnemies.Count <= 0)
                    {
                        gameManager.Victory();
                    }
                    else
                    {
                        gameManager.Lose();
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        enemyDetector = other.gameObject.GetComponent<EnemyDetector>();
        spikeDetector = other.gameObject.GetComponent<SpikeDetector>();
        laserDetector = other.gameObject.GetComponent<LaserDetector>();
        
        if (enemyDetector!=null)
        {
            gameManager.PlayDeathParticles(other.gameObject);
            deathSound.Play();
            Destroy(other.gameObject);
            gameManager.RemoveEnemies(other.gameObject);
            if (gameManager.listOfEnemies.Count <= 0)
            {
                gameManager.Victory();
            }
        }
        if ((spikeDetector!=null) && !(isSpiked))
        {
            isSpiked = true;
            bounces = 0;
            spikeHit.Play();
            if (gameManager.DestroyCount(this.gameObject) >= gameManager.maxShootCount)
            {
                if (gameManager.listOfEnemies.Count <= 0)
                {
                    gameManager.Victory();
                }
                else
                {
                    gameManager.Lose();
                }
            }
        }
        if ((laserDetector!= null) && !(isLasered))
        {
            isLasered = true;
            bounces = 0;
            laserHit.Play();
            if (gameManager.DestroyCount(this.gameObject) >= gameManager.maxShootCount)
            {
                if (gameManager.listOfEnemies.Count <= 0)
                {
                    gameManager.Victory();
                }
                else
                {
                    gameManager.Lose();
                }
            }
        }
    }

}