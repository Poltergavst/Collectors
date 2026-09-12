using System.Linq;
using System.Collections.Generic;

public abstract class RequestMaker<T> where T: IRequest
{
    protected List<T> Requests;

    protected RequestMaker()
    {
        Requests = new List<T>();
    }

    public abstract void MakeRequest(T request);

    public void ProcessRequests()
    {
        foreach (var request in Requests)
        {
            if (TryFulfill(request))
                RemoveFromRequests(request);

            break;
        }
    }

    protected abstract bool IsEnough(int amount);

    protected abstract bool TryFulfill(T request);

    protected void AddToRequests(T request)
    {
        Requests.Add(request);
        Requests = Requests.OrderBy(request => request.Priority).ToList();
    }

    protected void RemoveFromRequests(T request) => Requests.Remove(request);
}