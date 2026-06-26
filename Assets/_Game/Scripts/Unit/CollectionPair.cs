public readonly struct CollectionPair
{
    public readonly Unit Unit;
    public readonly IPickable Pickable;
    public readonly float SqrDistance;

    public CollectionPair(Unit unit, IPickable pickable)
    {
        Unit = unit;
        Pickable = pickable;

        SqrDistance = unit.transform.position.SqrDistanceTo(pickable.GetCoordinates());
    }
}
