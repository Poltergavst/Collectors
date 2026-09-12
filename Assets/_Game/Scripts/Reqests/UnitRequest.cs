using System;
using System.Collections.Generic;

public class UnitRequest : IRequest
{
    public int Required { get; private set; }
    public QueuePriority Priority { get; private set; }

    public Action<List<Unit>> Callback { get; set; }

    public UnitRequest(int required, QueuePriority priority, Action<List<Unit>> callback)
    {
        Required = required;
        Priority = priority;
        Callback = callback;
    }
}