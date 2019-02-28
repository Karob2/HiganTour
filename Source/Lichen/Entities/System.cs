using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public abstract class System
    {
        LinkedList<Subsystem> subsystems = new LinkedList<Subsystem>();

        public abstract void Update(Scene scene);

        public void RunSubsystems(Entity entity, Action action)
        {
            LinkedListNode<Subsystem> node = subsystems.First;
            if (node == null) action();
            else node.Value.Update(node, entity, action);
        }

        public System AddSubsystem(Subsystem subsystem)
        {
            subsystems.AddLast(subsystem);
            return this;
        }
    }
}
