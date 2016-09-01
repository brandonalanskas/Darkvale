using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace Darkvale.Colliders
{
    public class TransitionCollider: BoxCollider
    {
        public string destination;
        public string location;

        public TransitionCollider(int x, int y, int tileWidth, string destination, string location)
            :base(x, y, tileWidth, 0)
        {
            this.destination = destination;
            this.location = location;
        }
    }
}
