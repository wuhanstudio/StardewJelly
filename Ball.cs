using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace StardewJelly;

public class Ball: IEntity
{
    public IShapeF Bounds { get; }
    
    private bool _dead;

    public bool Dead()
    {
        return _dead;
    }

    private readonly Vector2 _ballOffset = new Vector2(48, 48);
    
    private Vector2 _position;
    private Dir _direction;
    
    private const int Speed = 10;
    private const int _radius = 48;

    private Random rand = new Random();
    Texture2D _ballTexture;

      public Ball(Texture2D texture)
      {
          _ballTexture = texture;
          _position = Player.Instance.Position;
          Bounds = new CircleF(_position, _radius / 2); 
          _direction = Player.Instance.Direction;
      }
    
    public void Update(GameTime gameTime)
    {
        // Move towards the player
        if (_dead)
        {
            return;
        }
        
        if (!Player.Instance.Dead())
        {
            switch (_direction)
            {
                case Dir.Up:
                    _position.Y =  _position.Y - Speed;
                    break;
                case Dir.Down:
                    _position.Y = _position.Y + Speed;
                    break;
                case Dir.Left:
                    _position.X = _position.X - Speed;
                    break;
                case Dir.Right:
                    _position.X = _position.X + Speed;
                    break;
            }
        }
    
        Bounds.Position =  _position;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw the collision box
        // spriteBatch.DrawCircle((CircleF)Bounds, _radius, Color.Red, 3f);

        if (!_dead)
        {
            spriteBatch.Draw(_ballTexture, _position - _ballOffset, Color.White);
        }
    }
    
    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        if (collisionInfo.Other.GetType() == typeof(Enemy))
        {
            _dead = true;
        }
    }
}
