using Ksc.HR.Domain.Repositories.Rule;

namespace Ksc.HR.Domain.Repositories
{
    public interface IBaseUnitOfwork : IDisposable
    {
        bool IsDisposed
        {
            get;
        }
      
        Task<int> SaveAsync(CancellationToken cancellationToken= default,bool checklog = true);
        int Save(bool checklog = true);
        Task BulkSaveAsync();
        void UpdateRange(IEnumerable<object> Entity);
        void RemoveRange(IEnumerable<object> Entity);
    }
}