using UnityEngine;

[RequireComponent(typeof(UnitSpawner), typeof(BaseBuilder), typeof(SphereCollider))]
public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Base _basePrefab;
    [SerializeField] private BaseConfig _baseConfig;
    [SerializeField] private BaseBuilder _baseBuilder;
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private SpawnpointsProvider _spawnpointsProvider;

    private BaseCreator _creator;
    private UnitSpawner _spawner;
    private SharedBaseServices _shared;
    private ResourceRepository _repository;

    public SphereCollider Collider { get; private set; }
    public BaseRuntime Runtime { get; private set; }
    public UnitsRegistry UnitRegistry => Runtime.UnitRegistry;
    public ResourceStorage ResourceStorage => Runtime.ResourceStorage;

    private void Awake()
    {
        UnitsRegistry registry = new ();
        ResourceStorage storage = new ();
        ResourceGatherer gatherer = new ();
        ResourceDistributor distributor = new (storage);

        _spawner = GetComponent<UnitSpawner>();
        _baseBuilder = GetComponent<BaseBuilder>();
        Collider = GetComponent<SphereCollider>();

        _spawner.Initialise(registry);

        UnitsConveyor conveyor = new (_spawner, distributor, registry, _baseConfig.CreationTime, _baseConfig.CreationPrice);

        Runtime = new(this, conveyor, gatherer, distributor, registry, storage);
    }

    private void OnEnable() => _scanner.ScanPerformed += Runtime.OnScanned;

    private void OnDisable() => _scanner.ScanPerformed -= Runtime.OnScanned;

    private void Start()
    {
        if (_shared == null)
        {
            _spawner.SpawnSeveral(_baseConfig.InitialUnitCount);

            _repository = new();
            ConfigureSharedServices(new SharedBaseServices(_inputReader, _basePrefab, _spawnpointsProvider, _baseConfig, _repository));
        }

        Runtime.SetRepository(_shared.Repository);
        Runtime.StartUnitProduction();           
    }
    
    private void Update() => Runtime.Tick();

    private void OnDestroy() => Runtime.Dispose();

    public void ConfigureSharedServices(SharedBaseServices shared)
    {
        _shared = shared;
        _creator ??= new BaseCreator(_shared);
        _baseBuilder.Initialise(Runtime, _creator, _shared.Config);
    }
}
