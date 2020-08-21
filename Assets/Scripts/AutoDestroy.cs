using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int maxBounceCount;

    private Component enemyDetector = null;
    private Component spikeDetector = null;
    private AudioSource deathSound;
    private AudioSource bounceSound;
    private int bounces = 0;

    private bool isSpiked = false;

    void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        deathSound = GameObject.Find("Enemies").GetComponent<AudioSource>();
        bounceSound = this.GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("effects"))
        {
            bounceSound.volume = PlayerPrefs.GetFloat("effects");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!((collision.gameObject.GetComponent<EnemyDetector>())||(collision.gameObject.GetComponent<SpikeDetector>())))
        {
            bounceSound.Play();
            if (++bounces >= maxBounceCount)
            {
                bounces = 0;
                if (gameManager.DestroyCount(this.gameObject) == gameManager.maxShootCount)
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
            if (gameManager.DestroyCount(this.gameObject) == gameManager.maxShootCount)
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