using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PROJECT_RPG
{
    abstract class ViralComponentInterface
    {
        // Used for any data that needs to be added to the component at creation
        public abstract void Initialize();

        // All components will need to be updated at some point
        public abstract void Update(GameTime gametime);
    }
}
