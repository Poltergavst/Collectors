using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BuilderPreview))]
public class BaseBuilder : MonoBehaviour, ICursorInteractable
{
    [SerializeField] private LayerMask _ground;
    [SerializeField] private LayerMask _buildingObstacles;
    [SerializeField] private GameObject _flagPrefab;
    [SerializeField] private MapAreaConfig _mapAreaConfig;

    private BaseRuntime _base;
    private BaseConfig _config;
    private BaseCreator _creator;

    private BuilderPreview _builderPreview;
    private GameObject _flag;
    private Vector3 _placement;
    private bool _isValidPosition;
    private bool _isRequesting;

    private Collider[] _objectsInBuildingRadius;

    public bool IsBuildModeActive { get; private set; }  

    private void Awake()
    {
        IsBuildModeActive = false;

        _builderPreview = GetComponent<BuilderPreview>();
        _objectsInBuildingRadius = new Collider[10];

        _builderPreview.Create();

        _flag = Instantiate(_flagPrefab, transform);
        _flag.SetActive(false);
    }

    private void Update()
    {
        if (IsBuildModeActive == false)
            return;

        var position = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _ground))
        {
            _placement = hit.point;
            _builderPreview.UpdatePosition(_placement);

            bool isBlocked = Physics.OverlapSphereNonAlloc(hit.point, _config.RequiredSpaceRadius, _objectsInBuildingRadius, _buildingObstacles) > 0;
            bool isWithinMap = _mapAreaConfig.Contains(_placement);

            _isValidPosition = isBlocked == false && isWithinMap;
            _builderPreview.UpdateMaterial(_isValidPosition);
        }
    }

    public void Initialise(BaseRuntime runtime, BaseCreator factory, BaseConfig config)
    {
        _base = runtime;
        _config = config;
        _creator = factory;
    }

    public void Interact()
    {
        if (IsBuildModeActive == false)
            EnterBuildMode();
    }

    public void HandlePlacementClick()
    {
        if (_isValidPosition)
        {
            SetFlag(_placement);
            ExitBuildMode();
        }
    }

    public void EnterBuildMode()
    {
        if (IsBuildModeActive)
        {
            ExitBuildMode();
            return;
        }

        IsBuildModeActive = true;
        _placement = transform.position;
        _builderPreview.ShowPreview();
    }

    public void ExitBuildMode()
    {
        if (IsBuildModeActive == false)
            return;

        IsBuildModeActive = false;
        _builderPreview.HidePreview();
    }

    private void BuildBase(Unit builder)
    {
        Base instance = _creator.Create(_flag.transform.position);

        ClearConstructionSite(instance.GetComponent<Collider>());
        instance.Runtime.UnitRegistry.Register(builder);

        _flag.SetActive(false);
        _flag.transform.position = transform.position;

        _isRequesting = false;
    }

    private void ClearConstructionSite(Collider instance)
    {
        float offset = 0.12f;
        float radius = instance.bounds.size.x + offset;
        LayerMask collidersToRemove = ~0;
        Vector3 origin = instance.transform.position;

        Collider[] colliders = Physics.OverlapSphere(origin, radius, collidersToRemove, QueryTriggerInteraction.Collide);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Resource>(out _) || collider.TryGetComponent<Unit>(out _))
            {
                Vector3 direction = origin.DirectionTo(collider.transform.position);

                collider.transform.position = origin + direction * (instance.bounds.extents.magnitude + offset);
            }
        }
    }

    private void SetFlag(Vector3 position)
    {
        _flag.transform.position = position;
        _flag.SetActive(true);

        if (_isRequesting == false)
        {
            ResourceRequest request = new ResourceRequest(_config.BuildingPrice, () => QueuePriority.Normal, OnResourcesReceived);
            _base.RequestResources(request);
            _isRequesting = true;
        }
    }

    private void OnResourcesReceived() => _base.RequestUnits(new UnitRequest(1, QueuePriority.Normal, SendBuilder));

    private void SendBuilder(List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            void OnCompleted()
            {
                unit.TasksCompleted -= OnCompleted;
                BuildBase(unit);
            }

            unit.TasksCompleted += OnCompleted;
            unit.AddTaskToQueue(new ReachTarget(_flag.transform, 0.5f));
            unit.PerformTasks();
        }
    }
}