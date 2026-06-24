using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _unitsCounter;
    [SerializeField] private TMP_Text _resoucreCounter;
    [SerializeField] private Base _base;

    private void OnEnable()
    {
        _base.UnitsCountChanged += UpdateBots;
        _base.ResourcesCountChanged += UpdateResource;
    }

    private void OnDisable()
    {
        _base.UnitsCountChanged -= UpdateBots;
        _base.ResourcesCountChanged -= UpdateResource;
    }

    private void Start()
    {
        _unitsCounter.text = $"{_base.UnitsCapacity}/{_base.UnitsCapacity}";
        _resoucreCounter.text = $"{_base.ResourceAvailable}";
    }

    private void UpdateBots(int value)
    {
        _unitsCounter.text = $"{value}/{_base.UnitsCapacity}";
    }

    private void UpdateResource(int value)
    {
        _resoucreCounter.text = $"{value}";
    }
}
