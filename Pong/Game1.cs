using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D rightBar, leftBar;
        Texture2D ball;

        Vector2 rightPosition;
        Vector2 leftPosition;
        Vector2 ballPosition;

        int barWidth = 20;
        int barHeight = 200;
        int ballSize = 20;
        int barSpeed = 5;
        Vector2 ballVelocity = new Vector2(-3, -3);

        Rectangle view;

        SoundEffect goalSound, tapSound;
        Song backSound;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rightBar = Content.Load<Texture2D>("assets/bar");
            leftBar = Content.Load<Texture2D>("assets/bar");
            ball = Content.Load<Texture2D>("assets/ball");

            goalSound = Content.Load <SoundEffect>("assets/goal");
            tapSound = Content.Load <SoundEffect>("assets/tap");
            backSound = Content.Load <Song>("assets/back");

            MediaPlayer.Play(backSound);
            MediaPlayer.Volume = 0.1f;
            

            // TODO: use this.Content to load your game content here

            view = GraphicsDevice.Viewport.Bounds;

            //x = view.X + 20
            //y = (altura da view / 2) - (altura da imagem / 2)
            leftPosition = new Vector2(view.X + 20, view.Height / 2 - barHeight / 2);
            rightPosition = new Vector2(view.Width - 40, view.Height / 2 - barHeight / 2);

            //x = (largura do campo de visão / 2) - (largura da bola / 2)
            //y = (altura do campo de visão / 2) - (altura da bola / 2)
            ballPosition = new Vector2(view.Width / 2 - ballSize, view.Height / 2 - ballSize);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState state = Keyboard.GetState();

            //Verificação do teclado
            if (state.IsKeyDown(Keys.W))
                leftPosition.Y -= barSpeed;
            else if (state.IsKeyDown(Keys.S))
                leftPosition.Y += barSpeed;

            if (state.IsKeyDown(Keys.Up))
                rightPosition.Y -= barSpeed;
            else if (state.IsKeyDown(Keys.Down))
                rightPosition.Y += barSpeed;

            //Verificação das posições das barras
            if(leftPosition.Y < view.Y) 
                leftPosition.Y = view.Y;
            else if (leftPosition.Y + barHeight > view.Height)
                leftPosition.Y = view.Height - barHeight;

            if (rightPosition.Y < view.Y)
                rightPosition.Y = view.Y;
            else if (rightPosition.Y + barHeight > view.Height)
                rightPosition.Y = view.Height - barHeight;

            //Verificação da bola
            ballPosition += ballVelocity;

            if(ballPosition.Y < view.Y || ballPosition.Y + ballSize > view.Height) 
            {
                ballVelocity.Y *= -1;
            }            
            
            if(ballPosition.X < view.X || ballPosition.X + ballSize > view.Width) 
            {
                ballPosition = new Vector2(view.Width / 2 - ballSize, view.Height / 2 - ballSize);

                if(ballVelocity.X > 0)
                {
                    ballVelocity = new Vector2(-3, -3);
                }
                else
                {
                    ballVelocity = new Vector2(3, 3);
                }

                leftPosition = new Vector2(view.X + 20, view.Height / 2 - barHeight / 2);
                rightPosition = new Vector2(view.Width - 40, view.Height / 2 - barHeight / 2);

                goalSound.Play();
            }

            //Verificação de colisão
            Rectangle leftRectangle = new Rectangle(leftPosition.ToPoint(), new Point(barWidth, barHeight));
            Rectangle rightRectangle = new Rectangle(rightPosition.ToPoint(), new Point(barWidth, barHeight));
            Rectangle ballRectangle = new Rectangle(ballPosition.ToPoint(), new Point(ballSize, ballSize));

            if (ballRectangle.Intersects(leftRectangle) || ballRectangle.Intersects(rightRectangle)) 
            {
                ballVelocity.X *= -1;
                
                if(ballVelocity.X > 0) 
                {
                    ballVelocity.X += 0.5f;
                }
                else
                {
                    ballVelocity.X -= 0.5F;
                }

                tapSound.Play();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(leftBar, leftPosition, Color.White);
            _spriteBatch.Draw(rightBar, rightPosition, Color.White);
            _spriteBatch.Draw(ball, ballPosition, Color.White);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}