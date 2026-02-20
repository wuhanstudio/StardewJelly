using System;
using System.Collections.Generic;
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

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        // GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
        
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
        {
            dir = Dir.Up;
            isMoving = true;
        }
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        {
            dir = Dir.Down;
            isMoving = true;
        }

        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            dir = Dir.Left;
            isMoving = true;
        }

        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            dir = Dir.Right;
            isMoving = true;
        }

        if (isMoving)
        {
            switch (dir)
            {
                case Dir.Up:
                    position.Y -= speed * dt;
                    break;
                case Dir.Down:
                    position.Y += speed * dt;
                    isMoving = true;
                    break;
                case Dir.Left:
                    position.X -= speed * dt;
                    isMoving = true;
                    break;
                case Dir.Right:
                    position.X += speed * dt;
                    isMoving = true;
                    break;
            }            
        }
        
        isMoving = false;
    }
}

