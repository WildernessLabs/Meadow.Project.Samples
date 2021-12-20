using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Peripherals.Sensors.Hid;

namespace Tetris
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        MicroGraphics graphics;
        AnalogJoystick joystick;
        TetrisGame game = new TetrisGame(8, 24);

        public MeadowApp()
        {
            Console.WriteLine("Tetris");

            Initialize();

            Console.WriteLine("Start game");
            StartGameLoop();
        }

        void Initialize()
        {
            var display = new Max7219(
                device: Device, 
                spiBus: Device.CreateSpiBus(Max7219.DefaultSpiBusSpeed), 
                csPin: Device.Pins.D01, 
                deviceCount: 4, 
                maxMode: Max7219.Max7219Type.Display);

            graphics = new MicroGraphics(display);
            graphics.CurrentFont = new Font4x8();

            joystick = new AnalogJoystick(Device, Device.Pins.A01, Device.Pins.A02, null, true);
            joystick.StartUpdating(TimeSpan.FromMilliseconds(20));
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

        void CheckInput(int tick)
        {
            if (tick % (21 - game.Level) == 0)
            {
                game.OnDown(true);
            }

            var pos = joystick.DigitalPosition;

            if (pos == DigitalJoystickPosition.Left)
            {
                game.OnLeft();
            }
            if (pos == DigitalJoystickPosition.Right)
            {
                game.OnRight();
            }
            if (pos == DigitalJoystickPosition.Up)
            {
                game.OnRotate();
            }
            if (pos == DigitalJoystickPosition.Down)
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
    }
}