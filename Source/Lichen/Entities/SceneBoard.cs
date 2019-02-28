using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public class SceneBoard
    {
        List<Scene> scenes = new List<Scene>();
        Dictionary<Scene, bool?> active = new Dictionary<Scene, bool?>();

        public void Add(Scene scene)
        {
            scenes.Add(scene);
            active.Add(scene, false);
        }

        public Scene New()
        {
            Scene scene = new Scene();
            Add(scene);
            return scene;
        }

        public void Activate(Scene scene)
        {
            active[scene] = true;
        }

        public void Deactivate(Scene scene)
        {
            active[scene] = false;
        }

        public void Switch(Scene scene)
        {
            foreach(Scene scene2 in scenes)
            {
                if (Object.ReferenceEquals(scene, scene2))
                {
                    active[scene] = true;
                }
                else
                {
                    active[scene] = false;
                }
            }
        }

        /*
        public void Update(string chain)
        {
            foreach (Scene scene in scenes)
            {
                scene.Update(chain);
            }
        }
        */

        public void Update()
        {
            foreach (Scene scene in scenes)
            {
                if (active[scene].Value) scene.Update();
            }
        }

        public void Render()
        {
            foreach (Scene scene in scenes)
            {
                if (active[scene].Value) scene.Render();
            }
        }
    }
}
