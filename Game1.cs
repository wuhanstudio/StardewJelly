using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended.Collisions.QuadTree;
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

    private Texture2D _background;
    private Texture2D ball;
    private Texture2D skull;
    
    private CollisionComponent _collisionComponent;
    private readonly List<IEntity> _entities = new List<IEntity>();

    Player player;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        
        var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1280, 720);
        this._camera = new OrthographicCamera(viewportAdapter);
        
        // Collision layer
        QuadTreeSpace quarTreeSpace = new QuadTreeSpace(new RectangleF(-500, -500, 2496, 2496));
        Layer defaultQuadLayer = new Layer(quarTreeSpace);
        _collisionComponent = new CollisionComponent(defaultQuadLayer);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _background = Content.Load<Texture2D>("background");

        Texture2D walkUp = Content.Load<Texture2D>("player/walkUp");
        Texture2D walkDown = Content.Load<Texture2D>("player/walkDown");
        Texture2D walkLeft = Content.Load<Texture2D>("player/walkLeft");
        Texture2D walkRight = Content.Load<Texture2D>("player/walkRight");

        player = new Player(walkUp, walkDown, walkLeft, walkRight);

        ball = Content.Load<Texture2D>("ball");
        skull = Content.Load<Texture2D>("skull");
        
        Pool<Enemy> enemyPool = new Pool<Enemy>(
            createItem: () => new Enemy(skull),      // Function that will be executed when we need to create a new Enemy
            resetItem: enemy => enemy.Reset(),  // Method that will be executed when the Enemy is returned to the pool for re-use
            capacity: 10                        // Maximum pool capacity, can not grow
        );
        
        for (int i = 0; i < 10; i++)
        {
            Enemy enemy = enemyPool.Obtain();
            _collisionComponent.Insert(enemy);
            _entities.Add(enemy);
        }
        
        _collisionComponent.Insert(player);
        _entities.Add(player);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _camera.Position = player.Position - new Vector2(_graphics.PreferredBackBufferWidth / 2f, _graphics.PreferredBackBufferHeight / 2f);
        
        // Make sure each entity moves around the screen
        foreach (IEntity entity in _entities)
        {
            entity.Update(gameTime);
        }

        // Make sure all collisions are detected and the OnCollision event for each is called
        _collisionComponent.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(transformMatrix: this._camera.GetViewMatrix());
        _spriteBatch.Draw(_background, new Vector2(-500, -500), Color.White);
        foreach (IEntity entity in _entities)
        {
            entity.Draw(_spriteBatch);
        }
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}