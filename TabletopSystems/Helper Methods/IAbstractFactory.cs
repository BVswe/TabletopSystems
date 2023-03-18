namespace TabletopSystems.Helper_Methods
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }
}