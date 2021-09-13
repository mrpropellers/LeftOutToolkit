namespace LeftOut.Runtime.Interfaces
{
    public interface INeedsInitialization<in T>
    {
        bool IsInitialized { get; }
        bool TryInitialize(T initializer);
    }
}
