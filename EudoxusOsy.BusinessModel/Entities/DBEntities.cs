using Imis.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace EudoxusOsy.BusinessModel
{
    public partial class DBEntities : IUnitOfWork
    {
        partial void OnContextCreated()
        {
            SavingChanges += new EventHandler(DBEntities_SavingChanges);
        }

        private void DBEntities_SavingChanges(object sender, EventArgs e)
        {
            IEnumerable<ObjectStateEntry> addedStateEntries = ObjectStateManager.GetObjectStateEntries(EntityState.Added).Where(se => !se.IsRelationship);
            foreach (ObjectStateEntry stateEntry in addedStateEntries)
            {
                if (stateEntry.Entity is IUserChangeTracking)
                {
                    IUserChangeTracking entity = (IUserChangeTracking)stateEntry.Entity;

                    if (string.IsNullOrEmpty(entity.CreatedBy))
                        entity.CreatedBy = Thread.CurrentPrincipal.Identity.Name;

                    DateTime currentDateTime = DateTime.Now;

                    entity.CreatedAt = currentDateTime;
                    //entity.CreatedAtDateOnly = currentDateTime.Date;
                }
            }

            IEnumerable<ObjectStateEntry> modifiedStateEntries = ObjectStateManager.GetObjectStateEntries(EntityState.Modified).Where(se => !se.IsRelationship);
            foreach (ObjectStateEntry stateEntry in modifiedStateEntries)
            {
                if (stateEntry.Entity is IUserChangeTracking)
                {
                    IUserChangeTracking entity = (IUserChangeTracking)stateEntry.Entity;

                    if (!string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name))
                    {
                        entity.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;
                        entity.UpdatedAt = DateTime.Now;
                    }
                }

                if (stateEntry.Entity is ITrackable)
                {
                    ITrackable entity = (ITrackable)stateEntry.Entity;
                    ChangeSet set = new ChangeSet(Thread.CurrentPrincipal.Identity.Name, DateTime.Now);
                    if (entity.AllowTrackingChanges)
                    {
                        for (int i = 0; i < stateEntry.CurrentValues.FieldCount; i++)
                        {
                            if (!stateEntry.CurrentValues[i].Equals(stateEntry.OriginalValues[i]))
                            {
                                if (stateEntry.OriginalValues.GetName(i) == "UpdatedAt" || stateEntry.OriginalValues.GetName(i) == "UpdatedBy")
                                    continue;

                                set.Changes.Add(new PropertyChange(stateEntry.OriginalValues[i], stateEntry.CurrentValues[i], stateEntry.OriginalValues.GetName(i)));
                            }
                        }
                        if (set.Changes.Count != 0)
                        {
                            Serializer<List<ChangeSet>> ser = new Serializer<List<ChangeSet>>();
                            List<ChangeSet> changes = ser.Deserialize(entity.ValueXML);
                            if (changes == null) changes = new List<ChangeSet>();
                            changes.Add(set);
                            entity.ValueXML = ser.Serialize(changes);
                        }
                    }
                }
            }
        }

        #region IUnitOfWork Members

        void IUnitOfWork.Commit()
        {
            SaveChanges();
        }

        public void MarkAsDeleted(object entity)
        {
            DeleteObject(entity);
        }

        public void MarkAsNew(object entity)
        {
            Type entityType = entity.GetType();

            // Cannot handle Generic Entities
            MetadataWorkspace.LoadFromAssembly(GetType().Assembly);

            EntityType edmType = MetadataWorkspace.GetItems<EntityType>(DataSpace.CSpace).FirstOrDefault(et => et.Name == entityType.Name);

            while (edmType.BaseType != null)
            {
                edmType = (EntityType)edmType.BaseType;
            }

            EntitySetBase entitySetBase = MetadataWorkspace.GetEntityContainer(DefaultContainerName, DataSpace.CSpace)
                                                           .BaseEntitySets
                                                           .FirstOrDefault(bes => bes.ElementType.Name == edmType.Name);
            if (entitySetBase == null)
                throw new NullReferenceException(string.Format("No EntitySet was found that contains the given EntityType '{0}'", edmType.FullName));

            AddObject(entitySetBase.Name, entity);
        }

        #endregion
    }
}