using System.Collections.Generic;

public class ResourceRepository
{
    private HashSet<IPickable> _freeResources;
    private HashSet<IPickable> _takenResources;
    public IReadOnlyCollection<IPickable> FreeResources => _freeResources;

    public ResourceRepository()
    {
        _freeResources = new HashSet<IPickable>();
        _takenResources = new HashSet<IPickable>();
    }

    public void Add(IPickable pickable)
    {
        if (_takenResources.Contains(pickable) == false)
            _freeResources.Add(pickable);
    }

    public void AddToTaken(IPickable pickable)
    {
        _takenResources.Add(pickable);
        _freeResources.Remove(pickable);
    }

    public void FreeResource(IPickable pickable) => _takenResources.Remove(pickable);
}
