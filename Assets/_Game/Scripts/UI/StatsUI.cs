using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Base _homeBase;
    [SerializeField] private TMP_Text _unitsCounter;
    [SerializeField] private TMP_Text _resoucreCounter;

    private void Start()
    {
        _homeBase.UnitRegistry.UnitsCountChanged += UpdateBots;
        _homeBase.ResourceStorage.ResourcesCountChanged += UpdateResource;

        UpdateBots(_homeBase.UnitRegistry.Capacity);
    }

    private void OnDisable()
    {
        _homeBase.UnitRegistry.UnitsCountChanged -= UpdateBots;
        _homeBase.ResourceStorage.ResourcesCountChanged -= UpdateResource;
    }

    private void UpdateBots(int value)
    {
        _unitsCounter.text = $"{value}/{_homeBase.UnitRegistry.Capacity}";
    }

    private void UpdateResource(int value)
    {
        _resoucreCounter.text = $"{value}";
    }
}
