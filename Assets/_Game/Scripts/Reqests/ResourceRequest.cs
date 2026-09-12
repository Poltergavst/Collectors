using System;

public class ResourceRequest : IRequest
{
    private Func<QueuePriority> _getPriority;

    public int Required { get; private set; }
    public QueuePriority Priority => _getPriority();

    public Action Callback { get; set; }

    public ResourceRequest(int required, Func<QueuePriority> getPriority, Action callback)
    {
        Required = required;
        _getPriority = getPriority;
        Callback = callback;
    }
}
