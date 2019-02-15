using Lichen;
using Lichen.Entities;
using Lichen.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiganTour.Scenes
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

        public bool IsActive()
        {
            return container.Active;
        }

        public virtual void Preload(Entity root) { }
        public virtual void Load() { }
        public virtual void Unload() { }
    }
}
