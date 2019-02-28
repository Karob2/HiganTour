using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public interface IRenderComponent
    {
        Entity Owner { get; set; }
        void Update();
        Component Clone();
    }
}
