using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;

namespace Frogger
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Ssd1309 display;
        GraphicsLibrary graphics;

        PushButton buttonLeft;
        PushButton buttonRight;
        PushButton buttonUp;
        PushButton buttonDown;

        RgbPwmLed onboardLed;

        FroggerGame frogger;

        //UI 
        int cellSize = 8;

        public MeadowApp()
        {
            frogger = new FroggerGame();

            Initialize();

            DrawTitleMenu();

            StartGame();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

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

            buttonLeft = new PushButton(Device, Device.Pins.D13, ResistorMode.PullDown);
            buttonRight = new PushButton(Device, Device.Pins.D12, ResistorMode.PullDown);
            buttonUp = new PushButton(Device, Device.Pins.D11, ResistorMode.PullDown);
            buttonDown = new PushButton(Device, Device.Pins.D08, ResistorMode.PullDown);

            Console.WriteLine("Initialize hardware complete.");
        }

        void StartGame()
        {
            InitializeGame();
            StartGameLoop();
        }

        void InitializeGame()
        {

        }

        void StartGameLoop()
        {
            while (true)
            {
                frogger.Update();
              //  CheckInput();
                graphics.Clear();
                DrawBackground();
                DrawLanes();
                DrawFrog();
                DrawLives();
                graphics.Show();

              //  Thread.Sleep(10);
            }
        }

        void CheckInput()
        {
            if(buttonUp.State == true)
            {
                frogger.OnUp();
            }
            else if (buttonLeft.State == true)
            {
                frogger.OnLeft();
            }
            else if (buttonRight.State == true)
            {
                frogger.OnRight();
            }
            else if (buttonDown.State == true)
            {
                frogger.OnDown();
            }
        }

        void DrawBackground()
        {
            //draw docks
            for(int i = 0; i < 5; i++)
            {
                graphics.DrawRectangle(10 + 24 * i, 0, 12, 9, true, false);

                DrawFrog(12 + 24 * i, 2, 1);
            }

            //draw water
            graphics.DrawRectangle(0, cellSize, 128, cellSize * 3, true, true);
        }

        void DrawLanes()
        {
            int startPos, index, x = 0, y;
            int cellOffset;
            for (byte row = 0; row < 6; row++)
            {
                startPos = (int)(frogger.GameTime * frogger.LaneSpeeds[row]) % frogger.LaneLength;
                cellOffset = (int)(8.0f * frogger.GameTime * frogger.LaneSpeeds[row]) % cellSize;

                if (startPos < 0)
                {
                    startPos = frogger.LaneLength - (Math.Abs(startPos) % 64);
                }

                for (byte i = 0; i < frogger.Columns + 2; i++)
                {
                    index = frogger.LaneData[row, (startPos + i) % frogger.LaneLength];

                    if(index == 0) { continue; }

                    y = cellSize * (row + 1);
                    x = (i - 1) * cellSize - cellOffset;

                    switch(row)
                    {
                        case 0:
                        case 1:
                        case 2:
                            DrawLog(x, y, index);
                            break;
                        case 3:
                        case 5:
                            DrawTruck(x, y, index);
                            break;
                        case 4:
                            DrawCar(x, y, index);
                            break;
                    }
                }
            }
        }

        void DrawLives()
        {
            for(int i = 1; i < frogger.Lives; i++)
            {
                DrawFrog(cellSize * (frogger.Columns - i), cellSize * frogger.Rows - 1, 1);
            }
        }

        void DrawFrog()
        {
            DrawFrog(frogger.FrogX * cellSize, frogger.FrogY * cellSize, 1);
        }

        void DrawFrog(int x, int y, int frame)
        {
            if(frame == 0)
            {
                DrawBitmap(x, y, 1, 8, frogger.frogLeft);
            }
            else if(frame == 1)
            {
                DrawBitmap(x, y, 1, 8, frogger.frogUp);
            }
            else
            {
                DrawBitmap(x, y, 1, 8, frogger.frogRight);
            }
        }

        void DrawTruck(int x, int y, int index)
        {
            if(index == 1) DrawBitmap(x, y, 1, 8, frogger.truckLeft);
            else if (index == 2) DrawBitmap(x, y, 1, 8, frogger.truckCenter);
            else if (index == 3) DrawBitmap(x, y, 1, 8, frogger.truckRight);
        }

        void DrawLog(int x, int y, int index)
        {
            if (index == 1) DrawBitmap(x, y, 1, 8, frogger.logLeft);
            else if (index == 2) DrawBitmap(x, y, 1, 8, frogger.logCenter);
            else if (index == 3) DrawBitmap(x, y, 1, 8, frogger.logRight);
        }

        void DrawCar(int x, int y, int index)
        {
            if (index == 1) DrawBitmap(x, y, 1, 8, frogger.carLeft);
            else if (index == 2) DrawBitmap(x, y, 1, 8, frogger.carRight);
        }

        void DrawBitmap(int x, int y, int width, int height, byte[] bitmap)
        {
            for (var ordinate = 0; ordinate < height; ordinate++) //y
            {
                for (var abscissa = 0; abscissa < width; abscissa++) //x
                {
                    var b = bitmap[(ordinate * width) + abscissa];
                    byte mask = 0x01;

                    for (var pixel = 0; pixel < 8; pixel++)
                    {
                        if ((b & mask) > 0)
                        {
                           graphics.DrawPixel(x + (8 * abscissa) + 7 - pixel, y + ordinate);
                        }
                        else
                        {
                            graphics.DrawPixel(x + (8 * abscissa) + 7 - pixel, y + ordinate, false);
                        }
                        mask <<= 1;
                    }
                }
            }
        }

        void DrawTitleMenu()
        {
            Console.WriteLine("start title");

            int count = 0;
            {
                graphics.Clear();

                DrawBitmap(0, 0, 5, 40, frogger.titleBmp);

                DrawFrog(50, 50, count % 3);

                DrawBitmap(80, 10, 1, 8, frogger.carLeft);
                DrawBitmap(88, 10, 1, 8, frogger.carRight);

                DrawBitmap(80, 30, 1, 8, frogger.truckLeft);
                DrawBitmap(88, 30, 1, 8, frogger.truckCenter);
                DrawBitmap(96, 30, 1, 8, frogger.truckRight);

                DrawBitmap(80, 50, 1, 8, frogger.logLeft);
                DrawBitmap(88, 50, 1, 8, frogger.logCenter);
                DrawBitmap(96, 50, 1, 8, frogger.logRight);

                graphics.Show();

                Thread.Sleep(250);
                count++;
            }
        }
    }
}