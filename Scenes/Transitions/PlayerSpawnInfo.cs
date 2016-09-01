using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkvale.Scenes.Transitions
{
    public class PlayerSpawnInfo
    {
        public int x;
        public int y;
        public int direction;

        public PlayerSpawnInfo(int x, int y, int dir)
        {
            this.x = x;
            this.y = y;
            this.direction = dir;
        }
    }
}
