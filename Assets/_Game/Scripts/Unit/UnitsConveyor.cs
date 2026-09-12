using System.Collections;
using UnityEngine;

public class UnitsConveyor
{
    private float _spawnCooldown = 0.2f;
    private int _productionPrice;
    private UnitSpawner _spawner;
    private ResourceDistributor _distributor;
    private UnitsRegistry _unitRegistry;

    public UnitsConveyor(UnitSpawner spawner, ResourceDistributor distributor, UnitsRegistry unitRegistry, float productionTime, int productionPrice)
    {
        _spawner = spawner;
        _distributor = distributor;
        _unitRegistry = unitRegistry;
        _spawnCooldown = productionTime;
        _productionPrice = productionPrice;
    }

    public void Start() => RequestUnit();

    private void RequestUnit()
    {
        _distributor.MakeRequest(new ResourceRequest(_productionPrice, () => _unitRegistry.Capacity <= 1 ? QueuePriority.High : QueuePriority.Low, OnResourcesGet));
    }

    private void OnResourcesGet()
    {
        _spawner.StartCoroutine(ProduceUnit());
    }

    private IEnumerator ProduceUnit()
    {
        yield return new WaitForSeconds(_spawnCooldown);
        _spawner.Spawn();

        RequestUnit();
    }
}