using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI victoryMessage;
    [SerializeField] TextMeshProUGUI loseMessage;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI ammoCount;
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
        ammoCount.text = "Ammo: " + maxShootCount;
    }
    public void Victory()
    {
        if (isGame)
        {
            victoryMessage.gameObject.SetActive(true);
            victoryParticles.Play();
            ammoCount.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
            isGame = false;
        }
    }
    public void Shoot(Vector2 direction, int shootCount)
    {
         if (shootCount < maxShootCount)
         {
            ammoCount.text = "Ammo: " + (maxShootCount - shootCount);
            clone = Instantiate(projectilePrefab, playerTransform) as GameObject;
            cloneRb = clone.GetComponent<Rigidbody>();
            cloneRb.AddForce(direction * force, ForceMode.Impulse);
         }
        if (shootCount == maxShootCount)
        {
            isLast = true;
            ammoCount.text = "Ammo: " + (maxShootCount - shootCount);
            clone = Instantiate(projectilePrefab, playerTransform) as GameObject;
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
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        destroyedCount = 0;
        isGame = true;
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
}
