using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _botsCounter;
    [SerializeField] private TMP_Text _resoucreCounter;
    [SerializeField] private Base _base;

    private void OnEnable()
    {
        _base.UnitsChanged += UpdateBots;
        _base.ResourcesChanged += UpdateResource;
    }

    private void OnDisable()
    {
        _base.UnitsChanged -= UpdateBots;
        _base.ResourcesChanged -= UpdateResource;
    }

    private void Start()
    {
        _botsCounter.text = $"{_base.BotsCount}/{_base.BotsCount}";
        _resoucreCounter.text = $"{_base.ResourceCounter}";
    }

    private void UpdateBots(int value)
    {
        _botsCounter.text = $"{value}/{_base.BotsCount}";
    }

    private void UpdateResource(int value)
    {
        _resoucreCounter.text = $"{value}";
    }
}
