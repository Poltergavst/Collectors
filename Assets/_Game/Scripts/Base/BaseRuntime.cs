using UnityEngine;

public class BaseRuntime
{
    private Base _base;
    private UnitsConveyor _conveyor;
    private ResourceGatherer _gatherer;
    private ResourceDistributor _distributor;
    private ResourceRepository _repository;

    public BaseRuntime(Base @base, UnitsConveyor conveyor, ResourceGatherer gatherer, ResourceDistributor resourceDistributor,
        UnitsRegistry unitRegistry, ResourceStorage resourceStorage)
    {
        _base = @base;
        _conveyor = conveyor;
        _gatherer = gatherer;
        _distributor = resourceDistributor;

        UnitRegistry = unitRegistry;
        ResourceStorage = resourceStorage;

        _gatherer.Gathered += OnDelivered;
    }

    public UnitsRegistry UnitRegistry { get; private set; }
    public ResourceStorage ResourceStorage { get; private set; }

    public void StartUnitProduction() => _conveyor.Start();

    public void SetRepository(ResourceRepository repository) => _repository = repository;

    public void Tick()
    {
        _distributor.ProcessRequests();
        UnitRegistry.ProcessRequests();
    }

    public void OnScanned(Collider[] scannedObjects)
    {
        if (_repository == null || scannedObjects.Length == 0)
            return;

        foreach ( var scanned in scannedObjects )
        {
            if (scanned == null)
                continue; 

            if (scanned.gameObject.TryGetComponent(out IPickable pickable))
            {
                _repository.Add(pickable);
            }
        }

        _gatherer.SendGathering(_repository, UnitRegistry, _base.transform, _base.Collider.bounds.extents.x);
    }

    public void RequestUnits(UnitRequest unitRequest) => UnitRegistry.MakeRequest(unitRequest);

    public void RequestResources(ResourceRequest request) => _distributor.MakeRequest(request);

    public void Dispose() => _gatherer.Gathered -= OnDelivered;

    private void OnDelivered(Unit unit, IPickable pickable)
    {
        ResourceStorage.GainResources(1);
        UnitRegistry.Return(unit);
        _repository.FreeResource(pickable);
    }
}