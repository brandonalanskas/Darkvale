using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nez;
using Nez.Tiled;

using Darkvale.Scenes.Transitions;
using Darkvale.Entitites;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Darkvale.Scenes
{
    public class BaseScene : Scene
    {
        DefaultRenderer renderer;
        public Player player;
        bool isIndoor;
        protected string mapName;
        protected string prevMapName;
        private PlayerSpawnInfo playerSpawn;
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }
        public bool IsIndoor
        {
            get { return isIndoor; }
            set { isIndoor = value; }
        }
        public string MapName
        {
            get { return mapName; }
            set { mapName = value; }
        }

        public string PrevMapName
        {
            get { return prevMapName; }
            set { prevMapName = value; }
        }

        private PlayerSpawnInfo PlayerSpawn
        {
            get { return playerSpawn; }
            set { playerSpawn = value; }
        }

        public BaseScene()
            : base()
        {
            renderer = new DefaultRenderer();
        }



        public override void initialize()
        {
            base.initialize();
            //Player = new Player("player");

            //Core.debugRenderEnabled = true;
        }

        protected void initializeGraphics()
        {
            renderer = new DefaultRenderer(100, this.camera);
            renderer.shouldDebugRender = true;
            this.addRenderer(renderer);
            this.getRenderer<DefaultRenderer>().render(this);

            setDesignResolution(512, 256, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            Screen.setSize(512 * 3, 256 * 3);
        }

        protected void createTileMap(string mapName)
        {
            Entity tiledEntity = createEntity("tiled-map-entity");
            TiledMap tiledMap = this.content.Load<TiledMap>("Maps/" + mapName);
            TiledMapComponent tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap, "Collisions"));
            tiledMapComponent.setLayersToRender(new string[] { "Ground", "Walls", "Details" });
            tiledMapComponent.renderLayer = 10;

            TiledMapComponent tiledMapDetailsComp = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapDetailsComp.setLayerToRender("Above-Details");
            tiledMapDetailsComp.renderLayer = -1;

            List<TiledObjectGroup> tiledObjects = tiledMap.objectGroups;

            tiledEntity = TransitionManager.attachTransitionColliders(tiledObjects, tiledEntity, mapName);
            this.PlayerSpawn = TransitionManager.findPlayerSpawn(tiledObjects, PrevMapName);

            tiledMapDetailsComp.material = Material.stencilWrite();
            tiledMapDetailsComp.material.effect = this.content.loadNezEffect<SpriteAlphaTestEffect>();
        }


        protected void initializePlayer()
        {
            PlayerSpawnInfo psi = PlayerSpawn;
            player.transform.position = new Vector2(psi.x, psi.y);
            player.anim.direction = psi.direction;
            this.addEntity(this.player);
            camera.entity.addComponent(new FollowCamera(this.player));
        }
    }
}
