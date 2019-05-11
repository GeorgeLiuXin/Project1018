using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld
{
    public interface IComponent
    {
        ActorEntity Owner
        {
            get;
            set;
        }

        void OnComponentReadyToStart();
        void OnComponentStart();
        void OnPreDestroy();
    }
}
