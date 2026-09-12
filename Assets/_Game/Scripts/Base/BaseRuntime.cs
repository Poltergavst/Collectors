using System.Linq;
using UnityEngine;

public class BaseRuntime
{
    private Base _base;
    private UnitsConveyor _conveyor;
    private ResourceGatherer _gatherer;
    private ResourceDistributor _distributor;

    public BaseRuntime(Base @base, UnitsConveyor conveyor, ResourceGatherer gatherer, ResourceDistributor resourceDistributor, UnitsRegistry unitRegistry, ResourceStorage resourceStorage)
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

    public void Tick()
    {
        _distributor.ProcessRequests();
        UnitRegistry.ProcessRequests();
    }

    public void OnScanned(Collider[] scannedObjects)
    {
        if (scannedObjects.Length == 0)
            return;

        var pickables = scannedObjects
            .Select(scanned => scanned != null ? scanned.gameObject.GetComponent<IPickable>() : null)
            .Where(pickable => pickable != null && pickable.IsDetectable)
            .ToList();

        _gatherer.SendGathering(pickables, UnitRegistry, _base.transform, _base.Collider.bounds.extents.x);
    }

    public void RequestUnits(UnitRequest unitRequest) => UnitRegistry.MakeRequest(unitRequest);

    public void RequestResources(ResourceRequest request) => _distributor.MakeRequest(request);

    public void Dispose() => _gatherer.Gathered -= OnDelivered;

    private void OnDelivered(Unit unit, IPickable pickable)
    {
        ResourceStorage.GainResources(1);
        UnitRegistry.Return(unit);
    }
}