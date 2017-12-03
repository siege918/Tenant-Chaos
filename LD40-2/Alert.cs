using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    public class Alert
    {
        protected Game1 game;

        public static Texture2D tex_note;
        public static Texture2D tex_button;
        public static SpriteFont letterFont;
        protected TimeSpan expirationTime;
        TimeSpan currentTime;
        protected Apartment apartment;
        Rectangle bounds;
        protected Rectangle[] buttonBounds;
        protected Vector2[] textLocs;
        Vector2 greetingPos;
        Vector2 bodyPos;
        Vector2 farewellPos;
        protected string greeting;
        protected string body;
        protected string farewell;
        protected string[] choices;
        MouseState oldMouseState;
        int X;
        bool dismissed = false;

        string expiresIn;

        public bool Expired
        {
            get
            {
                return currentTime >= expirationTime;
            }
        }

        public bool Dismissed
        {
            get
            {
                return dismissed || Expired;
            }
        }

        public float PercentComplete
        {
            get
            {
                return 1f - (float)(expirationTime.TotalSeconds - currentTime.TotalSeconds) / (float)expirationTime.TotalSeconds;
            }
        }

        public Alert(Game1 Game, Apartment apartment, int X, Apartment toApartment = null)
        {
            game = Game;
            this.apartment = apartment;
            this.X = X;
            bounds = new Rectangle(X, 30, tex_note.Width, tex_note.Height);

            Initialize(apartment, toApartment);

            buttonBounds = new Rectangle[choices.Length];
            textLocs = new Vector2[choices.Length];

            Vector2 greetingSize = letterFont.MeasureString(greeting);
            Vector2 bodySize = letterFont.MeasureString(body);

            greetingPos = new Vector2(X + 50, 50);
            bodyPos = greetingPos + new Vector2(0, 50);
            farewellPos = greetingPos + new Vector2(0, 70) + (Vector2.UnitY * bodySize);


            int y = bounds.Bottom - 100;
            for (int i = choices.Length - 1; i >= 0; i--)
            {
                Vector2 textSize = letterFont.MeasureString(choices[i]);

                textLocs[i] = new Vector2(30 + X, y)
                    + ((new Vector2(tex_button.Bounds.Width, tex_button.Bounds.Height) - textSize) / 2);
                buttonBounds[i] = new Rectangle(30 + X, y, tex_button.Width, tex_button.Height);
                y -= 70;
            }
        }

        public virtual void Initialize(Apartment apartment, Apartment toApartment)
        {
            greeting = apartment.Greeting;
            farewell = apartment.Farewell + "\n" + apartment.TenantName + "\nApartment " + apartment.ApartmentNumber;
            body = WrapText(
                    "I am writing you today to tell you what a good job you're doing. I can't imagine a better landlord than you. Please have my children, thanks. Oh, and if you wouldn't mind, FIX MY FUCKING SINK, YOU SWINE."
                );
            choices = new string[] { "Ok", "Nah" };
            expirationTime = new TimeSpan(0, 0, 15);
        }

        public virtual void Update(GameTime gameTime, MouseState mouseState, Point mousePos, bool allowInteract, bool removed)
        {
            currentTime += gameTime.ElapsedGameTime;

            TimeSpan remainingTime = expirationTime - currentTime;

            expiresIn = "Expires in " + (int)remainingTime.TotalSeconds + " seconds.";

            if (PercentComplete >= 1f)
            {
                Expire();
            }
            else
            {
                if (oldMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released && allowInteract && !removed)
                {
                    for (int i = 0; i < choices.Length; i++)
                    {
                        if (buttonBounds[i].Contains(mousePos))
                        {
                            Answer(i);
                            break;
                        }
                    }
                }
            }
            oldMouseState = mouseState;

            if (removed)
            {
                Move(-50);
            }
        }

        public void Move(int X)
        {
            greetingPos.X += X;
            bodyPos.X += X;
            farewellPos.X += X;
            this.X += X;
            bounds.X += X;

            for (int i = choices.Length - 1; i >= 0; i--)
            {
                Vector2 textSize = letterFont.MeasureString(choices[i]);

                textLocs[i].X += X;
                buttonBounds[i].X += X;
            }
        }

        public virtual void Answer(int index)
        {
            Dismiss();
        }

        public bool Contains(Point point)
        {
            return bounds.Contains(point);
        }

        public void Draw(SpriteBatch spriteBatch, bool drawText = true)
        {
            spriteBatch.Draw(tex_note, bounds, Color.White);
            if (drawText)
            {
                spriteBatch.DrawString(letterFont, greeting, greetingPos, Color.Black);
                spriteBatch.DrawString(letterFont, body, bodyPos, Color.Black);
                spriteBatch.DrawString(letterFont, farewell, farewellPos, Color.Black);

                int y = bounds.Bottom - 100;
                for (int i = choices.Length - 1; i >= 0; i--)
                {
                    spriteBatch.Draw(tex_button, new Vector2(30 + X, y), Color.Tan);
                    spriteBatch.DrawString(letterFont, choices[i], textLocs[i], Color.Black);
                    y -= 70;
                }

                if (expiresIn != null)
                {
                    Vector2 measure = letterFont.MeasureString(expiresIn);
                    spriteBatch.DrawString(letterFont, expiresIn, new Vector2(bounds.Left + 10, bounds.Bottom + 10), Color.Red);
                }  
            }
        }

        public virtual void Expire()
        {
            Dismiss();
        }

        public virtual void Dismiss()
        {
            dismissed = true;
        }

        public static string WrapText(string text, int width = 700)
        {
            if (letterFont.MeasureString(text).X < width)
            {
                return text;
            }

            string[] words = text.Split(' ');
            StringBuilder wrappedText = new StringBuilder();
            float linewidth = 0f;
            float spaceWidth = letterFont.MeasureString(" ").X;
            for (int i = 0; i < words.Length; ++i)
            {
                Vector2 size = letterFont.MeasureString(words[i]);
                if (linewidth + size.X < width)
                {
                    linewidth += size.X + spaceWidth;
                }
                else
                {
                    wrappedText.Append("\n");
                    linewidth = size.X + spaceWidth;
                }
                wrappedText.Append(words[i]);
                wrappedText.Append(" ");
            }

            return wrappedText.ToString();
        }
    }
}
