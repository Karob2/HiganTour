using HiganTour.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace HiganTour.Components
{
    class MenuComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        public void Update()
        {

            if (!((Game1)Lichen.GlobalServices.Game).GetMR())
            {

                if (Lichen.GlobalServices.InputManager.JustPressed(Lichen.Input.GameCommand.MenuConfirm))
                {
                    ((Game1)Lichen.GlobalServices.Game).ChangeScene(1);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.M))
                {
                    ((Game1)Lichen.GlobalServices.Game).ChangeScene(3);
                }
            }
            if (((Game1)Lichen.GlobalServices.Game).GetMR())
            {

                if (Lichen.GlobalServices.InputManager.JustPressed(Lichen.Input.GameCommand.MenuCancel))
                {
                    ((Game1)Lichen.GlobalServices.Game).MRPause();

                    ((Game1)Lichen.GlobalServices.Game).ChangeScene(0);
                }

                if (Lichen.GlobalServices.InputManager.JustPressed(Lichen.Input.GameCommand.MenuUp))
                {
                    ((Game1)Lichen.GlobalServices.Game).SwitchSong(false);
                }
                if (Lichen.GlobalServices.InputManager.JustPressed(Lichen.Input.GameCommand.MenuDown))
                {
                    ((Game1)Lichen.GlobalServices.Game).SwitchSong(true);
                }
            }
        }
    }
}
