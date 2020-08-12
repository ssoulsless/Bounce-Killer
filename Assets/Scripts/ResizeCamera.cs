using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ResizeCamera : MonoBehaviour
{
    [SerializeField] Camera _camera;
    public float x;

    public void Awake()
    {
        _camera = gameObject.GetComponent<Camera>();
        x = Screen.width / 9;
        x = Screen.height / x;
        x *= 120;
        _camera.orthographicSize = x / 200;
    }
}
