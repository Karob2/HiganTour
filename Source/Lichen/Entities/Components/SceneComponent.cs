using Lichen.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public class SceneComponent : Component, IRenderComponent, IUpdateComponent
    {
        Scene scene;

        public SceneComponent()
        {
            scene = new Scene();
        }

        public SceneComponent(Scene scene)
        {
            this.scene = scene;
        }

        public Scene GetScene()
        {
            return scene;
        }

        public Entity GetSceneRoot()
        {
            return scene.Root;
        }

        public void Render()
        {
            scene.Render(Owner);
        }

        public void Update()
        {
            scene.Update(Owner);
        }
    }
}
