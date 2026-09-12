public class ResourceDistributor : RequestMaker<ResourceRequest>
{
    private ResourceStorage _resources;

    public ResourceDistributor(ResourceStorage resources) : base()
    {
        _resources = resources;
    }

    public bool TryGetResources(int amount)
    {
        if (IsEnough(amount))
        {
            _resources.SpendResources(amount);
            return true;
        }

        return false;
    }

    public override void MakeRequest(ResourceRequest request) => AddToRequests(request);

    protected override bool IsEnough(int required) => _resources.ResourceCount >= required;

    protected override bool TryFulfill(ResourceRequest request)
    {
        if (TryGetResources(request.Required))
        {
            request.Callback?.Invoke();
            return true;
        }

        return false;
    }
}
