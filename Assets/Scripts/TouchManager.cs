
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private Vector2 deltaPos;
    private GameManager gameManager;
    private int shootCount = 0;
    [SerializeField] GameObject[] directionalLines;
    private Transform[] linesTransform;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        for (int i = 0; i<directionalLines.Length; i++)
        {
            linesTransform[i] = directionalLines[i].GetComponent<Transform>();
        }
    }
    void Update()
    {
        if (Input.touchCount != 0)
        {
            Touch _touch;
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                startTouchPos = _touch.position;
            }
            if (_touch.phase == TouchPhase.Ended)
            {
                endTouchPos = _touch.position; 
                deltaPos = endTouchPos - startTouchPos;           
                gameManager.Shoot(deltaPos.normalized, ++shootCount);
            }
            
        }
    }
}
