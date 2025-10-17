using Ksc.HR.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using KSC.Domain;
using KSC.Identity.Abstractions.Services;
using Ksc.HR.Data.Persistant.Repositories;

namespace Ksc.HR.Data.Persistant
{
    public abstract class UnitOfWork<TDbContext> : IBaseUnitOfwork where TDbContext : DbContext
    {
        private readonly IUserService _userService;

        public UnitOfWork(TDbContext databaseContext, IUserService userService) : base()
        {
            DatabaseContext = databaseContext;
            this._userService = userService;
        }

        protected TDbContext DatabaseContext { get; }

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
                    DatabaseContext.Dispose();
                }
            }

            IsDisposed = true;
        }

        //public async Task<int> SaveAsyncWithTransaction(CancellationToken cancellationToken)
        //{
        //    int result = -1;

        //    _dbTransaction.BeginTransaction();
        //    try
        //    {
        //        result =
        //        await DatabaseContext.SaveChangesAsync(cancellationToken);
        //        _dbTransaction.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        _dbTransaction.Rollback();
        //        throw;
        //    }
        //    return result;
        //}

        public async Task<int> SaveAsync(CancellationToken cancellationToken, bool checklog = true)
        {
            int result = -1;

            try
            {
                string authenticateUser = "";
                string authorizeUser = "";
                try
                {
                    authenticateUser = _userService.GetAuthenticateUser();
                    authorizeUser = _userService.GetAuthorizeUser();
                }
                catch (Exception)
                {

                    authenticateUser = "";
                    authorizeUser = "";
                }
                if(checklog)
                result = this.DatabaseContext.Log(authenticateUser, authorizeUser);

                await DatabaseContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                throw;
            }


            return result;
        }

        public async Task BulkSaveAsync()
        {
            try
            {

                await DatabaseContext.BulkSaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void BulkSave()
        {
            try
            {
                DatabaseContext.BulkSaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateRange(IEnumerable<object> Entity)
        {
            DatabaseContext.UpdateRange(Entity);
        }
        public void RemoveRange(IEnumerable<object> Entity)
        {
            DatabaseContext.RemoveRange(Entity);
        }

        public int Save(bool checklog = true)
        {
            int result = -1;
            string authenticateUser = "";
            string authorizeUser = "";
            try
            {
                authenticateUser = _userService.GetAuthenticateUser();
                authorizeUser = _userService.GetAuthorizeUser();
            }
            catch (Exception)
            {

                authenticateUser = "";
                authorizeUser = "";
            }
            try
            {
                if(checklog)
                result = this.DatabaseContext.Log(authenticateUser, authorizeUser);

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
