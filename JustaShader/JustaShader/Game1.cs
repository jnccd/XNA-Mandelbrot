using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace JustaShader
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int Timer;
        float X;
        float Y;
        float Zoom = 10;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (true)
            {
                graphics.IsFullScreen = true;
                Values.WindowSize.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Values.WindowSize.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            }

            graphics.PreferredBackBufferWidth = Values.WindowSize.X;
            graphics.PreferredBackBufferHeight = Values.WindowSize.Y;
            IsMouseVisible = true;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Assets.Load(Content, GraphicsDevice);
        }
        
        protected override void Update(GameTime gameTime)
        {
            Control.Update();
            FPSCounter.Update(gameTime);

            if (Control.CurKS.IsKeyDown(Keys.Escape))
                Exit();

            float speed = (Zoom / 5f);

            if (Control.CurKS.IsKeyDown(Keys.S))
                Y -= speed;

            if (Control.CurKS.IsKeyDown(Keys.W))
                Y += speed;

            if (Control.CurKS.IsKeyDown(Keys.D))
                X -= speed;

            if (Control.CurKS.IsKeyDown(Keys.A))
                X += speed;

            if (Control.CurMS.ScrollWheelValue > Control.LastMS.ScrollWheelValue)
                Zoom -= Zoom / 10;

            if (Control.CurMS.ScrollWheelValue < Control.LastMS.ScrollWheelValue)
                Zoom += Zoom / 10;

            Timer++;

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            Assets.Shader.Parameters["MatrixTransform"].SetValue(halfPixelOffset * projection);
            Assets.Shader.Parameters["timer"].SetValue(Timer);
            Assets.Shader.Parameters["camX"].SetValue(X / 30f);
            Assets.Shader.Parameters["camY"].SetValue(Y / 30f);
            Assets.Shader.Parameters["mouseWheel"].SetValue(Zoom);
            Assets.Shader.Parameters["mouseX"].SetValue(Control.CurMS.X / (float)Values.WindowSize.X - 0.5f);
            Assets.Shader.Parameters["mouseY"].SetValue(Control.CurMS.Y / (float)Values.WindowSize.Y - 0.5f);

            spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Assets.Shader);

            spriteBatch.Draw(Assets.White, new Rectangle(0, 0, Values.WindowSize.X, Values.WindowSize.Y), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
