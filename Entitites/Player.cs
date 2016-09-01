using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tiled;

using Darkvale.Components;
namespace Darkvale.Entitites
{
    public class Player : Entity
    {
        public PlayerAnimMovement anim = new PlayerAnimMovement();
        BoxCollider collider =new BoxCollider(-5, 0, 10, 12);
        public string displayName;
        public int testStat = 0;
        public Player(string name)
            : base(name)
        {
            
            this.addComponent(anim);
            this.colliders.add(collider);


            Flags.setFlagExclusive(ref collider.collidesWithLayers, 0);
            Flags.setFlagExclusive(ref collider.physicsLayer, 1);
            
        }

        public void refreshComponents()
        {
            this.removeComponent<PlayerAnimMovement>();
            this.addComponent(new PlayerAnimMovement());
        }
    }
}
