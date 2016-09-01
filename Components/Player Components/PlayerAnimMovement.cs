using System;
using Nez;
using Nez.Sprites;
using Nez.Systems;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using Darkvale.Colliders;
using Darkvale.Scenes;
using Darkvale.Scenes.Transitions;
using Darkvale.Entitites;

namespace Darkvale.Components
{
    public class PlayerAnimMovement : Component, ITriggerListener, IUpdatable
    {
        enum Animations
        {
            WalkUp,
            WalkDown,
            WalkRight,
            WalkLeft,
            IdleUp,
            IdleDown,
            IdleRight,
            IdleLeft
        }

        Sprite<Animations> playerAnim;
        Mover mover;
        float moveSpeed = 100f;
        public int direction;

        VirtualIntegerAxis xAxisInput;
        VirtualIntegerAxis yAxisInput;

        List<Subtexture> sprites = new List<Subtexture>();

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();

            Texture2D link = entity.scene.content.Load<Texture2D>("Spritesheets/LinkRun");
            sprites = splitTextures(link);

            mover = entity.addComponent(new Mover());
            playerAnim = entity.addComponent(new Sprite<Animations>(sprites[0]));

            var shadow = entity.addComponent(new SpriteMime(playerAnim));
            shadow.color = new Color(10, 10, 10, 80);
            shadow.material = Material.stencilRead();
            shadow.renderLayer = -2;

            SpriteAnimation walkUp = new SpriteAnimation(new List<Subtexture>()
            {
                sprites[0],
                sprites[1],
                sprites[2],
                sprites[3],
                sprites[4],
                sprites[5],
                sprites[6]
            });

            SpriteAnimation walkLeft = new SpriteAnimation(new List<Subtexture>()
            {
                sprites[7],
                sprites[8],
                sprites[9],
                sprites[10],
                sprites[11],
                sprites[12],
                sprites[13]
            });

            SpriteAnimation walkRight = new SpriteAnimation(new List<Subtexture>()
            {
                sprites[14],
                sprites[15],
                sprites[16],
                sprites[17],
                sprites[18],
                sprites[19],
                sprites[20]
            });

            SpriteAnimation walkDown = new SpriteAnimation(new List<Subtexture>()
            {
                sprites[21],
                sprites[22],
                sprites[23],
                sprites[24],
                sprites[25],
                sprites[26],
                sprites[27]
            });

            SpriteAnimation idleUp = new SpriteAnimation(new List<Subtexture>()
            {
                sprites[0]
            });

            SpriteAnimation idleLeft = new SpriteAnimation(new List<Subtexture>()
            {
                sprites[7]
            });

            SpriteAnimation idleRight = new SpriteAnimation(new List<Subtexture>()
            {
                sprites[14]
            });

            SpriteAnimation idleDown = new SpriteAnimation(new List<Subtexture>()
            {
                sprites[21]
            });

            walkUp.fps = 11;
            walkLeft.fps = 11;
            walkRight.fps = 11;
            walkDown.fps = 11;

            walkLeft.totalDuration = 0.5f;


            playerAnim.addAnimation(Animations.WalkUp, walkUp);
            playerAnim.addAnimation(Animations.WalkLeft, walkLeft);
            playerAnim.addAnimation(Animations.WalkRight, walkRight);
            playerAnim.addAnimation(Animations.WalkDown, walkDown);
            playerAnim.addAnimation(Animations.IdleUp, idleUp);
            playerAnim.addAnimation(Animations.IdleLeft, idleLeft);
            playerAnim.addAnimation(Animations.IdleRight, idleRight);
            playerAnim.addAnimation(Animations.IdleDown, idleDown);


            setupInput();
        }

        private List<Subtexture> splitTextures(Texture2D sprite)
        {
            List<Subtexture> sprites = new List<Subtexture>();
            //Up
            sprites.Add(new Subtexture(sprite, 0, 0, 24, 32));
            sprites.Add(new Subtexture(sprite, 25, 0, 24, 32));
            sprites.Add(new Subtexture(sprite, 50, 0, 24, 32));
            sprites.Add(new Subtexture(sprite, 80, 0, 24, 32));
            sprites.Add(new Subtexture(sprite, 104, 0, 24, 32));
            sprites.Add(new Subtexture(sprite, 128, 0, 24, 32));
            sprites.Add(new Subtexture(sprite, 155, 0, 24, 32));

            //Left
            sprites.Add(new Subtexture(sprite, 0, 38, 24, 32));
            sprites.Add(new Subtexture(sprite, 25, 38, 24, 32));
            sprites.Add(new Subtexture(sprite, 52, 38, 28, 32));
            sprites.Add(new Subtexture(sprite, 80, 38, 28, 32));
            sprites.Add(new Subtexture(sprite, 108, 38, 24, 32));
            sprites.Add(new Subtexture(sprite, 132, 38, 26, 32));
            sprites.Add(new Subtexture(sprite, 158, 38, 30, 32));

            //Right
            sprites.Add(new Subtexture(sprite, 0, 78, 24, 32));
            sprites.Add(new Subtexture(sprite, 25, 78, 24, 32));
            sprites.Add(new Subtexture(sprite, 52, 78, 26, 32));
            sprites.Add(new Subtexture(sprite, 78, 78, 30, 32));
            sprites.Add(new Subtexture(sprite, 108, 78, 24, 32));
            sprites.Add(new Subtexture(sprite, 132, 78, 26, 32));
            sprites.Add(new Subtexture(sprite, 160, 78, 26, 32));

            //Down
            sprites.Add(new Subtexture(sprite, 0, 112, 24, 32));
            sprites.Add(new Subtexture(sprite, 25, 112, 24, 29));
            sprites.Add(new Subtexture(sprite, 52, 112, 28, 32));
            sprites.Add(new Subtexture(sprite, 80, 112, 24, 32));
            sprites.Add(new Subtexture(sprite, 104, 112, 24, 32));
            sprites.Add(new Subtexture(sprite, 128, 112, 24, 32));
            sprites.Add(new Subtexture(sprite, 151, 112, 24, 32));

            //IdleUp
            sprites.Add(new Subtexture(sprite, 0, 0, 24, 32));

            //IdleLeft
            sprites.Add(new Subtexture(sprite, 0, 38, 24, 32));

            //IdleRight
            sprites.Add(new Subtexture(sprite, 0, 78, 24, 32));

            //IdleDown
            sprites.Add(new Subtexture(sprite, 0, 112, 24, 32));

            return sprites;
        }

        void setupInput()
        {
            xAxisInput = new VirtualIntegerAxis();
            xAxisInput.nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
            xAxisInput.nodes.Add(new VirtualAxis.GamePadLeftStickX());
            xAxisInput.nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));
            xAxisInput.nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.A, Keys.D));

            yAxisInput = new VirtualIntegerAxis();
            yAxisInput.nodes.Add(new VirtualAxis.GamePadDpadUpDown());
            yAxisInput.nodes.Add(new VirtualAxis.GamePadLeftStickY());
            yAxisInput.nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));
            yAxisInput.nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.W, Keys.S));

        }

        void IUpdatable.update()
        {

            Vector2 moveDir = new Vector2(xAxisInput.value, yAxisInput.value);
            Animations animation = Animations.WalkDown;

            if (moveDir.X < 0)
            {
                animation = Animations.WalkLeft;
                direction = 1;
            }
            else if (moveDir.X > 0)
            {
                animation = Animations.WalkRight;
                direction = 2;
            }

            if (moveDir.Y < 0)
            {
                animation = Animations.WalkUp;
                direction = 0;
            }
            else if (moveDir.Y > 0)
            {
                animation = Animations.WalkDown;
                direction = 3;
            }

            if (moveDir != Vector2.Zero)
            {
                if (!playerAnim.isAnimationPlaying(animation))
                    playerAnim.play(animation);

                var movement = moveDir * 100f * Time.deltaTime;

                CollisionResult res;
                mover.move(movement, out res);
            }

            else
            {
                switch (direction)
                {
                    case 0:
                        animation = Animations.IdleUp;
                        playerAnim.play(animation);
                        break;
                    case 1:
                        animation = Animations.IdleLeft;
                        playerAnim.play(animation);
                        break;
                    case 2:
                        animation = Animations.IdleRight;
                        playerAnim.play(animation);
                        break;
                    case 3:
                        animation = Animations.IdleDown;
                        playerAnim.play(animation);
                        break;
                    default:
                        break;
                }

                playerAnim.stop();
            }
        }
        void ITriggerListener.onTriggerEnter(Collider other, Collider self)
        {
            TransitionCollider tc = (TransitionCollider)other;
            TransitionManager.changeScenes(tc.destination, tc.location);
            Debug.log("triggerEnter: {0}", other.entity.name);
        }


        void ITriggerListener.onTriggerExit(Collider other, Collider self)
        {
            Debug.log("triggerExit: {0}", other.entity.name);
        }
    }
}
