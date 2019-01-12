using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public class Entity
    {
        public Entity Parent { get; set; }
        public LinkedList<Entity> Children { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        private float relativeX;
        private float relativeY;
        public float RelativeX { get { return relativeX; } }
        public float RelativeY { get { return relativeY; } }
        public bool Active { get; set; } = true;
        public bool Visible { get; set; } = true;
        private bool inheritedVisibility;
        public bool InheritedVisibility { get { return inheritedVisibility; } }
        // UpdateComponent: For components that have code to run every frame, such as updating motion and collisions, and responding to player input.
        // RenderComponent: For components that need to be drawn every frame.
        // ComponentList: For components that do not Update or Render, such as data holders.
        // UpdateChains: Same as Update, but allows lists of multiple components to be ran together.
        public IUpdateComponent UpdateComponent { get; set; }
        public IRenderComponent RenderComponent { get; set; }
        public Dictionary<Type,Component> ComponentList { get; set; }
        public Dictionary<string,UpdateChain> UpdateChains { get; set; }

        public bool RenderByDepth { get; set; }
        public float RenderOrder { get; set; }
        public int RenderBackupOrder { get; set; }
        public static int RenderBackupCount { get; set; }

        public List<Entity> ActorList { get; set; }

        public Entity()
        {
            Children = new LinkedList<Entity>();
            ComponentList = new Dictionary<Type, Component>();
            UpdateChains = new Dictionary<String, UpdateChain>();
        }

        public Entity(float x, float y) : this()
        {
            X = x;
            Y = y;
        }

        public Entity(float x, float y, bool active, bool visible) : this()
        {
            X = x;
            Y = y;
            Active = active;
            Visible = visible;
        }

        public Entity(bool active, bool visible) : this()
        {
            Active = active;
            Visible = visible;
        }

        public Entity SetPosition(float x, float y)
        {
            X = x;
            Y = y;
            return this;
        }

        public Entity SetActive()
        {
            Active = true;
            return this;
        }
        public Entity SetActive(bool active)
        {
            Active = active;
            return this;
        }

        public Entity SetVisible()
        {
            Visible = true;
            return this;
        }
        public Entity SetVisible(bool visible)
        {
            Visible = visible;
            return this;
        }

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

        public Entity AddComponent(Component component)
        {
            ComponentList.Add(component.GetType(), component);
            component.Owner = this;
            return this;
        }

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

        // TODO: Decide whether or not to keep both AttachTo and AddChild.

        public Entity AttachTo(Entity entity)
        {
            Parent = entity;
            entity.Children.AddLast(this);
            return this;
        }

        public Entity AddChild(Entity entity)
        {
            Children.AddLast(entity);
            entity.Parent = this;
            return this;
        }

        public bool IsVisible()
        {
            return inheritedVisibility;
        }

        public void Render()
        {
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
                RenderOrder = -Y;
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

        public void Update(string chain = null)
        {
            if (!Active) return;
            if (Parent != null)
            {
                inheritedVisibility = Parent.InheritedVisibility;
            }
            else
            {
                inheritedVisibility = Visible;
            }
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
                    child.Value.Update(chain);
                    child = child.Next;
                }
            }
        }

        public Entity Clone()
        {
            Entity entity = new Entity();
            foreach (Entity child in Children)
            {
                entity.AddChild(child.Clone());
            }
            entity.X = X;
            entity.Y = Y;
            entity.Active = Active;
            entity.Visible = Visible;

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

            return entity;
        }
    }

    public class EntitySorter : IComparer<Entity>
    {
        public int Compare(Entity e1, Entity e2)
        {
            int compared = e2.RenderOrder.CompareTo(e1.RenderOrder);
            if (compared != 0) return compared;
            return e1.RenderBackupOrder.CompareTo(e2.RenderBackupOrder);
        }
    }
}
