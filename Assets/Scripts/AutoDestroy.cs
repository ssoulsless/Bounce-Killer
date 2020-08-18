using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int maxBounceCount;

    private Component enemyDetector = null;
    private AudioSource deathSound;
    private AudioSource bounceSound;
    private int bounces = 0;

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
        if (!(collision.gameObject.GetComponent<EnemyDetector>()))
        {
            bounceSound.Play();
            if (++bounces >= maxBounceCount)
            {
                Destroy(gameObject);
                bounces = 0;
                if (gameManager.DestroyCount() == gameManager.maxShootCount)
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
    }

}