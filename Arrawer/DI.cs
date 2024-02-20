using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrawer
{
    interface GameDI
    {
        public void OnUpdate(GameTime gameTime);
        public void OnDraw(GameTime gameTime, SpriteBatch spriteBatch);
        public virtual void ContentLoad(ContentManager Content) { }
    }
}
