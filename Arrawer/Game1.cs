using DopaEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arrawer
{
    public class Game1 : DEWorld
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont defaultFont;
        private GameDI arrowGame = new ArrowGame();
        public Game1()
        {
        }

        public override void OnLoadContent()
        {
            base.OnLoadContent();
            defaultFont = DE.Get().DefaultFont;
            arrowGame.ContentLoad(DE.Get().VM.Content);
            ((ArrowGame)arrowGame).font = defaultFont;

        }
        public override void OnRender(GameTime gameTime, SpriteBatch SpriteBatch)
        {
            base.OnRender(gameTime, SpriteBatch);
            arrowGame.OnDraw(gameTime, SpriteBatch);
        }

        public override void OnUpdate(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                DE.Get().Exit();
            base.OnUpdate(gameTime);
            arrowGame.OnUpdate(gameTime);
        }
    }
}