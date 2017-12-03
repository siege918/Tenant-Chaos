using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LD40_2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int BUILDINGSIZE = 64;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SpriteFont debugFont;
        public static SpriteFont buttonFont;
        public static SpriteFont plaqueFont;
        public static Texture2D tex_wall;
        public static Texture2D tex_window;
        public static Texture2D tex_UI;
        public static Texture2D tex_button;
        public static Texture2D tex_transparency;
        public static Texture2D tex_buildingTop;
        public static Texture2D tex_porch;
        public static Texture2D tex_dirt;
        public static Texture2D tex_grass;
        public static Song bgm;
        int fps = 0;
        public static Random rand;
        Apartment[,] apartments;
        RenderTarget2D gameArea;
        Rectangle gameBounds;
        Vector2 aspectVector;
        Vector2 mousePos;
        Vector2 mouseWorldPos;
        Point mousePoint;

        int money = 0;

        public int Money
        {
            get
            {
                return money;
            }
        }

        int buildingCost = 200;

        public int BuildingCost
        {
            get
            {
                return buildingCost;
            }
        }

        float costModifier = 1.5f;

        MouseState oldMouseState;

        Rectangle buyButtonRect;
        Vector2 buyButtonTextLoc;
        Vector2 dateLoc;
        Vector2 priceLoc;

        int buildingWidth = 1;
        int buildingHeight = 1;
        bool stopBuilding = false;

        bool timeIsPassing = false;
        Calendar calendar = new Calendar();
        TimeSpan runtime;

        List<Alert> Alerts;

        Camera2D camera;
        int numApartments = 0;
        int rentMilestone = 1;

        public int alertX = 30;

        bool gameOver = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            rand = new Random();
            apartments = new Apartment[BUILDINGSIZE,BUILDINGSIZE];
            base.Initialize();
            Window.AllowUserResizing = true;
            camera = new Camera2D(new Viewport(0, 0, 1920, 1080));
            camera.Location = new Vector2(0, -140);
            camera.Zoom = 1;
            aspectVector = new Vector2(1, 1);
            gameArea = new RenderTarget2D(GraphicsDevice, 1920, 1080);
            runtime = new TimeSpan();
            Alerts = new List<Alert>();
            AddAlert(new TutorialAlert1(this, null, alertX));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            debugFont = Content.Load<SpriteFont>("DebugFont");
            buttonFont = Content.Load<SpriteFont>("ButtonFont");
            plaqueFont = Content.Load<SpriteFont>("PlaqueFont");
            bgm = Content.Load<Song>("SocialMediaJAM");
            Alert.letterFont = Content.Load<SpriteFont>("LetterFont");
            Alert.tex_note = Content.Load<Texture2D>("note");
            Alert.tex_button = Content.Load<Texture2D>("answerButton");
            tex_wall = Content.Load<Texture2D>("Wall");
            Apartment.tex_wall = tex_wall;
            tex_window = Content.Load<Texture2D>("window");
            Apartment.tex_window = tex_window;
            Apartment.tex_plaque = Content.Load<Texture2D>("plaque");
            tex_UI = Content.Load<Texture2D>("UI");
            tex_button = Content.Load<Texture2D>("Button");
            buyButtonRect = new Rectangle(30, 960, tex_button.Width, tex_button.Height);
            Vector2 buySize = buttonFont.MeasureString("Buy");
            tex_transparency = Content.Load<Texture2D>("transparency");
            buyButtonTextLoc = new Vector2(buyButtonRect.X, buyButtonRect.Y)
                + ((new Vector2(buyButtonRect.Width, buyButtonRect.Height) - buySize) / 2);
            tex_buildingTop = Content.Load<Texture2D>("buildingTopper");
            tex_porch = Content.Load<Texture2D>("porch");
            tex_dirt = Content.Load<Texture2D>("dirt");
            tex_grass = Content.Load<Texture2D>("grass");
            dateLoc = new Vector2(1600, 975);
            priceLoc = new Vector2(400, 1010);

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(bgm);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0;
            }

            //apartments[0, 0] = new Apartment(new Vector2(0, 0), new Point(0, 0), calendar);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            if (MediaPlayer.Volume < 1)
            {
                MediaPlayer.Volume += .005f;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            int top = 0;
            int left = 0;
            int width = 1920;
            int height = 1080;

            float ratio = (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height;
            float idealRatio = 16f / 9f;

            if (ratio <= idealRatio)
            {
                height = (int)((Window.ClientBounds.Width / idealRatio) + 0.5f);
                width = GraphicsDevice.Viewport.Width;
                top = (GraphicsDevice.Viewport.Height - height) / 2;
            }
            else
            {
                width = (int)((Window.ClientBounds.Height * idealRatio) + 0.5f);
                height = GraphicsDevice.Viewport.Height;
                left = (GraphicsDevice.Viewport.Width - width) / 2;
            }

            gameBounds = new Rectangle(left, top, width, height);

            if (gameOver)
            {
                base.Update(gameTime);
                return;
            }

            MouseState mouseState = Mouse.GetState();

            mousePos = new Vector2(mouseState.Position.X, mouseState.Position.Y);
            mouseWorldPos = Vector2.Transform(mousePos, Matrix.Invert(camera.TransformMatrix));

            mousePoint = mouseState.Position;
            mousePoint.X -= left;
            mousePoint.Y -= top;
            mousePoint.X = (int)(mousePoint.X * 1920f / (float)width);
            mousePoint.Y = (int)(mousePoint.Y * 1080f / (float)height);

            int debug_width = (int) ((float)width * (1920f / (float)width));

            if (buyButtonRect.Contains(mousePoint) && oldMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                if (money >= buildingCost)
                {
                    BuildBuilding();
                    money -= buildingCost;
                    buildingCost = (int)(buildingCost * costModifier);
                    costModifier += .02f;
                }
            }

            bool interactWithCamera = true;

            if (oldMouseState.MiddleButton == ButtonState.Pressed && mouseState.MiddleButton == ButtonState.Released)
            {
                AddAlert(new Alert(this, apartments[0, 0], alertX));
            }

            bool removed = false;
            for (int i = 0; i < Alerts.Count; i++)
            {
                if (Alerts[i].Contains(mousePoint))
                {
                    interactWithCamera = false;
                }
                Alerts[i].Update(gameTime, mouseState, mousePoint, i == 0, removed);

                if (Alerts[i].Dismissed)
                {
                    Alerts.RemoveAt(i);
                    alertX -= 50;
                    removed = true;
                    i--;
                }
            }

            camera.Update(mouseState.ScrollWheelValue, mousePoint, mouseState, interactWithCamera);

            if (timeIsPassing)
            {
                runtime += gameTime.ElapsedGameTime;
                calendar.Update(runtime.TotalSeconds);
            }

            for (int x = 0; x < BUILDINGSIZE; x++)
            {
                for (int y = 0; y < BUILDINGSIZE; y++)
                {
                    if (apartments[x, y] == null)
                        break;

                    Alert alert = apartments[x, y].Update(this, apartments, alertX);
                    if (alert != null)
                    {
                        AddAlert(alert);
                    }

                    if (apartments[x,y].IsOccupied && calendar.DateChanged)
                    {
                        money += apartments[x, y].Rent;
                    }

                    if (apartments[x, y].isMouseIn(mousePoint) && mouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Released)
                    {
                        //Display tooltip about building
                    }
                }
            }

            oldMouseState = mouseState;

            if (money < 0)
                gameOver = true;


            if (numApartments == (64 * 64))
            {
                gameOver = true;
            }

            base.Update(gameTime);
        }

        public void AddAlert(Alert alert)
        {
            Alerts.Add(alert);
            alertX += 50;
        }

        public void StartTime()
        {
            timeIsPassing = true;
        }

        public void IncreaseRent(double rate)
        {
            Apartment.baseRent += (int)(Apartment.baseRent * rate);
        }

        public void ModifyTrust(double rate)
        {
            foreach (Apartment apartment in apartments)
            {
                if (apartment != null)
                {
                    apartment.ModifyTrust(rate);
                }
            }
        }

        public void ContinueToNextMilestone()
        {
            rentMilestone++;
        }

        public void GiveMoney(int money)
        {
            this.money += money;
        }

        private void BuildBuilding()
        {
            if (stopBuilding)
            {
                return;
            }

            if (buildingWidth > BUILDINGSIZE || buildingHeight > BUILDINGSIZE)
            {
                stopBuilding = true;
                return;
            }

            numApartments++;

            if (numApartments == 5 * rentMilestone)
            {
                AddAlert(new RentIncreaseAlert(this, null, alertX));
            }

            if (buildingWidth > buildingHeight)
            {
                for (int i = 0; i < buildingHeight; i++)
                {
                    if (apartments[buildingWidth - 1, i] == null)
                    {
                        apartments[buildingWidth - 1, i] = new Apartment(
                            new Vector2((buildingWidth - 1) * Apartment.width, -1 *i * Apartment.height),
                            new Point(buildingWidth - 1, i),
                            calendar
                        );
                        return;
                    }
                }
                //If not returned, we need to swivel
                buildingHeight++;
                if (buildingHeight <= BUILDINGSIZE)
                {
                    apartments[0, buildingHeight - 1] = new Apartment(
                        new Vector2(0, -1 * (buildingHeight - 1) * Apartment.height),
                        new Point(0, buildingHeight - 1),
                        calendar
                    );
                }
            }
            else
            {

                for (int i = 0; i < buildingWidth; i++)
                {
                    if (apartments[i, buildingHeight - 1] == null)
                    {
                        apartments[i, buildingHeight - 1] = new Apartment(
                            new Vector2(i * Apartment.width, -1 * (buildingHeight - 1) * Apartment.height),
                            new Point(i, buildingHeight - 1),
                            calendar
                        );
                        return;
                    }
                }
                //If not returned, we need to swivel
                buildingWidth++;
                if (buildingWidth <= BUILDINGSIZE)
                {
                    apartments[buildingWidth - 1, 0] = new Apartment(
                        new Vector2((buildingWidth - 1) * Apartment.width, 0),
                        new Point(buildingWidth - 1, 0),
                        calendar 
                    );
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            fps = (int)System.Math.Ceiling(1d / gameTime.ElapsedGameTime.TotalSeconds);

            GraphicsDevice.SetRenderTarget(gameArea);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Gameplay layer
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);

            spriteBatch.Draw(tex_dirt, new Rectangle(-9999999, 0, 99999999, 99999999), Color.White);

            for (int i = -50; i < 50; i++)
            {
                spriteBatch.Draw(tex_grass, new Vector2(i * 4096, 0), Color.White);
            }

            spriteBatch.Draw(tex_porch, Vector2.One * -256, Color.White);

            for (int x = 0; x < BUILDINGSIZE; x++)
            {
                int y = 0;
                while (y < BUILDINGSIZE)
                {
                    if (apartments[x,y] != null)
                    {
                        apartments[x, y].Draw(spriteBatch, camera);
                    }
                    else
                    {
                        break;
                    }
                    y++;
                }

                if (y == 0)
                    continue;

                Vector2 ceilingLoc = new Vector2((x * Apartment.width) - 22, -1 * ((y) * Apartment.height) - 15);
                spriteBatch.Draw(tex_buildingTop, ceilingLoc, Color.White);
            }
            spriteBatch.End();
            

            //UI layer
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //spriteBatch.DrawString(debugFont, "FPS: " + fps + "\nZoom: " + camera.Zoom + "\nCamera Pos: " + camera.Location, Vector2.Zero, Color.White);
            spriteBatch.Draw(tex_UI, new Vector2(0, 930), null, null, null, 0, null, Color.White, SpriteEffects.None, 0);
            spriteBatch.Draw(tex_UI, new Vector2(960, 930), null, null, null, 0, null, Color.White, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(tex_button, buyButtonRect, Color.White);
            spriteBatch.DrawString(buttonFont, calendar.ToString(), dateLoc, Color.Black);
            spriteBatch.DrawString(buttonFont, "Price: $" + buildingCost, priceLoc, Color.Black);
            spriteBatch.DrawString(buttonFont, "Buy", buyButtonTextLoc, Color.Black);
            spriteBatch.DrawString(buttonFont, "$" + money, new Vector2((1920 / 2) - (buttonFont.MeasureString("$" + money).X / 2), 940), Color.Black);

            int alertsToDraw = Alerts.Count - 1 > 15 ? 15 : Alerts.Count - 1;
            int X = alertsToDraw * 50;
            for (int i = alertsToDraw; i >= 0; i--)
            {
                Alerts[i].Draw(spriteBatch, i == 0);
                X -= 50;
            }

            if (gameOver)
            {
                spriteBatch.Draw(tex_transparency, gameArea.Bounds, Color.White);
                spriteBatch.Draw(Alert.tex_note, new Rectangle(710, 290, 500, 500), Color.White);
                string gameOverString = Alert.WrapText("Game Over!\n\nYou lasted " + (int)calendar.MonthsSince(new Calendar.Date()) + " months and built " + numApartments + " apartments!\n\nYou may now close the game.", 400);
                spriteBatch.DrawString(Alert.letterFont, gameOverString, new Vector2(730, 320), Color.Black);
            }

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(gameArea, gameBounds, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
