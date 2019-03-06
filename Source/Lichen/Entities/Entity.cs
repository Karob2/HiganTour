using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Lichen.Entities
{
    // I want to add a deletion/creation queue to GlobalServices. The problem is that it wouldn't know what groups the entity belongs to. I guess each entity needs to store what groups it belongs to, alongside its tag list (HashSet).
    // Disabling maintains all values, tags, groups. Deleting purges everything and puts an entity in a state where it really can be discarded from memory.
    // The point of disabling is to still allow an asset to be reused, like how I always have 30 or so enemies, but most of them are not in use and can be overwritten.
    public enum EntityState
    {
        Enabled,
        Disabled,
        Deleted
    }

    public enum EntityProperty
    {
        Active,
        Visible,
        Scene,
        State
    }

    public class EntityTemplate
    {
        //public Entity Parent { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public bool Visible { get; set; }
        public int RenderLayer { get; set; }
        public List<Component> Components { get; set; } = new List<Component>();

        public EntityTemplate() { }

        /*
        public EntityTemplate(Entity parent)
        {
            Parent = parent;
        }
        */

        public EntityTemplate SetPosition(float x, float y)
        {
            X = x;
            Y = y;
            return this;
        }

        public EntityTemplate SetVisible(bool visible = true)
        {
            Visible = visible;
            return this;
        }

        public EntityTemplate SetRenderLayer(int layer)
        {
            RenderLayer = layer;
            return this;
        }

        public EntityTemplate AddComponent(Component component)
        {
            Components.Add(component);
            return this;
        }

        public Entity MakeEntity(Entity parent)
        {
            Entity entity = parent.MakeChild();
            entity.X = X;
            entity.Y = Y;
            entity.Visible = Visible;
            entity.RenderLayer = RenderLayer;
            //AttachComponents(entity);
            foreach (Component component in Components)
            {
                component.Clone().AttachTo(entity);
            }
            return entity;
        }

        /*
        public void AttachComponents(Entity entity)
        {
            foreach (Component component in Components)
            {
                AttachComponent(entity, component);
            }
        }

        public void AttachComponent(Entity entity, Component component)
        {
            if (component is SpriteComponent)
            {
                entity.AddComponent<SpriteComponent>
            }
        }
        */
    }

    public class Entity
    {
        private Scene scene;
        public Scene Scene { get { return scene; } }
        //public Scene Scene { get { return scene; } set { PropagateScene(value); } }
        //private Scene ownScene;
        //public Scene OwnScene { get { return ownScene; } }
        //public bool IsScene { get; set; }
        private EntityState state = EntityState.Enabled;
        public EntityState State { get { return state; } set { PropagateState(value); } }
        private EntityState nextState;
        public EntityState NextState { get { return nextState; } set { nextState = value; } }
        public bool Enabled { get { return (state == EntityState.Enabled); } }

        HashSet<string> groups = new HashSet<string>();
        HashSet<string> tags = new HashSet<string>();
        public Util.Databank Data { get; set; } = new Util.Databank();

        public Entity Parent { get; set; }
        public LinkedList<Entity> Children { get; set; } = new LinkedList<Entity>();
        public float X { get; set; }
        public float Y { get; set; }
        /*
        private float relativeX;
        private float relativeY;
        public float RelativeX { get { return relativeX; } }
        public float RelativeY { get { return relativeY; } }
        */
        float cumulativeX;
        float cumulativeY;
        public float CumulativeX { get { return cumulativeX; } set { cumulativeX = value; } }
        public float CumulativeY { get { return cumulativeY; } set { cumulativeY = value; } }

        private bool active = true;
        private bool visible = true;
        public bool Active { get { return active; } set { PropagateActivity(value); } }
        public bool Visible { get { return visible; } set { PropagateVisibility(value); } }
        private bool inheritedVisibility = true;
        private bool inheritedActivity = true;
        public bool InheritedVisibility { get { return inheritedVisibility; } }
        public bool InheritedActivity { get { return inheritedActivity; } }
        // UpdateComponent: For components that have code to run every frame, such as updating motion and collisions, and responding to player input.
        // RenderComponent: For components that need to be drawn every frame.
        // ComponentList: For components that do not Update or Render, such as data holders.
        // UpdateChains: Same as Update, but allows lists of multiple components to be ran together.
        /*
        public IUpdateComponent UpdateComponent { get; set; }
        public IRenderComponent RenderComponent { get; set; }
        public Dictionary<Type,Component> ComponentList { get; set; }
        public Dictionary<string,UpdateChain> UpdateChains { get; set; }
        */
        Dictionary<Type, int> components = new Dictionary<Type, int>();
        // To enable full cloning:
        List<Component> componentReferences = new List<Component>();

        /*
        public bool RenderByDepth { get; set; }
        public float RenderOrder { get; set; }
        public int RenderBackupOrder { get; set; }
        public static int RenderBackupCount { get; set; }
        */
        public int RenderLayer { get; set; }
        public float RenderDepth { get; set; }
        public int AutoDepth { get; set; }

        //public List<Entity> ActorList { get; set; }

        // TODO: This fails to remove the entities from the previous scene's groups. Maybe I should never transfer entities across scenes.
        public void PropagateScene(Scene scene)
        {
            if (this.scene == scene) return;
            this.scene = scene;
            RebindGroups();
            PropagateProperty(EntityProperty.Scene);
        }

        public void PropagateState(EntityState state)
        {
            if (this.state == state) return;
            this.state = state;
            PropagateProperty(EntityProperty.State);
        }

        public void PropagateActivity(bool active)
        {
            if (this.active == active) return;
            this.active = active;
            PropagateProperty(EntityProperty.Active);
        }

        public void PropagateVisibility(bool visible)
        {
            if (this.visible == visible) return;
            this.visible = visible;
            PropagateProperty(EntityProperty.Visible);
        }

        public void PropagateAll()
        {
            PropagateProperty(EntityProperty.Scene);
            PropagateProperty(EntityProperty.State);
            PropagateProperty(EntityProperty.Active);
            PropagateProperty(EntityProperty.Visible);
        }

        // Only propagate to a specific child.
        public void PropagateAll(Entity child)
        {
            //if (IsScene) return; //probably not necessary since the caller already checks
            foreach (EntityProperty property in Enum.GetValues(typeof(EntityProperty)))
            {
                if (child.InheritProperty(property))
                {
                    child.PropagateProperty(property);
                }
            }
        }

        // Recursively set all descendants to have the same property value.
        public void PropagateProperty(EntityProperty property)
        {
            //if (IsScene) return;
            if (Children.Count != 0)
            {
                LinkedListNode<Entity> child = Children.First;
                while (child != null)
                {
                    // Only propagate if the child was changed.
                    if (child.Value.InheritProperty(property))
                    {
                        child.Value.PropagateProperty(property);
                    }
                    child = child.Next;
                }
            }
        }

        // Returns false if the property was already equal to the parent's property.
        public bool InheritProperty(EntityProperty property)
        {
            bool test;

            switch (property)
            {
                case EntityProperty.Scene:
                    if (scene == Parent.Scene) return false;
                    scene = Parent.Scene;
                    RebindGroups();
                    break;
                case EntityProperty.State:
                    if (state == Parent.State) return false;
                    state = Parent.State;
                    break;
                case EntityProperty.Active:
                    test = Parent.Active && Parent.InheritedActivity;
                    if (inheritedActivity == test) return false;
                    inheritedActivity = test;
                    break;
                case EntityProperty.Visible:
                    test = Parent.Visible && Parent.InheritedVisibility;
                    if (inheritedVisibility == test) return false;
                    inheritedVisibility = test;
                    break;
            }
            return true;
        }

        public Entity() { }

        // OLD TODO: Since a scene is now required, remove scene =!= null checks from this class.
        public Entity(Scene scene)
        {
            this.scene = scene;
        }

        /*
        public Entity(Entity parent)
        {
            //jazz
            this.scene = parent.scene;
            AttachTo(parent);
        }
        */

        public Entity MakeChild()
        {
            Entity entity = new Entity(Scene);
            entity.AttachTo(this);
            return entity;
        }

        /*
        public Entity(float x, float y) : this()
        {
            X = x;
            Y = y;
        }

        public Entity(float x, float y, bool active, bool visible) : this()
        {
            X = x;
            Y = y;
            this.active = active;
            this.visible = visible;
        }

        public Entity(bool active, bool visible) : this()
        {
            this.active = active;
            this.visible = visible;
        }
        */

        public Entity SetPosition(float x, float y)
        {
            X = x;
            Y = y;
            return this;
        }

        /*
        public Entity SetActive()
        {
            Active = true;
            return this;
        }
        */
        public Entity SetActive(bool active)
        {
            // The property automatically sets all children's inheritedActivity.
            Active = active;
            return this;
        }

        /*
        public Entity SetVisible()
        {
            Visible = true;
            return this;
        }
        */
        public Entity SetVisible(bool visible)
        {
            // The property automatically sets all children's inheritedVisibility.
            Visible = visible;
            return this;
        }

        /*
        public Entity SetRenderOrder(float renderOrder)
        {
            RenderOrder = renderOrder;
            return this;
        }

        public Entity SetRenderByDepth(bool renderByDepth)
        {
            RenderByDepth = renderByDepth;
            return this;
        }
        */

        public Entity SetRenderOrder(int layer, float depth)
        {
            RenderLayer = layer;
            RenderDepth = depth;
            return this;
        }

        public Entity SetRenderLayer(int renderLayer)
        {
            RenderLayer = renderLayer;
            return this;
        }

        public Entity SetRenderDepth(float renderDepth)
        {
            RenderDepth = renderDepth;
            return this;
        }

        public Entity AddComponent<T>(T component) where T : Component
        {
            // TODO: This doesn't work at all if the entity is not attached to a scene.
            // So either make entities always attached to scenes (and not able to change scenes),
            // Or modify this and also add code in Scene propagation.

            // Give the component a reference back to the entity.
            component.Owner = this;
            componentReferences.Add(component);

            if (scene != null)
            {
                ComponentGroup<T> group = Scene.GetComponentGroup<T>();
                // Add to scene's component group by type.
                group.Add(component, out int componentId);
                // Add to entity's component list.
                components.Add(component.GetType(), componentId);
                component.FilterComponent(); // This leads to downcasting via GetComponentFilter<> whenever an item is successfully filtered. (At the time of this writing, that's once per attach of each RenderComponent inheritor.)
                component.OnAttach();
            }
            /*
            ComponentList.Add(component.GetType(), component);
            component.Owner = this;
            */
            return this;
        }

        public T GetComponent<T>() where T : Component
        {
            if (!components.TryGetValue(typeof(T), out int componentId)) return null;
            return scene.GetComponentGroup<T>().List[componentId];
        }

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            if (!components.TryGetValue(typeof(T), out int componentId))
            {
                component = null;
                return false;
            }
            component = scene.GetComponentGroup<T>().List[componentId];
            return true;
        }

        /*
        public Entity AddUpdateComponent(IUpdateComponent component)
        {
            UpdateComponent = component;
            component.Owner = this;
            return this;
        }

        public Entity AddRenderComponent(IRenderComponent component)
        {
            RenderComponent = component;
            component.Owner = this;
            return this;
        }

        public Entity AddChainComponent(string chain, IUpdateComponent component)
        {
            if (!UpdateChains.ContainsKey(chain)) UpdateChains.Add(chain, new UpdateChain());
            UpdateChains[chain].Add(component);
            component.Owner = this;
            return this;
        }
        */

        /*
    public Entity AddActor(List<Entity> actorList)
    {
        ActorList = actorList;
        actorList.Add(this);
        return this;
    }

    public Entity AddActorList(List<Entity> actorList)
    {
        ActorList = actorList;
        return this;
    }
    */

        // TODO: Decide whether or not to keep both AttachTo and AddChild.

        public Entity AttachTo(Entity entity)
        {
            Debug.Assert(entity.scene == this.scene || this.scene == null, "Child belongs to different scene from parent.");

            Parent = entity;
            entity.Children.AddLast(this);
            // Inherit container from parent.
            //Container = entity.Container;
            //Scene = entity.Scene;
            /*
            if (entity.IsScene)
            {
                this.PropagateScene(entity.OwnScene);
            }
            else
            {
            */
                entity.PropagateAll(this);
            /*
            }
            */
            return this;
        }

        public Entity AddChild(Entity entity)
        {
            Debug.Assert(entity.scene == this.scene || entity.scene == null, "Child belongs to different scene from parent.");

            Children.AddLast(entity);
            entity.Parent = this;
            // Inherit container from parent.
            //entity.Container = Container;
            //entity.Scene = Scene;
            /*
            if (this.IsScene)
            {
                entity.PropagateScene(this.ownScene);
            }
            else
            {
            */
                this.PropagateAll(entity);
            /*
            }
            */
            return this;
        }

        /*
        public Entity MakeScene(string sceneName, Scene scene = null)
        {
            // Set self as container so that children will inherit it.
            //Container = this;
            if (ownScene == null) ownScene = new Scene(sceneName);
            else ownScene = scene;
            ownScene.SetEntity(this);
            IsScene = true;

            if (Children.Count != 0)
            {
                LinkedListNode<Entity> child = Children.First;
                while (child != null)
                {
                    child.Value.PropagateScene(ownScene);
                    child = child.Next;
                }
            }
            return this;
        }
        */

        public Entity AddToGroup(string groupName)
        {
            /*
             * if (groups == null)
            {
                groups = new HashSet<string>();
            }
            else
            {
            */
                // Check if already in group.
                if (groups.Contains(groupName)) return this;
            /*
            }
            */
            groups.Add(groupName);

            if (scene != null) scene.AddToGroup(groupName, this);
            return this;
        }

        // TODO: Replace this with an actual name functionality.
        public Entity SetName(string name)
        {
            AddToGroup(name);
            return this;
        }

        /*
        public void RemoveFromGroups()
        {
            if (groups == null) return;

            foreach (string groupName in groups)
            {
                scene.RemoveFromGroup(groupName, this);
            }
            groups.Clear();
        }
        */

        private void RebindGroups()
        {
            //if (groups == null) return;
            if (scene == null) return;
            foreach (string group in groups)
            {
                scene.AddToGroup(group, this);
            }
            List<Component> refs = componentReferences;
            componentReferences = new List<Component>();
            foreach (Component component in refs)
            {
                component.AttachTo(this);
            }
        }

        public bool HasGroup(string groupName)
        {
            return groups.Contains(groupName);
        }

        // TODO: Possibly convert tag system to bool/flag system. I'm not sure which is better under-the-hood.
        // Welp, currently I have both, and they have separate roles.
        public Entity AddTag(string tagName)
        {
            //if (tags == null) tags = new HashSet<string>();
            tags.Add(tagName);
            return this;
        }

        public Entity RemoveTag(string tagName)
        {
            //if (tags == null) return this;
            tags.Remove(tagName);
            return this;
        }

        public bool HasTag(string tagName)
        {
            //if (tags == null) return false;
            return tags.Contains(tagName);
        }

        /*
        public int GetInt(string name)
        {
            if (integers == null) return 0;
            return integers.TryGetValue(name, out int output) ? output : 0;
        }

        public void SetInt(string name, int value)
        {
            if (integers == null)
            {
                integers = new Dictionary<string, int>();
                integers.Add(name, value);
            }
            else
            {
                integers[name] = value;
            }
        }
        */

        // Even if Visible = true, the entity might be invisible due to an invisible parent, hence InheritedVisibility.
        // This is basically just a rename of InheritedVisibility, so I'm removing it for now.
        // TODO: I could rename inherited to this - it's much more... uh, nice.
        /*
        public bool IsVisible()
        {
            return inheritedVisibility;
        }
        */
        
        public void UpdateCumulativePosition(int counter = 0)
        {
            if (!Enabled) return;
            if (!Visible) return;

            if (Parent != null)
            {
                cumulativeX = Parent.CumulativeX + X;
                cumulativeY = Parent.CumulativeY + Y;
            }
            else
            {
                cumulativeX = X;
                cumulativeY = Y;
            }
            AutoDepth = counter;
            if (Children.Count != 0)
            {
                LinkedListNode<Entity> child = Children.First;
                while (child != null)
                {
                    counter++;
                    child.Value.UpdateCumulativePosition(counter);
                    child = child.Next;
                }
            }
        }

        /*
        public void OverrideCumulativePosition(float x, float y)
        {
            cumulativeX = x;
            cumulativeY = y;
        }
        */

        /*
        public void Render()
        {
            if (!Enabled) return;
            if (!Visible) return;

            if (RenderByDepth)
            {
                List<Entity> renderList = new List<Entity>();
                RenderBackupCount = 0;
                this.BuildSortedRenderList(renderList);
                renderList.Sort(new EntitySorter());
                foreach (Entity entity in renderList)
                {
                    entity.RenderComponent.Render();
                }
                return;
            }

            if (Parent != null)
            {
                relativeX = Parent.RelativeX + X;
                relativeY = Parent.RelativeY + Y;
            }
            else
            {
                relativeX = X;
                relativeY = Y;
            }
            if (RenderComponent != null) RenderComponent.Render();
            if (Children.Count != 0)
            {
                LinkedListNode<Entity> child = Children.First;
                while (child != null)
                {
                    child.Value.Render();
                    child = child.Next;
                }
            }
        }

        public void BuildSortedRenderList(List<Entity> renderList)
        {
            if (!Enabled) return;
            if (!Visible) return;

            if (Parent != null)
            {
                relativeX = Parent.RelativeX + X;
                relativeY = Parent.RelativeY + Y;
            }
            else
            {
                relativeX = X;
                relativeY = Y;
            }
            if (RenderComponent != null)
            {
                renderList.Add(this);
                RenderBackupOrder = RenderBackupCount;
                RenderBackupCount++;
                RenderOrder = -Y; // TODO: Should have custom sort order (Z?), not forced to sort by Y.
            }
            if (Children.Count != 0)
            {
                LinkedListNode<Entity> child = Children.First;
                while (child != null)
                {
                    child.Value.BuildSortedRenderList(renderList);
                    child = child.Next;
                }
            }
        }

        // TODO: Having to go through every single Active entity for each update chain is wasteful.
        public void Update(string chain = null)
        {
            if (!Enabled) return;
            if (!Active) return;

            /-*
            if (Parent != null)
            {
                inheritedVisibility = Parent.InheritedVisibility;
            }
            else
            {
                inheritedVisibility = Visible;
            }
            *-/
            if (chain == null)
            {
                if (UpdateComponent != null) UpdateComponent.Update();
            }
            else
            {
                if (UpdateChains != null && UpdateChains.ContainsKey(chain))
                {
                    foreach (IUpdateComponent component in UpdateChains[chain])
                    {
                        component.Update();
                    }
                }
            }
            if (Children.Count != 0)
            {
                LinkedListNode<Entity> child = Children.First;
                while (child != null)
                {
                    // Scenes handle their own chains during their regular Update.
                    if (chain == null || !child.Value.IsScene) child.Value.Update(chain);
                    child = child.Next;
                }
            }

            if (chain == null && IsScene)
            {
                foreach (string sceneChain in OwnScene.GetUpdateChains())
                {
                    this.Update(sceneChain);
                }
            }
        }
*/

        public Entity CloneTo(Entity parent)
        {
            Entity entity = parent.MakeChild();
            entity.CopyPropertiesFrom(this);
            foreach (Entity child in Children)
            {
                entity.AddChild(child.CloneTo(entity));
            }
            return this;
        }

        public Entity Clone()
        {
            Entity entity = new Entity(scene);
            entity.CopyPropertiesFrom(this);
            foreach (Entity child in Children)
            {
                entity.AddChild(child.Clone());
            }
            return this;
        }

        void CopyPropertiesTo(Entity entity)
        {
            entity.X = X;
            entity.Y = Y;
            entity.Active = Active; // TODO: Does this cause unneccesary propagation? I can call some "entity.CopyFrom(this)".
            entity.Visible = Visible;
            entity.RenderLayer = RenderLayer;
            entity.RenderDepth = RenderDepth;

            //if (groups != null)
            //{
                foreach (string groupName in groups)
                {
                    entity.AddToGroup(groupName);
                }
            //}
            //if (tags != null)
            //{
                foreach (string tagName in tags)
                {
                    entity.AddTag(tagName);
                }
            //}

            foreach (Component component in componentReferences)
            {
                component.Clone().AttachTo(entity);
            }
            /*
            if (UpdateComponent != null) entity.AddUpdateComponent((IUpdateComponent)UpdateComponent.Clone());
            if (RenderComponent != null) entity.AddRenderComponent((IRenderComponent)RenderComponent.Clone());
            foreach (KeyValuePair<Type, Component> entry in ComponentList)
            {
                entity.AddComponent(entry.Value.Clone());
            }
            foreach (KeyValuePair<string, UpdateChain> chain in UpdateChains)
            {
                foreach (IUpdateComponent component in chain.Value)
                {
                    entity.AddChainComponent(chain.Key, (IUpdateComponent)component.Clone());
                }
            }
            */
        }

        void CopyPropertiesFrom(Entity entity)
        {
            X = entity.X;
            Y = entity.Y;
            Active = entity.Active; // TODO: Does this cause unneccesary propagation? I can call some "entity.CopyFrom(this)".
            Visible = entity.Visible;
            RenderLayer = entity.RenderLayer;
            RenderDepth = entity.RenderDepth;

            foreach (string groupName in entity.groups)
            {
                AddToGroup(groupName);
            }

            foreach (string tagName in entity.tags)
            {
                AddTag(tagName);
            }

            foreach (Component component in entity.componentReferences)
            {
                component.Clone().AttachTo(this);
            }
        }
    }

    public class EntitySorter : IComparer<Entity>
    {
        public int Compare(Entity e1, Entity e2)
        {
            int compared = e1.RenderLayer.CompareTo(e2.RenderLayer);
            if (compared != 0) return compared;
            return e1.RenderDepth.CompareTo(e2.RenderDepth);
        }
    }
}
