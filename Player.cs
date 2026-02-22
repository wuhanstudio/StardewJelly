using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;

namespace StardewJelly;
class Player
{
    private readonly Vector2 _playerOffset = new Vector2(48, 48);
    
    private Vector2 _position = new Vector2(500, 300);
    private const int Speed = 300;
    private Dir _dir  = Dir.Down;
    private bool _isMoving = false;
    
    private readonly SpriteSheet _walkUpSpriteSheet;
    private readonly SpriteSheet _walkDownSpriteSheet;
    private readonly SpriteSheet _walkLeftSpriteSheet;
    private readonly SpriteSheet _walkRightSpriteSheet;

    private readonly AnimationController _walkUpAnimationController;
    private readonly AnimationController _walkDownAnimationController;
    private readonly AnimationController _walkLeftAnimationController;
    private readonly AnimationController _walkRightAnimationController;

    public Player(Texture2D walkUp, Texture2D walkDown, Texture2D walkLeft, Texture2D walkRight)
    {
        Texture2DAtlas walkUpAtlas = Texture2DAtlas.Create("player/walkUpAtlas", walkUp, 96, 96);
        Texture2DAtlas walkDownAtlas = Texture2DAtlas.Create("player/walkDownAtlas", walkDown, 96, 96);
        Texture2DAtlas walkLeftAtlas = Texture2DAtlas.Create("player/walkLeftAtlas", walkLeft, 96, 96);
        Texture2DAtlas walkRightAtlas = Texture2DAtlas.Create("player/walkRightAtlas", walkRight, 96, 96);

        // Walk Up Animation
        _walkUpSpriteSheet = new SpriteSheet("SpriteSheet/walkup", walkUpAtlas);
        _walkUpSpriteSheet.DefineAnimation("walkup", builder =>
        {
            builder.IsLooping(true)
                .AddFrame(walkUpAtlas.GetRegion(0).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkUpAtlas.GetRegion(1).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkUpAtlas.GetRegion(2).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkUpAtlas.GetRegion(3).Name, TimeSpan.FromSeconds(0.2));
        });
        SpriteSheetAnimation walkUpAnimation = _walkUpSpriteSheet.GetAnimation("walkup");
        
        // Walk Down Animation
        _walkDownSpriteSheet = new SpriteSheet("SpriteSheet/walkdown", walkDownAtlas);
        _walkDownSpriteSheet.DefineAnimation("walkdown", builder =>
        {
            builder.IsLooping(true)
                .AddFrame(walkDownAtlas.GetRegion(0).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkDownAtlas.GetRegion(1).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkDownAtlas.GetRegion(2).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkDownAtlas.GetRegion(3).Name, TimeSpan.FromSeconds(0.2));
        });
        SpriteSheetAnimation walkDownAnimation = _walkDownSpriteSheet.GetAnimation("walkdown");

        // Walk Left Animation
        _walkLeftSpriteSheet = new SpriteSheet("SpriteSheet/walkleft", walkLeftAtlas);
        _walkLeftSpriteSheet.DefineAnimation("walkleft", builder =>
        {
            builder.IsLooping(true)
                .AddFrame(walkLeftAtlas.GetRegion(0).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkLeftAtlas.GetRegion(1).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkLeftAtlas.GetRegion(2).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkLeftAtlas.GetRegion(3).Name, TimeSpan.FromSeconds(0.2));
        });
        SpriteSheetAnimation walkLeftAnimation = _walkLeftSpriteSheet.GetAnimation("walkleft");

        // Walk Right Animation
        _walkRightSpriteSheet = new SpriteSheet("SpriteSheet/walkright", walkRightAtlas);
        _walkRightSpriteSheet.DefineAnimation("walkright", builder =>
        {
            builder.IsLooping(true)
                .AddFrame(walkRightAtlas.GetRegion(0).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkRightAtlas.GetRegion(1).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkRightAtlas.GetRegion(2).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walkRightAtlas.GetRegion(3).Name, TimeSpan.FromSeconds(0.2));
        });
        SpriteSheetAnimation walkRightAnimation = _walkRightSpriteSheet.GetAnimation("walkright");
        
        _walkUpAnimationController = new AnimationController(walkUpAnimation);
        _walkDownAnimationController = new AnimationController(walkDownAnimation);
        _walkLeftAnimationController = new AnimationController(walkLeftAnimation);
        _walkRightAnimationController = new AnimationController(walkRightAnimation);
    }
    
    public Vector2 Position
    {
        get => _position;
        set => _position = value;
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
            _dir = Dir.Up;
        }
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        {
            _dir = Dir.Down;
        }

        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            _dir = Dir.Left;
        }

        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            _dir = Dir.Right;
        }

        // Player is moving if any movement key is currently pressed
        _isMoving = movementKeys.Any(k => keyboardState.IsKeyDown(k));
        
        if (_isMoving)
        {
            switch (_dir)
            {
                case Dir.Up:
                    _walkUpAnimationController.Update(gameTime);
                    if(_position.Y > 200)
                        _position.Y -= Speed * dt;
                    break;
                case Dir.Down:
                    _walkDownAnimationController.Update(gameTime);
                    if(_position.Y < 1250)
                        _position.Y += Speed * dt;
                    break;
                case Dir.Left:
                    _walkLeftAnimationController.Update(gameTime);
                    if(_position.X > 225)
                        _position.X -= Speed * dt;
                    break;
                case Dir.Right:
                    _walkRightAnimationController.Update(gameTime);
                    if(_position.X < 1275)
                        _position.X += Speed * dt;
                    break;
            }            
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    { 
        if(_isMoving)
        {
            Texture2DRegion currentWalkFrame;
            switch (_dir)
            {
                case Dir.Up:
                    currentWalkFrame= _walkUpSpriteSheet.TextureAtlas[_walkUpAnimationController.CurrentFrame];
                    spriteBatch.Draw(currentWalkFrame, Position - _playerOffset, Color.White);
                    break;  
                case Dir.Down:
                    currentWalkFrame = _walkDownSpriteSheet.TextureAtlas[_walkDownAnimationController.CurrentFrame];
                    spriteBatch.Draw(currentWalkFrame, Position - _playerOffset, Color.White);
                    break; 
                case Dir.Left:
                    currentWalkFrame = _walkLeftSpriteSheet.TextureAtlas[_walkLeftAnimationController.CurrentFrame];
                    spriteBatch.Draw(currentWalkFrame, Position - _playerOffset, Color.White);
                    break;   
                case Dir.Right:
                    currentWalkFrame = _walkRightSpriteSheet.TextureAtlas[_walkRightAnimationController.CurrentFrame];
                    spriteBatch.Draw(currentWalkFrame, Position - _playerOffset, Color.White);
                    break;   
            }
        }
        else
        {
            Texture2DRegion currentWalkFrame;
            switch (_dir)
            {
                case Dir.Up:
                    currentWalkFrame= _walkUpSpriteSheet.TextureAtlas[3];
                    spriteBatch.Draw(currentWalkFrame, Position - _playerOffset, Color.White);
                    break;  
                case Dir.Down:
                    currentWalkFrame = _walkDownSpriteSheet.TextureAtlas[3];
                    spriteBatch.Draw(currentWalkFrame, Position - _playerOffset, Color.White);
                    break; 
                case Dir.Left:
                    currentWalkFrame = _walkLeftSpriteSheet.TextureAtlas[3];
                    spriteBatch.Draw(currentWalkFrame, Position - _playerOffset, Color.White);
                    break;   
                case Dir.Right:
                    currentWalkFrame = _walkRightSpriteSheet.TextureAtlas[3];
                    spriteBatch.Draw(currentWalkFrame, Position - _playerOffset, Color.White);
                    break;   
            }
        }
    }
}

