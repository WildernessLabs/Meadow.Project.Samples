using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;

namespace Span4
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Ssd1309 display;
        GraphicsLibrary graphics;

        IDigitalInputPort portLeft;
        IDigitalInputPort portRight;
        IDigitalInputPort portDown;
        IDigitalInputPort portReset;

        PiezoSpeaker speaker;

        Span4Game connectGame;

        byte currentColumn = 0;

        public MeadowApp()
        {
            Console.WriteLine("Span 4");

            connectGame = new Span4Game();

            Initialize();

            graphics.Clear();
            graphics.DrawText(0, 0, "Meadow Span4!");
            graphics.DrawText(0, 10, "v0.0.4");
            graphics.Show();

            Thread.Sleep(250);

            StartGameLoop();
        }

        void StartGameLoop()
        {
            while (true)
            {
                CheckInput();

                graphics.Clear(false);
                DrawGame();
                graphics.Show();

                Thread.Sleep(150);
            }
        }

        void CheckInput()
        {
            if (portLeft.State == true)
            {
                if(currentColumn > 0)
                {
                    currentColumn -= 1;
                }
            }
            else if (portRight.State == true)
            {
                if (currentColumn < connectGame.Width - 1)
                {
                    currentColumn += 1;
                }
            }
            else if (portDown.State == true)
            {
                connectGame.AddChip(currentColumn);
                speaker.PlayTone(440, 200);

            }
            else if(portReset.State == true)
            {
                connectGame.Reset();
            }
        }

        int CellSize = 9;
        int yStart = 9;
        int xStart = 0;

        void DrawGame()
        {
            //draw gameboard
            graphics.DrawRectangle(0, 9, 64, 55, true, false);

            for(int i = 1; i < 7; i++)
            {
                graphics.DrawLine(CellSize * i,
                    yStart,
                    CellSize * i,
                    yStart + CellSize * 6 + 1,
                    true);
            }

            for (int j = 1; j < 6; j++)
            {
                graphics.DrawLine(xStart,
                    yStart + j*CellSize,
                    63 + xStart,
                    yStart + j * CellSize,
                    true);
            }

            for (int x = 0; x < connectGame.Width; x++)
            {
                for (int y = 0; y < connectGame.Height; y++)
                {
                    if(connectGame.GameField[x,y] == 0) { continue; }
                    DrawChipOnBoard(x, y, connectGame.GameField[x, y] == 1);
                }
            }

            //Game state
            switch(connectGame.GameState)
            {
                case Span4Game.GameStateType.Draw:
                    graphics.DrawText(2, 0, "Draw");
                    break;
                case Span4Game.GameStateType.Player1Wins:
                    graphics.DrawText(2, 0, "Player 1 Wins!");
                    break;
                case Span4Game.GameStateType.Player2Wins:
                    graphics.DrawText(2, 0, "Player 2 Wins!");
                    break;
                case Span4Game.GameStateType.Player1Turn:
                    DrawPreviewChip(currentColumn, true);
                    break;
                case Span4Game.GameStateType.Player2Turn:
                    DrawPreviewChip(currentColumn, false);
                    break;
            }

            //Draw side display
            int xText = 75; 
            graphics.DrawText(xText, 0, "Span4!");

            graphics.DrawText(xText, 18, "Player 1");
            DrawChip(115, 21, true);

            graphics.DrawText(xText, 27, "Player 2");
            DrawChip(115, 30, false);

            graphics.DrawText(xText, 45, "Score:");
            graphics.DrawText(xText, 54, $"{connectGame.Player1Wins} to {connectGame.Player2Wins}");
        }

        void DrawPreviewChip(int column, bool isFilled)
        {
            DrawChip(xStart + column * CellSize + 5,
                5,
                isFilled);
        }

        void DrawChipOnBoard(int column, int row, bool isFilled)
        {
            DrawChip(xStart + column * CellSize + 5,
                yStart + (connectGame.Height - row - 1) * CellSize + 5,
                isFilled);
        }
        void DrawChip(int xCenter, int yCenter, bool isFilled)
        {
            graphics.DrawCircle(xCenter, yCenter, 3,
                            true, isFilled, true);
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            portLeft = Device.CreateDigitalInputPort(Device.Pins.D13);
            portRight = Device.CreateDigitalInputPort(Device.Pins.D11);
            portDown = Device.CreateDigitalInputPort(Device.Pins.D12);
            portReset = Device.CreateDigitalInputPort(Device.Pins.D07);

            speaker = new PiezoSpeaker(Device.CreatePwmPort(Device.Pins.D05));

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
        }
    }
}