using KSC.Infrastructure.Persistance;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities.Log;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Repositories.Log;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Reflection;
using Ksc.HR.Share.Model.Log;

namespace Ksc.HR.Data.Persistant.Repositories
{
    public partial class LogDataRepository : EfRepository<LogData, long>, ILogDataRepository
    {
        private readonly KscHrContext _kscHrContext;

        public LogDataRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public string GetLogJson<T>(T source) //where T : IEntityBase
        {
            return JsonConvert.SerializeObject(source, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        public T GetModelFromJson<T>(string sourceJson) //where T : IEntityBase
        {
            var result = JsonConvert.DeserializeObject<T>(sourceJson);
            return result;
        }
        public async Task<bool> SaveLog<T>(T entity, int logType, string authenticateUser = null, string authorizeUser = null)
        {
            return await Task.Run(() =>
            {
                PropertyInfo propertyInfo = entity.GetType().GetProperty("Id");
                string entityId = "";
                if (propertyInfo != null)
                {
                    entityId = propertyInfo.GetValue(entity).ToString();
                }
                int entityTypeId = _kscHrContext.EntityTypes.First(x => x.Title == entity.GetType().Name).Id;
                var log = new LogData
                {
                    InsertDate = DateTime.Now,
                    OperationTypeId = logType,
                    EntityId = Convert.ToInt64(entityId),
                    EntityTypeId = entityTypeId,
                    AuthenticateUser = authenticateUser,
                    AuthorizeUser = authorizeUser,
                    Value = GetLogJson<T>(entity)
                };
                _kscHrContext.LogDatas.Add(log);
                return true;
            }, new CancellationToken());
        }
        //public async Task<bool> SaveOrderLog(long Id, EnumLogType logType)
        //{
        //    return await Task.Run(async () =>
        //    {
        //        var order = _unitOfWork.OrderHeaderRepository.GetOneWithRelatedForLog(Id).GetAwaiter().GetResult();
        //        await SaveLog<Domain.Order.Entities.OrderHeader>(order, logType);
        //        await _unitOfWork.SaveAsync(new CancellationToken());

        //        return true;
        //    }, new CancellationToken());
        //}
        //public void InsertLog(string authenticateUser = "", string authorizeUser = "")
        //{
        //    var changesInfo = _kscHrContext.ChangeTracker.Entries().Where(t =>
        //    t.State == EntityState.Modified ||
        //    t.State == EntityState.Added ||
        //    t.State == EntityState.Deleted)
        //    .Select(x => new
        //    {
        //        //x.CurrentValues,
        //        OriginalValues = x.OriginalValues.Properties.ToDictionary(pn => pn, pn => x.OriginalValues[pn]),
        //        CurrentValues = x.CurrentValues.Properties.ToDictionary(pn => pn, pn => x.CurrentValues[pn]),
        //        EntityName = x.Entity.GetType().Name,
        //        x.Entity,
        //        x.State,
        //        propertyInfo = x.Entity.GetType().GetProperty("Id"),
        //        EntityValue = JsonConvert.SerializeObject(x.Entity),
        //        //OriginalValues = JsonConvert.SerializeObject(x.OriginalValues),
        //        //x.GetDatabaseValues
        //    }).ToList();
        //    var addedEntities = changesInfo.Where(x => x.State == EntityState.Added).ToList();
        //    if (addedEntities.Count > 0)
        //    {
        //        _kscHrContext.SaveChanges();
        //        changesInfo = addedEntities;
        //    }
        //    //if (changesInfo.Any(x => x.OriginalValues.Any(a=>a.Value) != x.CurrentValues))
        //    //{
        //    foreach (var item in changesInfo)
        //    {
        //        if (_kscHrContext.EntityTypes.Any(x => x.Title == item.EntityName.Trim()))
        //        {
        //            int entityTypeId = _kscHrContext.EntityTypes.First(x => x.Title == item.EntityName.Trim()).Id;
        //            long entityId = 0;
        //            if (item.propertyInfo != null)
        //            {
        //                entityId = Convert.ToInt64(item.propertyInfo.GetValue(item.Entity).ToString());
        //            }
        //            var orginalvalue = item.OriginalValues.Where(x=>x.Key.Name!= "RowVersion").ToDictionary(x => x.Key.Name, x => (x.Value != null ? x.Value.ToString() : null));
        //            var currentvalue = item.CurrentValues.Where(x => x.Key.Name != "RowVersion").ToDictionary(x => x.Key.Name, x => (x.Value != null ? x.Value.ToString() : null));
        //            var checkchanges = true;
        //            if (orginalvalue == currentvalue && item.State == EntityState.Modified)
        //                checkchanges = false;
        //            int operationTypeId = (int)item.State;
        //            if (checkchanges)
        //            {
        //                var model = new LogData()
        //                {
        //                    EntityId = entityId,
        //                    AuthenticateUser = authenticateUser,
        //                    AuthorizeUser = authorizeUser,
        //                    EntityTypeId = entityTypeId,
        //                    Value = JsonConvert.SerializeObject(orginalvalue) /*JsonConvert.SerializeObject(value)*/,
        //                    //Value = GetLogJson(item.Entity) /*JsonConvert.SerializeObject(value)*/,
        //                    InsertDate = DateTime.Now,
        //                    OperationTypeId = operationTypeId
        //                };
        //                _kscHrContext.LogDatas.AddAsync(model);
        //            }
        //            //if (entityId <= 0)
        //        }
        //    }
        //    _kscHrContext.SaveChanges();
        //    //}
        //}

        public IQueryable<LogData> GetAllRelated()
        {
            return _kscHrContext.LogDatas.Include(a => a.EntityType).Include(a => a.OperationType).AsQueryable();
        }
    }
}

