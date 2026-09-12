using UnityEngine;

public class MapBoundsMaker : MonoBehaviour
{
    [SerializeField] private float _wallHeight;
    [SerializeField] private float _wallThickness;
    [SerializeField] private MapAreaConfig _config;
    [SerializeField] private GameObject _ground;
    
    private float _thicknessOffset;
    private Vector3 _wallSize, _wallSizeSides;
    private BoxCollider _wallNear, _wallFar, _wallLeft, _wallRight;

    private void Awake()
    {
        float verticalExtent = _wallHeight / 2;

        _thicknessOffset = _wallThickness * MathConstants.Half;

        _wallNear = CreateWall(nameof(_wallNear));
        _wallFar = CreateWall(nameof(_wallFar));
        _wallLeft = CreateWall(nameof(_wallLeft));
        _wallRight = CreateWall(nameof(_wallRight));

        ResizeMap(_config.Radius);
    }

    private void OnEnable() => _config.RadiusChanged += ResizeMap;
    private void OnDisable() => _config.RadiusChanged -= ResizeMap;

    private BoxCollider CreateWall(string name)
    {
        GameObject wall = new(name);

        BoxCollider collider = wall.AddComponent<BoxCollider>();
        collider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        wall.transform.SetParent(transform);

        return collider;
    }

    private void ResizeWall(BoxCollider wall, Vector3 position, Vector3 size)
    {
        wall.transform.position = position;
        wall.size = size;
    }

    private void UpdateSizeValues()
    {
        _wallSize = new(_config.BoundSize, _wallHeight, _wallThickness);
        _wallSizeSides = new(_wallThickness, _wallHeight, _config.BoundSize);
    }

    private void ResizeMap(float radius)
    {
        float verticalExtent = _wallHeight / 2;

        UpdateSizeValues();

        ResizeWall(_wallNear, new Vector3(0, verticalExtent, radius + _thicknessOffset), _wallSize);
        ResizeWall(_wallFar, new Vector3(0, verticalExtent, -radius - _thicknessOffset), _wallSize);
        ResizeWall(_wallLeft, new Vector3(-radius - _thicknessOffset, verticalExtent, 0), _wallSizeSides);
        ResizeWall(_wallRight, new Vector3(radius + _thicknessOffset, verticalExtent, 0), _wallSizeSides);

        _ground.transform.localScale = (Vector3.one * (_config.BoundSize + _wallThickness * 2)).Change(y: 1);
    }
}