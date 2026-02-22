using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Graphics;

namespace StardewJelly;

public class Enermy: IEntity
{
    public bool dead =  false;
    public IShapeF Bounds { get; }
    
    private readonly Vector2 _enermyOffset = new Vector2(48, 66);
    
    private Vector2 _position = new Vector2(300, 300);
    private const int Speed = 300;
    private const int _radius = 48;
    private Dir _dir  = Dir.Down;

    private SpriteSheet _floatingSpriteSheet;
    private AnimationController _floatingAnimationController;
    
    public Enermy(Texture2D texture)
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
        
        Bounds = new CircleF(_position, _radius);
    }
    
    public void Update(GameTime gameTime)
    {
        Bounds.Position =  _position;
        _floatingAnimationController.Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawCircle((CircleF)Bounds, _radius, Color.Red, 3f);
        Texture2DRegion currentWalkFrame= _floatingSpriteSheet.TextureAtlas[_floatingAnimationController.CurrentFrame];
        
        spriteBatch.Draw(currentWalkFrame, _position - _enermyOffset, Color.White);
    }
    
    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        dead = true;
    }
}