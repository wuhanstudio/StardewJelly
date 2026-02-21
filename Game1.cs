using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.ViewportAdapters;

namespace StardewJelly;

enum Dir
{
    Up,
    Down,
    Left,
    Right
}

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private OrthographicCamera _camera;
    
    Texture2D playerSprite;
    
    Texture2D walkUp;
    Texture2D walkDown;
    Texture2D walkLeft;
    Texture2D walkRight;
    
    private Texture2DAtlas walk_up_atlas;
    private Texture2DAtlas walk_down_atlas;
    private Texture2DAtlas walk_left_atlas;
    private Texture2DAtlas walk_right_atlas;
    
    private SpriteSheet walk_up_sprite_sheet;
    private SpriteSheet walk_down_sprite_sheet;
    private SpriteSheet walk_left_sprite_sheet;
    private SpriteSheet walk_right_sprite_sheet;

    private AnimationController walkUpAnimationController;
    private AnimationController walkDownAnimationController;
    private AnimationController walkLeftAnimationController;
    private AnimationController walkRightAnimationController;
    
    Texture2D background;
    Texture2D ball;
    Texture2D skull;
    
    Player player = new Player();
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        
        var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1280, 720);
        this._camera = new OrthographicCamera(viewportAdapter);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        playerSprite = Content.Load<Texture2D>("player/player");
        
        walkUp = Content.Load<Texture2D>("player/walkUp");
        walkDown = Content.Load<Texture2D>("player/walkDown");
        walkLeft = Content.Load<Texture2D>("player/walkLeft");
        walkRight = Content.Load<Texture2D>("player/walkRight");
        
        walk_up_atlas = Texture2DAtlas.Create("player/walkUpAtlas", walkUp, 96, 96);
        walk_down_atlas = Texture2DAtlas.Create("player/walkDownAtlas", walkDown, 96, 96);
        walk_left_atlas = Texture2DAtlas.Create("player/walkLeftAtlas", walkLeft, 96, 96);
        walk_right_atlas = Texture2DAtlas.Create("player/walkRightAtlas", walkRight, 96, 96);

        // Walk Up Animation
        walk_up_sprite_sheet = new SpriteSheet("SpriteSheet/walkup", walk_up_atlas);
        walk_up_sprite_sheet.DefineAnimation("walkup", builder =>
        {
            builder.IsLooping(true)
                .AddFrame(walk_up_atlas.GetRegion(0).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_up_atlas.GetRegion(1).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_up_atlas.GetRegion(2).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_up_atlas.GetRegion(3).Name, TimeSpan.FromSeconds(0.2));
        });
        SpriteSheetAnimation walkUpAnimation = walk_up_sprite_sheet.GetAnimation("walkup");
        walkUpAnimationController = new AnimationController(walkUpAnimation);
        
        // Walk Down Animation
        walk_down_sprite_sheet = new SpriteSheet("SpriteSheet/walkdown", walk_down_atlas);
        walk_down_sprite_sheet.DefineAnimation("walkdown", builder =>
        {
            builder.IsLooping(true)
                .AddFrame(walk_down_atlas.GetRegion(0).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_down_atlas.GetRegion(1).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_down_atlas.GetRegion(2).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_down_atlas.GetRegion(3).Name, TimeSpan.FromSeconds(0.2));
        });
        SpriteSheetAnimation walkDownAnimation = walk_down_sprite_sheet.GetAnimation("walkdown");
        walkDownAnimationController = new AnimationController(walkDownAnimation);

        // Walk Left Animation
        walk_left_sprite_sheet = new SpriteSheet("SpriteSheet/walkleft", walk_left_atlas);
        walk_left_sprite_sheet.DefineAnimation("walkleft", builder =>
        {
            builder.IsLooping(true)
                .AddFrame(walk_left_atlas.GetRegion(0).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_left_atlas.GetRegion(1).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_left_atlas.GetRegion(2).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_left_atlas.GetRegion(3).Name, TimeSpan.FromSeconds(0.2));
        });
        SpriteSheetAnimation walkLeftAnimation = walk_left_sprite_sheet.GetAnimation("walkleft");
        walkLeftAnimationController = new AnimationController(walkLeftAnimation);

        // Walk Right Animation
        walk_right_sprite_sheet = new SpriteSheet("SpriteSheet/walkright", walk_right_atlas);
        walk_right_sprite_sheet.DefineAnimation("walkright", builder =>
        {
            builder.IsLooping(true)
                .AddFrame(walk_right_atlas.GetRegion(0).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_right_atlas.GetRegion(1).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_right_atlas.GetRegion(2).Name, TimeSpan.FromSeconds(0.2))
                .AddFrame(walk_right_atlas.GetRegion(3).Name, TimeSpan.FromSeconds(0.2));
        });
        SpriteSheetAnimation walkRightAnimation = walk_right_sprite_sheet.GetAnimation("walkright");
        walkRightAnimationController = new AnimationController(walkRightAnimation);

        background = Content.Load<Texture2D>("background");
        ball = Content.Load<Texture2D>("ball");
        skull = Content.Load<Texture2D>("skull");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _camera.Position = player.Position - new Vector2(_graphics.PreferredBackBufferWidth / 2f, _graphics.PreferredBackBufferHeight / 2f);

        if (player.IsMoving)
        {
            switch (player.getDirection())
            {
                case Dir.Up:
                    walkUpAnimationController.Update(gameTime);
                    break;  
                case Dir.Down:
                    walkDownAnimationController.Update(gameTime);
                    break; 
                case Dir.Left:
                    walkLeftAnimationController.Update(gameTime);
                    break;   
                case Dir.Right:
                    walkRightAnimationController.Update(gameTime);
                    break;   
            }
        }
        
        player.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(transformMatrix: this._camera.GetViewMatrix());
        _spriteBatch.Draw(background, new Vector2(-500, -500), Color.White);
        if (player.IsMoving)
        {
            Texture2DRegion currentWalkFrame;
            switch (player.getDirection())
            {
                case Dir.Up:
                    walkUpAnimationController.Update(gameTime);
                    currentWalkFrame= walk_up_sprite_sheet.TextureAtlas[walkUpAnimationController.CurrentFrame];
                    _spriteBatch.Draw(currentWalkFrame, player.Position, Color.White);
                    break;  
                case Dir.Down:
                    walkDownAnimationController.Update(gameTime);
                    currentWalkFrame = walk_down_sprite_sheet.TextureAtlas[walkDownAnimationController.CurrentFrame];
                    _spriteBatch.Draw(currentWalkFrame, player.Position, Color.White);
                    break; 
                case Dir.Left:
                    walkLeftAnimationController.Update(gameTime);
                    currentWalkFrame = walk_left_sprite_sheet.TextureAtlas[walkLeftAnimationController.CurrentFrame];
                    _spriteBatch.Draw(currentWalkFrame, player.Position, Color.White);
                    break;   
                case Dir.Right:
                    walkRightAnimationController.Update(gameTime);
                    currentWalkFrame = walk_right_sprite_sheet.TextureAtlas[walkRightAnimationController.CurrentFrame];
                    _spriteBatch.Draw(currentWalkFrame, player.Position, Color.White);
                    break;   
            }
        }
        else
        {
            Texture2DRegion currentWalkFrame;
            switch (player.getDirection())
            {
                case Dir.Up:
                    currentWalkFrame= walk_up_sprite_sheet.TextureAtlas[3];
                    _spriteBatch.Draw(currentWalkFrame, player.Position, Color.White);
                    break;  
                case Dir.Down:
                    currentWalkFrame = walk_down_sprite_sheet.TextureAtlas[3];
                    _spriteBatch.Draw(currentWalkFrame, player.Position, Color.White);
                    break; 
                case Dir.Left:
                    currentWalkFrame = walk_left_sprite_sheet.TextureAtlas[3];
                    _spriteBatch.Draw(currentWalkFrame, player.Position, Color.White);
                    break;   
                case Dir.Right:
                    currentWalkFrame = walk_right_sprite_sheet.TextureAtlas[3];
                    _spriteBatch.Draw(currentWalkFrame, player.Position, Color.White);
                    break;   
            }
        }
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}