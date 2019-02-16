using Lichen.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public class Scene
    {
        Entity root;
        Dictionary<string, EntityList> entityGroups;
        //public bool Active { get; set; } = true;
        //public bool Visible { get; set; } = true;
        List<string> updateChains;

        public Scene(List<string> chains = null)
        {
            //root = new Entity();
            entityGroups = new Dictionary<string, EntityList>();
            if (chains == null) updateChains = new List<string>();
            else updateChains = chains;
        }

        public Entity GetEntity()
        {
            return root;
        }

        public void SetEntity(Entity entity)
        {
            root = entity;
        }

        public Scene AddUpdateChain(string chain)
        {
            updateChains.Add(chain);
            return this;
        }

        public List<string> GetUpdateChains()
        {
            return updateChains;
        }

        public void Render(Entity sceneHost)
        {
            // Set root's parent to scene's host (temporary one-way relationship, as host does not have root as a child).
            // This allows root to inherit visibility and position from the scene's host.
            root.Parent = sceneHost;
            root.Render();
        }

        public void Update(Entity sceneHost)
        {
            // Set root's parent to scene's host (temporary one-way relationship, as host does not have root as a child).
            // This allows root to inherit visibility and position from the scene's host.
            root.Parent = sceneHost;
            root.Update();
            // Notice how the scene runs its update chains independently from any update chains outside of the scene.
            // So take care when you have scenes embedded within scenes.
            foreach (string chain in updateChains)
            {
                root.Update(chain);
            }
        }

        // Creates a new group if necessary.
        public EntityList GetGroup(string groupName)
        {
            EntityList list;
            if (!entityGroups.TryGetValue(groupName, out list))
            {
                list = new EntityList();
                entityGroups.Add(groupName, list);
            }
            return list;
        }

        public void CreateGroup(string groupName, Entity entity)
        {

        }

        public void AddToGroup(string groupName, Entity entity)
        {

        }

        /*
        // If list already exists, returns that list instead of making a new one.
        // TODO: How to handle differing maxSize values?
        public EntityList CreateTag(string name, int maxSize = 0)
        {
            EntityList list;
            if (!tagList.TryGetValue(name, out list))
            {
                list = new EntityList(maxSize);
                tagList.Add(name, list);
            }
            return list;
        }
        */
    }

    //class EntityTagPair

    public class EntityList
    {
        List<Entity> list;
        List<Entity> addList;
        int maxSize;

        public EntityList(int maxSize = 0)
        {
            this.maxSize = maxSize;
        }

        // Only tries to add entity onto end of list.
        // Returns false if the list is full.
        public bool AppendEntity(Entity entity)
        {
            if (maxSize <= 0 || list.Count < maxSize)
            {
                list.Add(entity);
                return true;
            }
            return false;
        }

        //public bool InsertEntity
    }
}
