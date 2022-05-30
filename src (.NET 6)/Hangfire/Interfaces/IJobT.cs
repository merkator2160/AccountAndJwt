namespace Hangfire.Interfaces
{
    public interface IJob<T>
    {
        void Execute(T parameter);
    }
}