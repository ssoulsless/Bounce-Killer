using System.Collections;
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

    private GameObject clone;
    private RectTransform pauseTransform;
    private Transform startThrowingPosition;
    private Rigidbody cloneRb;
    private AudioSource loseSound;
    private AudioSource victorySound;
    private AudioSource gameMusic;
    private AudioSource throwingSound;
    private LineRenderer preDirection;
    private int destroyedCount = 0;
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private Vector2 deltaPos;
    private int shootCount = 0;
    private float maxXValue;
    private float maxYValue;

    public bool isGame = true;
    public bool isLast = false;
    public int maxShootCount = 5;
    public List<GameObject> listOfEnemies = new List<GameObject>();


    private void Awake()
    {
        listOfEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        ammoCount.text = maxShootCount.ToString();
        pauseTransform = pauseGameButton.GetComponent<RectTransform>();
        maxXValue = Screen.width + pauseTransform.rect.x - pauseTransform.rect.width * pauseTransform.localScale.x;
        maxYValue = Screen.height + pauseTransform.rect.y - pauseTransform.rect.height * pauseTransform.localScale.y;
        victorySound = victoryMessage.GetComponent<AudioSource>();
        loseSound = loseMessage.GetComponent<AudioSource>();
        gameMusic = GameObject.Find("Audio Manager").GetComponent<AudioSource>();
        throwingSound = GameObject.Find("Player").GetComponent<AudioSource>();
        preDirection = GetComponent<LineRenderer>();
        startThrowingPosition = GameObject.Find("Start Position").GetComponent<Transform>();
    }
    private void Update()
    {
        if (Input.touchCount != 0)
        {
            if (isGame)
            {
                Touch _touch;
                _touch = Input.GetTouch(0);
                preDirection.SetPosition(0, startThrowingPosition.position); 
                if (_touch.phase == TouchPhase.Began)
                {
                    startTouchPos = _touch.position;
                }
                if (_touch.phase == TouchPhase.Moved) 
                {
                    endTouchPos = _touch.position;
                    if (!((endTouchPos.x >= maxXValue && endTouchPos.y > maxYValue) || (startTouchPos.x >= maxXValue && startTouchPos.y >= maxYValue)))
                    {
                        deltaPos = endTouchPos - startTouchPos;
                        deltaPos.Normalize();
                        preDirection.SetPosition(1, startThrowingPosition.position + new Vector3(deltaPos.x * 5, deltaPos.y * 5, 0));
                    }
                    else
                    {
                        startTouchPos = Vector2.zero;
                        endTouchPos = Vector2.zero;
                        deltaPos = Vector2.zero;
                        preDirection.SetPosition(1, startThrowingPosition.position);
                    }
                }
                if (_touch.phase == TouchPhase.Ended)
                {
                    endTouchPos = _touch.position;
                    deltaPos = endTouchPos - startTouchPos;
                    if (deltaPos != Vector2.zero)
                    {
                        preDirection.SetPosition(1, startThrowingPosition.position);
                        if (!((endTouchPos.x >= maxXValue && endTouchPos.y > maxYValue) || (startTouchPos.x >= maxXValue && startTouchPos.y >= maxYValue)))
                        {
                            Shoot(deltaPos.normalized, ++shootCount);
                        }
                    }
                }
            }
        }
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
            pauseGameButton.gameObject.SetActive(false);
            isGame = false;
            victorySound.Play();
            gameMusic.Stop();
        }
    }
    public void Shoot(Vector2 direction, int shootCount)
    {
         if (shootCount <= maxShootCount && isGame)
         {
            ammoCount.text = (maxShootCount - shootCount).ToString();
            clone = Instantiate(projectilePrefab, startThrowingPosition.position, startThrowingPosition.rotation) as GameObject;
            throwingSound.Play();
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
            pauseGameButton.gameObject.SetActive(false);
            backToMenu.gameObject.SetActive(true);
            isGame = false;
            loseSound.Play();
            gameMusic.Stop();
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
        StartCoroutine(delayResume());
        Time.timeScale = 1;
        backToMenu.gameObject.SetActive(false);
        resumeGameButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        pauseGameButton.gameObject.SetActive(true);
    }
    public IEnumerator delayResume ()
    {
        yield return new WaitForSeconds(0.5f);
        isGame = true;
    }
}
