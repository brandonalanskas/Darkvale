using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nez;

using Darkvale.Entitites;

namespace Darkvale.Scenes
{
    public class OutdoorScene : BaseScene
    {
        public OutdoorScene(string mapName, string prevMapName, Player player)
            :base()
        {
            IsIndoor = false;
            base.MapName = mapName;
            base.Player = player;
            base.PrevMapName = prevMapName;
        }

        public override void initialize()
        {
            base.initialize();
        }

        public void initializeView()
        {
            base.initializeGraphics();
            base.createTileMap(mapName);
            base.initializePlayer();
        }
    }
}
