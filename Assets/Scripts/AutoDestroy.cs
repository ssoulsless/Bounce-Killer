using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private int bounces = 0;
    [SerializeField] int maxBounceCount;
    private Component enemyDetector = null;
    [SerializeField] GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
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
    private void OnTriggerEnter(Collider other)
    {
        enemyDetector = other.gameObject.GetComponent<EnemyDetector>();
        if (enemyDetector!=null)
        {
            Destroy(other.gameObject);
            gameManager.RemoveEnemies(other.gameObject);
            if (gameManager.listOfEnemies.Count <= 0)
            {
                gameManager.Victory();
            }
        }
    }

}