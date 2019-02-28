using Lichen.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lichen.Entities
{
    public class Scene
    {
        public string Name { get; set; }
        Entity root;
        public Entity Root { get { return root; } }
        Dictionary<string, EntityGroup> entityGroups;
        //public bool Active { get; set; } = true;
        //public bool Visible { get; set; } = true;
        Dictionary<string, List<System>> systemChains;
        List<string> updateChains;
        List<string> renderChains;
        //List<System> systems;
        public Util.Databank Data { get; set; }

        Dictionary<Type, ComponentGroup> componentGroups = new Dictionary<Type, ComponentGroup>();
        public ComponentGroup<T> GetComponentGroup<T>() where T : Component
        {
            if (componentGroups.TryGetValue(typeof(T), out ComponentGroup group))
            {
                return (ComponentGroup<T>)group;
            }
            ComponentGroup<T> group2 = new ComponentGroup<T>();
            componentGroups.Add(typeof(T), group2);
            return group2;
        }

        public Scene(string sceneName = null)//, List<string> chains = null)
        {
            Name = sceneName;
            root = new Entity(this);
            entityGroups = new Dictionary<string, EntityGroup>();
            //if (chains == null) updateChains = new List<string>();
            //else updateChains = chains;
            updateChains = new List<string>();
            renderChains = new List<string>();
            systemChains = new Dictionary<string, List<System>>();
        }

        //public Scene(List<string> chains = null) : this(null, chains) { }

        public Entity GetEntity()
        {
            return root;
        }

        /*
        public void SetEntity(Entity entity)
        {
            root = entity;
        }
        */

        /*
        public Scene AddUpdateChain(string chain)
        {
            updateChains.Add(chain);
            return this;
        }

        public List<string> GetUpdateChains()
        {
            return updateChains;
        }
        */

        public void AddSystem(System system, string chain)
        {
            if (!systemChains.TryGetValue(chain, out List<System> systems))
            {
                systems = new List<System>();
                systemChains.Add(chain, systems);
            }
            systems.Add(system);
        }

        public void AddUpdateChain(string chain)
        {
            if (!systemChains.ContainsKey(chain)) systemChains.Add(chain, new List<System>());
            updateChains.Add(chain);
        }

        public void AddRenderChain(string chain)
        {
            if (!systemChains.ContainsKey(chain)) systemChains.Add(chain, new List<System>());
            renderChains.Add(chain);
        }

        // TODO: Replace this with a less ridgid way to change scenes - something that supports dynamic loading and unloading.
        // Maybe via delegates/actions? Like "myScene.OnLoad = delegate"
        /*
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
        */

        /*
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
    */

        public void Update(string chain)
        {
            List<System> systems = systemChains[chain];
            foreach(System system in systems)
            {
                system.Update(this);
            }
        }

        public void Update()
        {
            foreach (string chain in updateChains)
            {
                Update(chain);
            }
        }

        public void Render()
        {
            root.UpdateCumulativePosition();
            foreach (string chain in renderChains)
            {
                Update(chain);
            }
        }
        
        // Creates a new group if necessary.
        // TODO: In some cases, wouldn't it be better to fail than to silently return null?
        //       Well, I guess it can't be helped since groups can exist before the scene even does.
        private EntityGroup GetEntityGroup(string groupName)
        {
            EntityGroup list;
            if (!entityGroups.TryGetValue(groupName, out list))
            {
                /*
                list = new EntityGroup();
                entityGroups.Add(groupName, list);
                */
                return null;
            }
            return list;
        }

        // TODO: Misleading name sounds like it's getting the list of groups instead of the list within the group.
        public List<Entity> GetGroup(string groupName)
        {
            EntityGroup group = GetEntityGroup(groupName);
            if (group == null) return null;
            return group.GetList();
        }

        public Entity GetEntity(string entityName)
        {
            List<Entity> group = GetGroup(entityName);
            if (group != null) return group.FirstOrDefault();
            return null;
        }

        public EntityGroup CreateGroup(string groupName, int maxSize = 0)
        {
            EntityGroup group;

            // If group already exists, just return the handle to it.
            // TODO: How to handle differing maxSize values?
            if (entityGroups.TryGetValue(groupName, out group)) return group;

            group = new EntityGroup(maxSize);
            entityGroups.Add(groupName, group);
            return group;
        }

        // Creates a new group if neccessary.
        public void AddToGroup(string groupName, Entity entity)
        {
            EntityGroup group;
            if (!entityGroups.TryGetValue(groupName, out group))
            {
                group = CreateGroup(groupName);
            }
            group.AppendEntity(entity);
            return;
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

        public List<string> DebugGetGroups()
        {
            return entityGroups.Keys.ToList();
        }

        public void DebugPrintOnce(string id, string message)
        {
            Lichen.Util.Error.DebugPrintOnce(Name + ":" + id, Name + ":" + message);
        }

        public void DebugPrint(string message)
        {
            Lichen.Util.Error.DebugPrint(Name + ":" + message);
        }
    }

    //class EntityTagPair

    public class EntityGroup
    {
        List<Entity> list = new List<Entity>();
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

        public List<Entity> GetList()
        {
            return list;
        }
    }
}
