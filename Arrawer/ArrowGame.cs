using DopaEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrawer
{
    internal class ArrowGame : GameDI
    {
        public SpriteFont font;
        public DEUIObject leftImage, HPImage, HPIndicatorImage, hitSFX;
        bool left = false, right = false, top = false, bottom = false, space = false;
        int level = 1;
        int score = 0;
        List<string> arrows = new();
        List<string> arrowsInput = new();

        int initialSizeOfHealth = 800;
        int sizeOfHealth = 800;
        int health = 1000;
        int initialHealth = 1000;

        void GameDI.OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (arrows.Count > 0)
            {
                int yPos = leftImage.Transform.Width;
                int itemIndex = 0;
                int center = leftImage.Transform.Width / 2;
                int arrowSize = (leftImage.Transform.Width + 12) / 2;
                spriteBatch.DrawString(font, $"Score {score} - {sizeOfHealth} - {health}", new(15, 15), Color.White);
                arrows.ForEach((item) =>
                {
                    DEBaseObject arrowImage = leftImage;
                    int rotation = 0;
                    if (item != arrowsInput[itemIndex])
                    {

                        if (item == "right")
                            arrowImage.SpriteLocation = new(32 * 2, 0, 32, 32);
                        else if (item == "left")
                            arrowImage.SpriteLocation = new(32 * 0, 0, 32, 32);
                        else if (item == "top")
                            arrowImage.SpriteLocation = new(32 * 1, 0, 32, 32);
                        else if (item == "bottom")
                            arrowImage.SpriteLocation = new(32 * 3, 0, 32, 32);
                    } else
                    {
                        if (item == "right")
                            arrowImage.SpriteLocation = new(32 * 2, 32 * 1, 32, 32);
                        else if (item == "left")
                            arrowImage.SpriteLocation = new(32 * 0, 32 * 1, 32, 32);
                        else if (item == "top")
                            arrowImage.SpriteLocation = new(32 * 1, 32 * 1, 32, 32);
                        else if (item == "bottom")
                            arrowImage.SpriteLocation = new(32 * 3, 32 * 1, 32, 32);
                    }
                    arrowImage.SetPosition(
                        ((DE.centerWindow - (arrowSize * level)) - center) + yPos, 
                        DE.Get().VM._graphics.PreferredBackBufferHeight - 128);
                    arrowImage.OnRender(gameTime, spriteBatch);
                    yPos += arrowImage.Transform.Width + 12;
                    itemIndex += 1;
                });
            }

            HPImage.OnRender(gameTime, spriteBatch);
            HPIndicatorImage.OnRender(gameTime, spriteBatch);
            hitSFX.OnRender(gameTime, spriteBatch);

        }

        void GameDI.ContentLoad(ContentManager Content)
        {
            int size = 48;
            leftImage = new("ayo", Vector2.Zero, new(size, size)); //= Content.Load<Texture2D>("Left");

            initialSizeOfHealth = (int) (800 * 0.75f);
            sizeOfHealth = initialSizeOfHealth;
            int height = (int) (64 * 0.75f);
            int locY = DE.Get().VM._graphics.PreferredBackBufferHeight - 128;
            HPImage = new DEUIObject(
                "HP", 
                new(DE.centerWindow - (initialSizeOfHealth / 2),
                locY - 64), 
                new(initialSizeOfHealth, height));
            HPIndicatorImage = new DEUIObject("HP", 
                new(DE.centerWindow - (initialSizeOfHealth / 2) + 5, 
                (locY - 64) + 4), 
                new(initialSizeOfHealth - 8, height - 9));
            HPImage.ObjectScalingValue = 1f;
            HPImage.SpriteLocation = new(0, 0, 800, 64);
            HPIndicatorImage.SpriteLocation = new(7, 70, 789, 50);

            hitSFX = new("SFX-Swinged", Vector2.Zero, new(64 * 3, 16 * 3));
            hitSFX.SpriteLocation = new(192, 0, 64, 16);
        }

        void GameDI.OnUpdate(GameTime gameTime)
        {
            HPIndicatorImage.OnUpdate(gameTime);
            hitSFX.OnUpdate(gameTime);
            if (arrows.Count == 0)
            {
                arrowsInput = new();
                int i = 0;
                while (i < level)
                {
                    int randomArrowIndex = Random.Shared.Next(3);
                    if (randomArrowIndex == 0)
                        arrows.Add("left");
                    if (randomArrowIndex == 1)
                        arrows.Add("top");
                    if (randomArrowIndex == 2)
                        arrows.Add("bottom");
                    if (randomArrowIndex == 3)
                        arrows.Add("right");
                    arrowsInput.Add("");
                    i++;
                };
                return;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) && !left)
            {
                left = true;
                var index = arrowsInput.FindIndex(0, (pred) => pred == null || pred == "");
                FillArrowInput(index, "left");
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Left) && left) left = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !right)
            {
                right = true;
                var index = arrowsInput.FindIndex(0, (pred) => pred == null || pred == "");
                FillArrowInput(index, "right");
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Right) && right) right = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !top)
            {
                top = true;
                var index = arrowsInput.FindIndex(0, (pred) => pred == null || pred == "");
                FillArrowInput(index, "top");
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Up) && top) top = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !bottom)
            {
                bottom  = true;
                var index = arrowsInput.FindIndex(0, (pred) => pred == null || pred == "");
                FillArrowInput(index, "bottom");
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Down) && bottom) bottom = false;


            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !space)
            { 
                space = true;
                if (arrowsInput.Find((pred) => pred == "") == null)
                {

                    arrows = new();
                    if (level >= 8)
                    {
                        level = 1;
                    }
                    else
                        level++;

                    if (health <= 5)
                    {
                        health = 0;
                        initialHealth = 2000;
                        updateHealth(2000);
                    } else
                    {
                        score += level * 250;
                        updateHealth(-150);
                    }
                    hitSFX.ResetAnimation();
                    var animationSFX = new DEAnimation(hitSFX);
                    animationSFX.SetAnimationSprite(new()
                    {
                        new(0, 0),
                        new(64, 0),
                        new(128, 0),
                        new(192, 0)
                    });
                    hitSFX.AddAnimation(animationSFX);
                    animationSFX.StartAnimate();
                }
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space) && space) space = false;
        }

        private void FillArrowInput(int index, string key)
        {
            if (index < 0) return;
            bool allValid = true;
            int itemIndex = 0;
            arrowsInput[index] = key;
            arrows.ForEach((item) =>
            {
                var arrowInput = arrowsInput[itemIndex];
                if (item != arrowInput && arrowInput != "")
                    allValid = false;
                itemIndex += 1;
            });
            if (!allValid)
            {
                arrowsInput = new();
                arrows.ForEach((_) =>
                {
                    arrowsInput.Add("");
                });
            }
        }

        private void updateHealth(int value)
        {
            health += value;

            if (health < 0) health = 5;

            sizeOfHealth = (int)(((float)health / (float)initialHealth) * (float)initialSizeOfHealth);

            HPIndicatorImage.ResetAnimation();
            var animationHP = new DEAnimation(HPIndicatorImage);
            animationHP.SetAnimationSize(
                HPIndicatorImage.Transform.Size.ToVector2(),
                new Vector2(sizeOfHealth, 0));
            HPIndicatorImage.AddAnimation(animationHP);
            animationHP.StartAnimate();
        }
    }
}
