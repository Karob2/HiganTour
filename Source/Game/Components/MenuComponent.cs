using LifeDeath.Scenes;
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

            if (!((Game1)Lichen.GlobalServices.Game).getMR())
            {

                if (Lichen.GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Action1))
                {
                    ((Game1)Lichen.GlobalServices.Game).ChangeScene(1);
                }
                if (Lichen.GlobalServices.InputManager.Held(Lichen.Input.GameCommand.MenuRight))
                {
                    ((Game1)Lichen.GlobalServices.Game).ChangeScene(3);
                }
            }
            if (((Game1)Lichen.GlobalServices.Game).getMR())
            {

                if (Lichen.GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Action2))
                {
                    ((Game1)Lichen.GlobalServices.Game).ChangeScene(0);
                }

                if (Lichen.GlobalServices.InputManager.JustPressed(Lichen.Input.GameCommand.MenuUp))
                {
                    ((Game1)Lichen.GlobalServices.Game).SwitchSong();
                }
                if (Lichen.GlobalServices.InputManager.JustPressed(Lichen.Input.GameCommand.MenuDown))
                {
                    ((Game1)Lichen.GlobalServices.Game).MRPause();
                }
            }
        }
    }
}
