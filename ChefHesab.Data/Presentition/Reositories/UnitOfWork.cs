using ChefHesab.Data.Presentition.Context;
using ChefHesab.Domain.Peresentition.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ChefHesab.Data.Presentition.Reositories
{
    public abstract class UnitOfWork<TDbContext> : IBaseUnitOfWork where TDbContext : DbContext
    {
        protected ChefHesabContext DatabaseContext { get; }

        public UnitOfWork(ChefHesabContext context) : base()
        { 
            DatabaseContext = context;
        }


        public bool IsDisposed { get; protected set; }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {

                if (DatabaseContext != null)
                {
                    //DatabaseContext.Database.CloseConnection();

                    DatabaseContext.Dispose();
                }
            }

            IsDisposed = true;
        }

        public async Task<int> SaveAsync()
        {
            int result = -1;

            try
            {
                result =
                    await DatabaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }


            return result;
        }


      

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
