using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDeath.Components
{
    class PlayerControlComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        public void Update()
        {
            if (Lichen.GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Up))
            {
                Owner.Y -= 3;
            }
            if (Lichen.GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Down))
            {
                Owner.Y += 3;
            }
            if (Lichen.GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Left))
            {
                Owner.X -= 3;
            }
            if (Lichen.GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Right))
            {
                Owner.X += 3;
            }
        }
    }
}
