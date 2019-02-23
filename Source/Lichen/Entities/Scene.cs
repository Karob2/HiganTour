using Lichen.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public class Scene
    {
        public string Name { get; set; }
        Entity root;
        Dictionary<string, EntityGroup> entityGroups;
        //public bool Active { get; set; } = true;
        //public bool Visible { get; set; } = true;
        List<string> updateChains;

        public Scene(string sceneName, List<string> chains = null)
        {
            Name = sceneName;
            //root = new Entity();
            entityGroups = new Dictionary<string, EntityGroup>();
            if (chains == null) updateChains = new List<string>();
            else updateChains = chains;
        }

        public Scene(List<string> chains = null) : this(null, chains) { }

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

        // TODO: Replace this with a less ridgid way to change scenes - something that supports dynamic loading and unloading.
        // Maybe via delegates/actions? Like "myScene.OnLoad = delegate"
        public bool ChangeScene(string sceneName)
        {
            bool wasChanged = false;
            LinkedList<Entity> scenes;
            if (root.Parent != null)
            {
                scenes = root.Parent.Children;
            }
            else
            {
                // If the calling scene has no parent, then their are no siblings to change scene to, so just check the calling scene itself.
                scenes = new LinkedList<Entity>();
                scenes.AddLast(root);
            }
            foreach (Entity entity in root.Parent.Children)
            {
                if (entity.Scene != null)
                {
                    if (entity.Scene.Name == sceneName)
                    {
                        if (entity.State != EntityState.Enabled) wasChanged = true;
                        entity.State = EntityState.Enabled;
                    }
                    else
                    {
                        if (entity.State != EntityState.Disabled) wasChanged = true;
                        entity.State = EntityState.Disabled;
                    }
                }
                // TODO: Throw error if sceneName was not found.
            }
            return wasChanged;
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
        public EntityGroup GetGroup(string groupName)
        {
            EntityGroup list;
            if (!entityGroups.TryGetValue(groupName, out list))
            {
                list = new EntityGroup();
                entityGroups.Add(groupName, list);
            }
            return list;
        }

        public EntityGroup CreateGroup(string groupName, int maxSize)
        {
            EntityGroup group;

            // If group already exists, just return the handle to it.
            // TODO: How to handle differing maxSize values?
            if (entityGroups.TryGetValue(groupName, out group)) return group;

            group = new EntityGroup(maxSize);
            entityGroups.Add(groupName, group);
            return group;
        }

        public bool AddToGroup(string groupName, Entity entity)
        {
            EntityGroup group;
            if (!entityGroups.TryGetValue(groupName, out group)) return false;
            group.AppendEntity(entity);
            return true;
        }

        public void RemoveFromGroup(string groupName, Entity entity)
        {
            EntityGroup group;
            if (!entityGroups.TryGetValue(groupName, out group)) return;
            group.RemoveEntity(entity);
        }

        /*
        // If list already exists, returns that list instead of making a new one.
        // TODO: How to handle differing maxSize values?
        public EntityGroup CreateTag(string name, int maxSize = 0)
        {
            EntityGroup list;
            if (!tagList.TryGetValue(name, out list))
            {
                list = new EntityGroup(maxSize);
                tagList.Add(name, list);
            }
            return list;
        }
        */
    }

    //class EntityTagPair

    public class EntityGroup
    {
        List<Entity> list;
        //List<Entity> addList;
        int maxSize;

        public EntityGroup(int maxSize = 0)
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

        public void RemoveEntity(Entity entity)
        {
            list.Remove(entity);
        }

        //public bool InsertEntity
    }
}
