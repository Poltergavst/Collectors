using System;

public class ResourceStorage
{
    public int ResourceCount { get; private set; } = 0;

    public event Action<int> ResourcesCountChanged;

    public void GainResources(int amount)
    {
        ResourceCount += amount;
        ResourcesCountChanged?.Invoke(ResourceCount);
    }

    public void SpendResources(int amount)
    {
        if (amount <= ResourceCount)
        {
            ResourceCount -= amount;
            ResourcesCountChanged?.Invoke(ResourceCount);
        }
    }
}