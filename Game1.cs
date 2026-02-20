using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Comora;
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
    
    private Camera camera;
    
    Texture2D playerSprite;
    Texture2D walkUp;
    Texture2D walkDown;
    Texture2D walkLeft;
    Texture2D walkRight;
    
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
        
        this.camera = new Camera(_graphics.GraphicsDevice);
        
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
        player.Update(gameTime);

        this.camera.Position = player.Position;
        this.camera.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(this.camera);
        _spriteBatch.Draw(background, new Vector2(-500, -500), Color.White);
        _spriteBatch.Draw(playerSprite, player.Position, Color.White);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}