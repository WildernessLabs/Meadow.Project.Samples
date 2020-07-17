using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Hid;

namespace Tetris
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Max7219 display;
        GraphicsLibrary graphics;

        AnalogJoystick joystick;

        TetrisGame game = new TetrisGame(8, 24);


        public MeadowApp()
        {
            Console.WriteLine("Tetris");

            Init();

            Console.WriteLine("Start game");
            StartGameLoop();
        }

        int tick = 0;
        void StartGameLoop()
        {
            while (true)
            {
                tick++;
                CheckInput(tick);

                graphics.Clear();
                DrawTetrisField();
                graphics.Show();

                Thread.Sleep(50);
            }
        }

        async Task CheckInput(int tick)
        {
            if (tick % (21 - game.Level) == 0)
            {
                game.OnDown(true);
            }

            var pos = await joystick.GetPosition();

            if (pos == AnalogJoystick.DigitalJoystickPosition.Left)
            {
                game.OnLeft();
            }
            if (pos == AnalogJoystick.DigitalJoystickPosition.Right)
            {
                game.OnRight();
            }
            if (pos == AnalogJoystick.DigitalJoystickPosition.Up)
            {
                game.OnRotate();
            }
            if (pos == AnalogJoystick.DigitalJoystickPosition.Down)
            {
                game.OnDown();
            } 
        }

        void DrawTetrisField()
        {
            graphics.DrawText(0, 0, $"{game.LinesCleared}");

            int yOffset = 8;
            //draw current piece
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (game.IsPieceLocationSet(i, j))
                    {
                        graphics.DrawPixel((game.CurrentPiece.X + i),
                            game.CurrentPiece.Y + j + yOffset);
                    }
                }
            }

            //draw gamefield
            for (int i = 0; i < game.Width; i++)
            {
                for (int j = 0; j < game.Height; j++)
                {
                    if (game.IsGameFieldSet(i, j))
                    {
                        graphics.DrawPixel(i, j + yOffset);
                    }
                }
            }
        }

        void Init()
        {
            Console.WriteLine("Init");

            display = new Max7219(Device, Device.CreateSpiBus(), Device.Pins.D01, 4, Max7219.Max7219Type.Display);

            graphics = new GraphicsLibrary(display);
            graphics.CurrentFont = new Font4x8();

            joystick = new AnalogJoystick(Device, Device.Pins.A01, Device.Pins.A02, null, true);
        }
    }
}