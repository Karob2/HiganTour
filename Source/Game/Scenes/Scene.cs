using Lichen;
using Lichen.Entities;
using Lichen.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDeath.Scenes
{
    public class Scene
    {
        protected Entity root;
        protected Entity container;

        public void Activate()
        {
            container.SetActive(true);
            container.SetVisible(true);
        }

        public void Deactivate()
        {
            container.SetActive(false);
            container.SetVisible(false);
        }

        public virtual void Preload(Entity root) { }
        public virtual void Load() { }
        public virtual void Unload() { }
    }
}
