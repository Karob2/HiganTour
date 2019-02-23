using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    /*
    public class EntityProvider
    {
        //List<EntityQueueItem> queue;
        List<Entity> queue;

        public EntityProvider()
        {
            queue = new List<Entity>();
        }

        public Entity NewEntity()
        {
            Entity entity = new Entity();
            entity.State = EntityState.Disabled;
            entity.NextState = EntityState.Enabled;
            queue.Add(entity);
            return entity;
        }

        public void EnableEntity(Entity entity)
        {
            if (entity.State != EntityState.Disabled) return;

            entity.State = EntityState.Creating;
            queue.Add(entity);
            return;
        }

        public void DisableEntity(Entity entity)
        {
            if (entity.State == EntityState.Disabling) return;
            // Delete takes presedence over disable.
            if (entity.State == EntityState.Deleting) return;

            entity.State = EntityState.Disabling;
            queue.Add(entity);
            return;
        }

        public void DeleteEntity(Entity entity)
        {
            if (entity.State == EntityState.Deleting) return;

            entity.State = EntityState.Deleting;
            queue.Add(entity);
            return;
        }

        public void ProcessQueue()
        {
            foreach (Entity entity in queue)
            {
                switch (entity.State)
                {
                    case EntityState.Creating:
                        entity.State = EntityState.Created;
                        break;
                    case EntityState.Disabling:
                        entity.State = EntityState.Disabled;
                        break;
                    case EntityState.Deleting:
                        entity.RemoveFromGroups();
                        entity.State = EntityState.Deleted;
                        break;
                }
            }
            queue.Clear();
        }
    }
    */

    /*
    public class EntityQueueItem
    {
        Entity entity;
        EntityState state;

        EntityQueueItem()
        {

        }
    }
    */
}
