using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Nez;

using Darkvale.Scenes;
using Darkvale.Entitites;


namespace Darkvale
{

    public class Game1 : Core
    {

        public static Game1 gameRef;
        public Player player;
        public Game1()
            : base()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            Window.AllowUserResizing = true;
            Player player = new Player("player");

            var firstScene = new IndoorScene("Indoors1", "Outdoors1", player);
            firstScene.initializeView();
            //var myScene = new Outdoors1();
            //scene = myScene;

            scene = firstScene;
        }
    }
}

