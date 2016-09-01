using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Nez;
using Nez.Tiled;

using Darkvale.Entitites;
using Darkvale.Colliders;

namespace Darkvale.Scenes.Transitions
{
    public class TransitionManager
    {
        private const int tileWidth = 16;
        private const string namespaceName = "Darkvale.Scenes";
        public static void changeScenes(string destination, string currentLoc)
        {
            string mapName = destination.Split('_')[0];
            string sceneType = destination.Split('_')[1];
               
            if(sceneType == "Outdoors")
            {               
                BaseScene currentScene = (BaseScene)Core.scene;
                Player player = new Player("player");
                
                OutdoorScene destScene = new OutdoorScene(mapName, currentLoc, player);

                destScene.initializeView();

                Core.startSceneTransition(new FadeTransition(() => destScene as OutdoorScene));
            }
            else if(sceneType == "Indoors")
            {
                BaseScene currentScene = (BaseScene)Core.scene;
                Player player = new Player("player");

                IndoorScene destScene = new IndoorScene(mapName, currentLoc, player);

                destScene.initializeView();

                Core.startSceneTransition(new FadeTransition(() => destScene as IndoorScene));
            }
        }

        public static Entity attachTransitionColliders(List<TiledObjectGroup> objectLayers, Entity tiledEntity, string location)
        {
            foreach (TiledObjectGroup tog in objectLayers)
            {
                if (tog.name == "Transitions")
                {
                    TiledObjectGroup transitionLayer = tog;
                    foreach (TiledObject transition in transitionLayer.objects)
                    {
                        string destination;
                        transition.properties.TryGetValue("transitionTo", out destination);
                        TransitionCollider tc = new TransitionCollider(transition.x, transition.y, tileWidth, destination, location);
                        tc.isTrigger = true;
                        tiledEntity.addCollider(tc);
                    }                 
                }
                break;
            }
            return tiledEntity;
        }

        public static PlayerSpawnInfo findPlayerSpawn(List<TiledObjectGroup> objectLayers, string prevMap)
        {
            PlayerSpawnInfo psi = new PlayerSpawnInfo(0, 0, 0);
            foreach (TiledObjectGroup tog in objectLayers)
            {
                if (tog.name == "PlayerSpawn")
                {
                    TiledObjectGroup playerSpawnLayer = tog;
                    foreach (TiledObject to in playerSpawnLayer.objects)
                    {
                        if(to.name.Contains(prevMap))
                        {
                            string direction;
                            to.properties.TryGetValue("Direction", out direction);
                            psi.x = to.x;
                            psi.y = to.y;
                            psi.direction = int.Parse(direction);
                        }
                        break;
                    }
                }
                
            }
            return psi;
        }
    }
}
