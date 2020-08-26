using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text victoryMessage;
    [SerializeField] Text loseMessage;

    private AdManager adManager;

    [SerializeField] Button restartButton;
    [SerializeField] Button resumeGameButton;
    [SerializeField] Button backToMenu;
    [SerializeField] Button pauseGameButton;
    [SerializeField] Button nextLevelButton;
    [SerializeField] Button watchAdButton;
    [SerializeField] Image screenFadeImage;

    [SerializeField] Text ammoCount;
    [SerializeField] Image ammoImage;
    [SerializeField] ParticleSystem victoryParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject Lasers;
    [SerializeField] float force = 0.2f;
    [SerializeField] float laserDelay = 2f;
    [SerializeField] float size = 4f;

    private GameObject clone;
    private RectTransform pauseTransform;
    private Transform startThrowingPosition;
    private Rigidbody cloneRb;

    private int gamesWithoutAd;
    private string adCount = "gamesWithoutAd";

    private LevelManager levelManager;
    public int currentLevelNum;

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

    public bool isGame = false;
    public bool isLast = false;
    public int maxShootCount = 5;
    public bool isRewarded;

    public List<GameObject> listOfEnemies = new List<GameObject>();


    private void Awake()
    {
        isRewarded = false;

        StartCoroutine(DelayResume());

        if (PlayerPrefs.HasKey(adCount)) gamesWithoutAd = PlayerPrefs.GetInt(adCount);
        else { gamesWithoutAd = 0; PlayerPrefs.SetInt(adCount, gamesWithoutAd); PlayerPrefs.Save(); }

        listOfEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        ammoCount.text = maxShootCount.ToString();

        victorySound = victoryMessage.GetComponent<AudioSource>();
        loseSound = loseMessage.GetComponent<AudioSource>();
        pauseTransform = pauseGameButton.GetComponent<RectTransform>();
        preDirection = GetComponent<LineRenderer>();

        maxXValue = Screen.width + pauseTransform.rect.x - pauseTransform.rect.width * pauseTransform.localScale.x;
        maxYValue = Screen.height + pauseTransform.rect.y - pauseTransform.rect.height * pauseTransform.localScale.y;


        gameMusic = GameObject.Find("Audio Manager").GetComponent<AudioSource>();
        throwingSound = GameObject.Find("Player").GetComponent<AudioSource>();
        startThrowingPosition = GameObject.Find("Start Position").GetComponent<Transform>();
        adManager = GameObject.Find("AdManager").GetComponent<AdManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();


        if (currentLevelNum>=20)
        InvokeRepeating("LaserActivating", 0.5f, laserDelay);

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
                        preDirection.SetPosition(1, startThrowingPosition.position + new Vector3(deltaPos.x * size, deltaPos.y * size, 0));
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
            restartButton.gameObject.SetActive(true);
            backToMenu.gameObject.SetActive(true);

            victoryParticles.Play();
            victorySound.Play();
            gameMusic.Stop();

            ammoCount.gameObject.SetActive(false);
            ammoImage.gameObject.SetActive(false);
            pauseGameButton.gameObject.SetActive(false);
            isGame = false;
            nextLevelButton.gameObject.SetActive(true);
            if (currentLevelNum == levelManager.GetLevelNum())
            {
                levelManager.CompleteLevel();
            }
            GameCountWithoutAd();
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
            ammoCount.gameObject.SetActive(false);
            ammoImage.gameObject.SetActive(false);
            pauseGameButton.gameObject.SetActive(false);
            isGame = false;
            if (!isRewarded) StartCoroutine(LoseOffer());
            else
            {
                loseMessage.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
                backToMenu.gameObject.SetActive(true);
                gameMusic.Stop();
                GameCountWithoutAd();
            }
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        destroyedCount = 0;
        isGame = true;
        Time.timeScale = 1;
        isRewarded = false;       
        GameCountWithoutAd();
    }
    public int DestroyCount(GameObject objectToDestroy)
    {
        Destroy(objectToDestroy);
        return ++destroyedCount;
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
        StartCoroutine(DelayResume());
        Time.timeScale = 1;
        backToMenu.gameObject.SetActive(false);
        resumeGameButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        pauseGameButton.gameObject.SetActive(true);
        loseMessage.gameObject.SetActive(false);
    }
    public IEnumerator DelayResume ()
    {
        yield return new WaitForSeconds(0.5f);
        isGame = true;
    }
    public void PlayDeathParticles(GameObject other)
    {
        deathParticles.transform.position = other.transform.position;
        deathParticles.Play();
    }
    public void GoToNextLevel()
    {
        int sceneNumber = SceneManager.GetActiveScene().buildIndex;
        sceneNumber++;
        SceneManager.LoadScene(sceneNumber);
        isRewarded = false;
    }   
    private void LaserActivating()
    {
        if (Lasers.gameObject.activeSelf)
        {
            Lasers.SetActive(false);
        }
        else
        {
            Lasers.gameObject.SetActive(true);
        }
    }
    public void RewardForWatchingAd()
    {
        {
            isRewarded = true;
            if (shootCount == maxShootCount)
            {
                StartCoroutine(DelayResume());
                loseMessage.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(false);
                screenFadeImage.gameObject.SetActive(false);
                ammoCount.gameObject.SetActive(true);
                ammoImage.gameObject.SetActive(true);
                pauseGameButton.gameObject.SetActive(true);
                backToMenu.gameObject.SetActive(false);
                watchAdButton.gameObject.SetActive(false);
                ammoCount.text = "1";
                gameMusic.Play();
                maxShootCount++;
            }
            else
            {
                maxShootCount++;
            }
        }
    }
    private void GameCountWithoutAd()
    {
        gamesWithoutAd = PlayerPrefs.GetInt(adCount);
        gamesWithoutAd++;
        if (gamesWithoutAd < 7)
        {
            PlayerPrefs.SetInt(adCount, gamesWithoutAd);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt(adCount, 0);
            PlayerPrefs.Save();
            adManager.ShowStandartVideoAd();
        }
    }
    private IEnumerator LoseOffer()
    {
        if (!isRewarded)
        {
            screenFadeImage.gameObject.SetActive(true);
            watchAdButton.gameObject.SetActive(true);
            loseMessage.gameObject.SetActive(true);
            loseSound.Play();
            yield return new WaitForSeconds(3f);
            if (!isRewarded)
            {
                screenFadeImage.gameObject.SetActive(false);
                watchAdButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(true);
                backToMenu.gameObject.SetActive(true);
                gameMusic.Stop();
                GameCountWithoutAd();
            }
        }


    }
}
