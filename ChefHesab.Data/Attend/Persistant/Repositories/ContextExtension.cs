using Ksc.HR.Domain.Entities.Log;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant
{
    public static class ContextExtension
    {
        public static int Log(this DbContext dbContext, string authenticateUser, string authorizeUser)
        {

            var changesInfo = dbContext.ChangeTracker.Entries().Where(t =>
            t.State == EntityState.Modified ||
            t.State == EntityState.Added ||
            t.State == EntityState.Deleted).ToList()
                .Select(x => new
                {
                    //x.CurrentValues,
                    OriginalValues = x.OriginalValues,
                    CurrentValues = x.CurrentValues,
                    //EntityName = x.Entity?.GetType()?.Name,
                    x.Entity,
                    x.State,
                    //propertyInfo = x.Entity.GetType().GetProperty("Id"),tity),
                    //OriginalValues = JsonConvert.SerializeObject(x.OriginalValues),
                    //x.GetDatabaseValues}
                })
                .ToList();
            var entitynames = dbContext.Set<EntityType>().Where(x => x.IsActive == true).Select(x => x.Title);
            var savedchangesInfo = changesInfo.Where(x => entitynames.Any(a => a == x.Entity.GetType().Name)).ToList();

            var addedEntities = savedchangesInfo.Where(x => x.State == EntityState.Added).ToList();
            if (addedEntities.Count > 0)
            {
                dbContext.SaveChanges();
                //changesInfo = addedEntities;
            }
            //if (changesInfo.Any(x => x.OriginalValues.Any(a=>a.Value) != x.CurrentValues))
            //{
            foreach (var item in savedchangesInfo)
            {
                var EntityName = item.Entity?.GetType()?.Name;
                var OriginalValues = item.OriginalValues?.Properties?.ToDictionary(pn => pn, pn => item.OriginalValues[pn]);
                var CurrentValues = item.CurrentValues?.Properties?.ToDictionary(pn => pn, pn => item.CurrentValues[pn]);
                var propertyInfo = item.Entity.GetType().GetProperty("Id");
                int entityTypeId = dbContext.Set<EntityType>().First(x => x.Title == EntityName.Trim()).Id;
                long entityId = 0;
                if (propertyInfo != null)
                {
                    entityId = Convert.ToInt64(propertyInfo.GetValue(item.Entity).ToString());
                }
                var orginalvalue = OriginalValues.Where(x => x.Key.Name != "RowVersion").ToDictionary(x => x.Key.Name, x => (x.Value != null ? x.Value.ToString() : null));
                var currentvalue = CurrentValues.Where(x => x.Key.Name != "RowVersion").ToDictionary(x => x.Key.Name, x => (x.Value != null ? x.Value.ToString() : null));
                var checkchanges = true;
                var jsonOriginalValues = JsonConvert.SerializeObject(orginalvalue);
                var jsonCurrentValues = JsonConvert.SerializeObject(currentvalue);
                if (orginalvalue == currentvalue && item.State == EntityState.Modified)
                    checkchanges = false;
                int operationTypeId = (int)item.State;
                if (checkchanges)
                {
                    if (item.State == EntityState.Modified)
                    {
                        if (!dbContext.Set<LogData>().Any(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId))
                        {
                            var oldmodel = new LogData()
                            {
                                EntityId = entityId,
                                AuthenticateUser = authenticateUser,
                                AuthorizeUser = authorizeUser,
                                EntityTypeId = entityTypeId,
                                Value = jsonOriginalValues,
                                InsertDate = DateTime.Now,
                                OperationTypeId = operationTypeId
                            };
                            dbContext.Set<LogData>().AddAsync(oldmodel);
                        }
                    }
                    var newmodel = new LogData()
                    {
                        EntityId = entityId,
                        AuthenticateUser = authenticateUser,
                        AuthorizeUser = authorizeUser,
                        EntityTypeId = entityTypeId,
                        Value = item.State == EntityState.Modified ? jsonCurrentValues : jsonOriginalValues, /*JsonConvert.SerializeObject(value),
                        //Value = JsonConvert.SerializeObject(item.Entity),
                        //Value = GetLogJson(item.Entity) /*JsonConvert.SerializeObject(value),*/
                        InsertDate = DateTime.Now,
                        OperationTypeId = operationTypeId
                    };
                    dbContext.Set<LogData>().AddAsync(newmodel);
                }

            }
            return changesInfo.Count();
            //dbContext.SaveChanges();
            //}

        }
    }
}
