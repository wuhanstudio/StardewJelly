using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended.Collisions.QuadTree;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;

namespace StardewJelly;

public class MainMenuScreen : GameScreen
{
    private OrthographicCamera _camera;
    private KeyboardState _prev_keystate;
    
    private Texture2D _background;
    private Texture2D ball;
    private Texture2D skull;
    
    private CollisionComponent _collisionComponent;
    private Pool<Enemy> _enemyPool;
    private List<IEntity> _entities = new List<IEntity>();
    private List<Enemy> _enemies = new List<Enemy>();

    Player player;

    private bool gameStarted = true;

    public MainMenuScreen(Game game) : base(game)
    {
        var viewportAdapter = new BoxingViewportAdapter(Game.Window, GraphicsDevice, 1280, 720);
        _camera = new OrthographicCamera(viewportAdapter);
    }

    public override void LoadContent()
    {
        base.LoadContent();
        _background = Content.Load<Texture2D>("background");

        Texture2D walkUp = Content.Load<Texture2D>("player/walkUp");
        Texture2D walkDown = Content.Load<Texture2D>("player/walkDown");
        Texture2D walkLeft = Content.Load<Texture2D>("player/walkLeft");
        Texture2D walkRight = Content.Load<Texture2D>("player/walkRight");
        
        ball = Content.Load<Texture2D>("ball");
        skull = Content.Load<Texture2D>("skull");

        player = new Player(walkUp, walkDown, walkLeft, walkRight);
        
        _enemyPool = new Pool<Enemy>(
            createItem: () => new Enemy(skull),      // Function that will be executed when we need to create a new Enemy
            resetItem: enemy => enemy.Reset(),  // Method that will be executed when the Enemy is returned to the pool for re-use
            capacity: 10                        // Maximum pool capacity, can not grow
        );
        
        Reset();
    }

    private void Reset()
    {
        _entities.Clear();
        player.Reset();
        
        // Collision layer
        QuadTreeSpace quarTreeSpace = new QuadTreeSpace(new RectangleF(-500, -500, 2496, 2496));
        Layer defaultQuadLayer = new Layer(quarTreeSpace);
        _collisionComponent = new CollisionComponent(defaultQuadLayer);
        
        for (int i = 0; i < 10; i++)
        {
            Enemy enemy = _enemyPool.Obtain();
            _collisionComponent.Insert(enemy);
            _enemies.Add(enemy);
            _entities.Add(enemy);
        }
        
        _collisionComponent.Insert(player);
        _entities.Add(player);
        
        gameStarted = true;
    }
    
    public override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.R))
            Reset();
        
        if (gameStarted)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed ||
                (keyboardState.IsKeyUp(Keys.Space) && _prev_keystate.IsKeyDown(Keys.Space))
                )
            {
                Ball weapon = new Ball(ball);
                Player.Instance.weapons.Add(weapon);
                _collisionComponent.Insert(weapon);
                _entities.Add(weapon);
            }

            _prev_keystate = keyboardState;
            _camera.Position = player.Position - new Vector2(Game.GraphicsDevice.Viewport.Width / 2f, Game.GraphicsDevice.Viewport.Height / 2f);
            
            // Make sure each entity moves around the screen
            foreach (IEntity entity in _entities)
            {
                entity.Update(gameTime);
            }

            if (player.Dead())
            {
                gameStarted = false;
            }

            var deadEnemies = _enemies.Where(e => e.Dead()).ToList();
            foreach (Enemy enemy in deadEnemies)
            {
                if (enemy.Dead())
                {
                    _enemies.Remove(enemy);
                    _collisionComponent.Remove(enemy);
                }
            }
            
            var deadWeapons = Player.Instance.weapons.Where(e => e.Dead()).ToList();
            foreach (Ball weapon in deadWeapons)
            {
                if (weapon.Dead())
                {
                    Player.Instance.weapons.Remove(weapon);
                    _collisionComponent.Remove(weapon);
                }
            }
            
            // Make sure all collisions are detected and the OnCollision event for each is called
            _collisionComponent.Update(gameTime);
        }
        else
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Enter))
                Reset();
        }
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
        spriteBatch.Draw(_background, new Vector2(-500, -500), Color.White);
        foreach (IEntity entity in _entities)
        {
            entity.Draw(spriteBatch);
        }
        spriteBatch.End();
    }
}