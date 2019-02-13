using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public interface IUpdateComponent
    {
        Entity Owner { get; set; }
        void Update();
        Component Clone();
    }
}
