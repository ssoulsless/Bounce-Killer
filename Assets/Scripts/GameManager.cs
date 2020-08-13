using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text victoryMessage;
    [SerializeField] Text loseMessage;
    [SerializeField] Button restartButton;
    [SerializeField] Button resumeGameButton;
    [SerializeField] Button backToMenu;
    [SerializeField] Button pauseGameButton;
    [SerializeField] Text ammoCount;
    [SerializeField] Image ammoImage;
    [SerializeField] ParticleSystem victoryParticles;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float force = 0.2f;
    public List<GameObject> listOfEnemies = new List<GameObject>();
    private GameObject clone;
    private Transform playerTransform;
    private Rigidbody cloneRb;
    private int destroyedCount = 0;
    public bool isGame = true;
    public bool isLast = false;
    public int maxShootCount = 5;


    private void Start()
    {
        listOfEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        ammoCount.text = maxShootCount.ToString();
    }
    public void Victory()
    {
        if (isGame)
        {
            victoryMessage.gameObject.SetActive(true);
            victoryParticles.Play();
            ammoCount.gameObject.SetActive(false);
            ammoImage.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
            backToMenu.gameObject.SetActive(true);
            isGame = false;
        }
    }
    public void Shoot(Vector2 direction, int shootCount)
    {
         if (shootCount <= maxShootCount && isGame)
         {
            ammoCount.text = (maxShootCount - shootCount).ToString();
            clone = Instantiate(projectilePrefab, playerTransform.position, playerTransform.rotation) as GameObject;
            cloneRb = clone.GetComponent<Rigidbody>();
            cloneRb.AddForce(direction * force, ForceMode.Impulse);
         }
    }
    public void Lose()
    {
        if (isGame)
        {
            loseMessage.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            ammoCount.gameObject.SetActive(false);
            isGame = false;
            backToMenu.gameObject.SetActive(true);
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        destroyedCount = 0;
        isGame = true;
        Time.timeScale = 1;
    }
    public int DestroyCount()
    {
        destroyedCount++;
        return destroyedCount;
    }
    public void RemoveEnemies(GameObject enemy)
    {
        listOfEnemies.Remove(enemy);
    }
    public void PauseGame()
    {
        isGame = false;
        backToMenu.gameObject.SetActive(true);
        resumeGameButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        Time.timeScale = 0;
        pauseGameButton.gameObject.SetActive(false);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        isGame = true;
        backToMenu.gameObject.SetActive(false);
        resumeGameButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        pauseGameButton.gameObject.SetActive(true);
    }
}
