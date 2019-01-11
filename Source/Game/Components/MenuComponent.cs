using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDeath.Components
{
    class MenuComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        public void Update()
        {
            if (Lichen.GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Action1))
            {
                ((Game1)Lichen.GlobalServices.Game).ChangeScene(1);
            }
        }
    }
}
