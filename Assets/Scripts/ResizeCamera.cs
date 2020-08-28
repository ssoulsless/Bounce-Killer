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
        if (x >= 16)
        {
            x *= 120;
            _camera.orthographicSize = x / 200;
            if (_camera.orthographicSize >= 12)
            {
                _camera.orthographicSize = 12;
            }
        }
    }
}
