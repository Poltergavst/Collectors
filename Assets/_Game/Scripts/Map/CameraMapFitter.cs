using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMapFitter : MonoBehaviour
{
    [SerializeField] private MapAreaConfig _config;
    [SerializeField] private float _padding = 1f;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _camera.orthographic = true;

        FitCamera();
    }

    public void FitCamera()
    {
        float mapSize = _config.Radius * 2f;
        float sizeByHeight = mapSize / 2f;
        float sizeByWidth = mapSize / (2f * _camera.aspect);

        _camera.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth) + _padding;
    }
}