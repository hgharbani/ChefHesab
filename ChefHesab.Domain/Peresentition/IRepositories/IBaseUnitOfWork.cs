
namespace ChefHesab.Domain.Peresentition.IRepositories
{
    public interface IBaseUnitOfWork :IDisposable
    {
        bool IsDisposed    { get; }

    Task<int> SaveAsync(CancellationToken cancellationToken = default);
    int Save();

}
}
