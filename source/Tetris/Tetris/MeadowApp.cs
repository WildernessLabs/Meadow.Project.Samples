using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;

namespace Tetris
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Ssd1309 display;
        GraphicsLibrary graphics;

        IDigitalInputPort portLeft;
        IDigitalInputPort portUp;
        IDigitalInputPort portRight;
        IDigitalInputPort portDown;

        TetrisGame game = new TetrisGame(8, 18);

        int BLOCK_SIZE = 6;

        public MeadowApp()
        {
            Console.WriteLine("Tetris");

            Init();

            graphics.Clear();
            graphics.DrawText(0, 0, "Meadow Tetris");
            graphics.DrawText(0, 10, "v0.0.2");
            graphics.Show();

            Thread.Sleep(1000);

            StartGameLoop();
        }

        int tick = 0;
        void StartGameLoop()
        {
            while(true)
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

            if (portLeft.State == true) {
                game.OnLeft();
            }
            else if (portRight.State == true) {
                game.OnRight();
            }
            else if (portUp.State == true) {
                game.OnRotate();
            }
            else if (portDown.State == true) {
                game.OnDown();
            }
        }

        void DrawTetrisField()
        {
            int xIndent = 8;
            int yIndent = 12;

            graphics.DrawText(xIndent, 0, $"Lines: {game.LinesCleared}");

            graphics.DrawRectangle(6, 10, 52, 112); 

            //draw current piece
            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(game.IsPieceLocationSet(i, j))
                    {
                        //  graphics.DrawPixel(i, j);
                        graphics.DrawRectangle((game.CurrentPiece.X + i) * BLOCK_SIZE + xIndent,
                            (game.CurrentPiece.Y + j) * BLOCK_SIZE + yIndent,
                            BLOCK_SIZE + 1, BLOCK_SIZE, true, true);//+1 hack until we fix the graphics lib
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
                        graphics.DrawRectangle((i) * BLOCK_SIZE + xIndent,
                            (j) * BLOCK_SIZE + yIndent,
                            BLOCK_SIZE + 1, BLOCK_SIZE, true, true);//+1 hack until we fix the graphics lib
                    }
                }
            } 
        }

        void Init()
        {
            Console.WriteLine("Init");

            portLeft = Device.CreateDigitalInputPort(Device.Pins.D12);
            portUp = Device.CreateDigitalInputPort(Device.Pins.D13);
            portRight = Device.CreateDigitalInputPort(Device.Pins.D07);
            portDown = Device.CreateDigitalInputPort(Device.Pins.D11);

            var config = new SpiClockConfiguration(12000, SpiClockConfiguration.Mode.Mode0);

            var bus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

            display = new Ssd1309
            (
                device: Device,
                spiBus: bus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00
            );

            graphics = new GraphicsLibrary(display);
            graphics.CurrentFont = new Font4x8();
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;
        }
    }
}