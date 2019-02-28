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
    public abstract class SceneBase
    {
        //protected Entity root;
        public Scene Scene { get; set; }
        protected Entity sceneContainer;

        public void Activate()
        {
            sceneContainer.SetActive(true);
            sceneContainer.SetVisible(true);
        }

        public void Deactivate()
        {
            sceneContainer.SetActive(false);
            sceneContainer.SetVisible(false);
        }

        public bool IsActive()
        {
            return sceneContainer.Active;
        }

        public virtual void Preload(Scene scene)
        {
            Scene = scene;
        }
        public virtual void Load() { }
        public virtual void Unload() { }
    }
}
