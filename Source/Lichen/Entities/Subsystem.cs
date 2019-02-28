using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public abstract class Subsystem
    {
        public virtual void Update(LinkedListNode<Subsystem> node, Entity entity, Action action)
        {
            node = node.Next;
            if (node != null)
            {
                node.Value.Update(node, entity, action);
            }
            else
            {
                action();
            }
        }
    }
}
