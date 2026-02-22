using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Graphics;

namespace StardewJelly;

public class Enemy: IEntity
{
    private bool _dead;
    
    public bool Dead()
    {
        return _dead;
    }
    
    public IShapeF Bounds { get; }
    
    private readonly Vector2 _enermyOffset = new Vector2(48, 66);
    
    private Vector2 _position;
    private Vector2 _direction;
    
    private const int Speed = 5;
    private const int _radius = 48;

    private SpriteSheet _floatingSpriteSheet;
    private AnimationController _floatingAnimationController;

    private Random rand = new Random();
    
    public Enemy(Texture2D texture)
    {
        Texture2DAtlas floatingAtlas = Texture2DAtlas.Create("enermy/floating", texture, 96, 132);
        
        // Floating Animation
        _floatingSpriteSheet = new SpriteSheet("SpriteSheet/floating", floatingAtlas);
        _floatingSpriteSheet.DefineAnimation("floating", builder =>
        {
            builder.IsLooping(true)
                .AddFrame(floatingAtlas.GetRegion(0).Name, TimeSpan.FromSeconds(0.1))
                .AddFrame(floatingAtlas.GetRegion(1).Name, TimeSpan.FromSeconds(0.1))
                .AddFrame(floatingAtlas.GetRegion(2).Name, TimeSpan.FromSeconds(0.1))
                .AddFrame(floatingAtlas.GetRegion(3).Name, TimeSpan.FromSeconds(0.1))
                .AddFrame(floatingAtlas.GetRegion(4).Name, TimeSpan.FromSeconds(0.1))
                .AddFrame(floatingAtlas.GetRegion(5).Name, TimeSpan.FromSeconds(0.1))
                .AddFrame(floatingAtlas.GetRegion(6).Name, TimeSpan.FromSeconds(0.1))
                .AddFrame(floatingAtlas.GetRegion(7).Name, TimeSpan.FromSeconds(0.1))
                .AddFrame(floatingAtlas.GetRegion(8).Name, TimeSpan.FromSeconds(0.1))
                .AddFrame(floatingAtlas.GetRegion(9).Name, TimeSpan.FromSeconds(0.1));
        });
        SpriteSheetAnimation floatingAnimation = _floatingSpriteSheet.GetAnimation("floating");
        _floatingAnimationController = new AnimationController(floatingAnimation);

        if (rand.NextDouble() < 0.5)
        {
            _position = new Vector2(-500, rand.Next(-500, 2000));
        }
        else
        {
            _position = new Vector2(rand.Next(-500, 2000), -500);
        }
        Bounds = new CircleF(_position, _radius / 2);
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
            // Choose from one direction
            double choice = rand.NextDouble();

            if (choice < 0.40)
            {
                return;
            }
            if (choice < 0.80)
            {
                int randX = 0;
                while (randX.Equals(0))
                {
                    randX = rand.Next(-100, 100);
                }

                int randY = 0;
                while (randY.Equals(0))
                {
                    randY = rand.Next(-100, 100);
                }
                
                _direction = new Vector2(randX, randY);
                _direction.Normalize();
                _position = _position + _direction * Speed;
            }
            else if (choice < 0.94)
            {
                _direction = new Vector2(Player.Instance.Position.X - _position.X, 0);
                _direction.Normalize();
                _position = _position + _direction * Speed;
            }
            else if (choice < 0.98)
            {
                _direction = new Vector2(0, Player.Instance.Position.Y - _position.Y);
                _direction.Normalize();
                _position = _position + _direction * Speed;
            }
            else
            {
                _direction = Player.Instance.Position - _position;
                _direction.Normalize();
                _position = _position + _direction * Speed;
            }
        }
    
        Bounds.Position =  _position;
        _floatingAnimationController.Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw the collision box
        // spriteBatch.DrawCircle((CircleF)Bounds, _radius, Color.Red, 3f);
        if (!_dead)
        {
            Texture2DRegion currentWalkFrame= _floatingSpriteSheet.TextureAtlas[_floatingAnimationController.CurrentFrame];
            spriteBatch.Draw(currentWalkFrame, _position - _enermyOffset, Color.White);
        }
    }
    
    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        if (collisionInfo.Other.GetType() == typeof(Ball))
        {
            _dead = true;
        }
        else
        {
            _direction = -_direction;            
        }
    }
    
    public void Reset()
    {
        _dead = false;
    }
}