using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Collisions;

namespace StardewJelly;

public interface IEntity : ICollisionActor
{
    public void Update(GameTime gameTime);
    public void Draw(SpriteBatch spriteBatch);
}