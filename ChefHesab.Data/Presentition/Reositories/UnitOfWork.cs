using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Domain.Peresentition.IRepositories;
using Dalir.common.Context;
using Dalir.common.Interfaces;
using Dalir.common.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChefHesab.Data.Presentition.Reositories
{
    public abstract  class UnitOfWork<TDbContext> : IBaseUnitOfWork where TDbContext : DbContext
    {
        protected TDbContext DatabaseContext { get; }
        public UnitOfWork(TDbContext databaseContext)
        {
            DatabaseContext = databaseContext;
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

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            int result = -1;

            try
            {
                result =
                    await DatabaseContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                throw;
            }


            return result;
        }

       
        public int Save()
        {
            int result = -1;

            try
            {
                result =
                    DatabaseContext.SaveChanges();
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
