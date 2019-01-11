using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Util
{
    public interface IPoolable
    {
        bool Vacant { get; set; }
        void Reset();
    }
}
