using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace StardewJelly;
class Player
{
    private Vector2 position = new Vector2(500, 300);
    private int speed = 300;
    private Dir dir  = Dir.Down;
    private bool isMoving = false;

    public Vector2 Position
    {
        get => position;
        set => position = value;
    }

    public void setX(int x)
    {
        position.X = x;
    }

    public void setY(int y)
    {
        position.Y = y;
    }

    public Dir getDirection()
    {
        return dir;
    }
    
    public bool IsMoving
    {
        get => isMoving;
    }

    public void Update(GameTime gameTime)
    {
        Keys[] movementKeys =
        [
            Keys.W, Keys.A, Keys.S, Keys.D,  // WASD
            Keys.Up, Keys.Left, Keys.Down, Keys.Right  // Arrow keys
        ];
        
        KeyboardState keyboardState = Keyboard.GetState();
        // GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
        
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
        {
            dir = Dir.Up;
        }
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        {
            dir = Dir.Down;
        }

        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            dir = Dir.Left;
        }

        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            dir = Dir.Right;
        }

        // Player is moving if any movement key is currently pressed
        isMoving = movementKeys.Any(k => keyboardState.IsKeyDown(k));
        
        if (isMoving)
        {
            switch (dir)
            {
                case Dir.Up:
                    if(position.Y > 200)
                        position.Y -= speed * dt;
                    break;
                case Dir.Down:
                    if(position.Y < 1250)
                        position.Y += speed * dt;
                    isMoving = true;
                    break;
                case Dir.Left:
                    if(position.X > 225)
                        position.X -= speed * dt;
                    isMoving = true;
                    break;
                case Dir.Right:
                    if(position.X < 1275)
                        position.X += speed * dt;
                    isMoving = true;
                    break;
            }            
        }
    }
}

