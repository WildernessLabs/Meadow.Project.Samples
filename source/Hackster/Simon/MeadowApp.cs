using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using System;
using System.Threading;

namespace Simon
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        int ANIMATION_DELAY = 200;
        float[] notes = new float[] { 261.63f, 329.63f, 392, 523.25f };

        PwmLed[] leds;
        PushButton[] pushButtons; 

        PiezoSpeaker speaker;

        bool isAnimating = false;
        SimonGame game = new SimonGame();

        public MeadowApp()
        {
            Initialize();

            Console.WriteLine("Welcome to Simon");
            SetAllLEDs(true);            
            game.OnGameStateChanged += OnGameStateChanged;            
            game.Reset();            
        }

        void Initialize() 
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            leds = new PwmLed[4];
            leds[0] = new PwmLed(Device, Device.Pins.D10, TypicalForwardVoltage.Red);
            leds[1] = new PwmLed(Device, Device.Pins.D09, TypicalForwardVoltage.Green);
            leds[2] = new PwmLed(Device, Device.Pins.D08, TypicalForwardVoltage.Blue);
            leds[3] = new PwmLed(Device, Device.Pins.D07, TypicalForwardVoltage.Yellow);

            pushButtons = new PushButton[4];
            pushButtons[0] = new PushButton(Device, Device.Pins.MISO);
            pushButtons[0].Clicked += ButtonRedClicked;
            pushButtons[1] = new PushButton(Device, Device.Pins.D02);
            pushButtons[1].Clicked += ButtonGreenClicked;
            pushButtons[2] = new PushButton(Device, Device.Pins.D03);
            pushButtons[2].Clicked += ButtonBlueClicked;
            pushButtons[3] = new PushButton(Device, Device.Pins.D04);
            pushButtons[3].Clicked += ButtonYellowClicked;

            speaker = new PiezoSpeaker(Device.CreatePwmPort(Device.Pins.D11));

            led.SetColor(RgbLed.Colors.Green);
        }

        void ButtonRedClicked(object sender, EventArgs e)
        {
            OnButton(0);
        }

        void ButtonGreenClicked(object sender, EventArgs e)
        {
            OnButton(1);
        }

        void ButtonBlueClicked(object sender, EventArgs e)
        {
            OnButton(2);
        }

        void ButtonYellowClicked(object sender, EventArgs e)
        {
            OnButton(3);
        }

        void OnButton(int buttonIndex)
        {
            Console.WriteLine("Button tapped: " + buttonIndex);
            if (isAnimating == false)
            {                
                TurnOnLED(buttonIndex);
                game.EnterStep(buttonIndex);
            }
        }

        void OnGameStateChanged(object sender, SimonEventArgs e)
        {
            //var th = new Thread(() =>
            //{
                Console.WriteLine($"OnGameStateChanged - {e.GameState}");

                switch (e.GameState)
                {
                    case GameState.Start:
                        break;
                    case GameState.NextLevel:                        
                        ShowStartAnimation();                        
                        ShowNextLevelAnimation(game.Level);
                        Console.WriteLine("Game - 1");
                        ShowSequenceAnimation(game.Level);
                        Console.WriteLine("Game - 4");
                        break;
                    case GameState.GameOver:
                        ShowGameOverAnimation();
                        game.Reset();
                        break;
                    case GameState.Win:
                        ShowGameWonAnimation();
                        break;
                }
            //});
            //th.Start();
        }

        void TurnOnLED(int index, int duration = 400)
        {
            Console.WriteLine("TurnOnLED - 1");
            leds[index].IsOn = true;
            Console.WriteLine($"notes {notes[index]} duration {duration}");            
            speaker.PlayTone(notes[index], duration);            
            leds[index].IsOn = false;
            Console.WriteLine("TurnOnLED - 2");
        }

        void SetAllLEDs(bool isOn)
        {
            leds[0].IsOn = isOn;
            leds[1].IsOn = isOn;
            leds[2].IsOn = isOn;
            leds[3].IsOn = isOn;
        }

        void ShowStartAnimation()
        {
            if (isAnimating)
                return;
            isAnimating = true;
            SetAllLEDs(false);
            for (int i = 0; i < 4; i++)
            {
                leds[i].IsOn = true;
                Thread.Sleep(ANIMATION_DELAY);
            }
            for (int i = 0; i < 4; i++)
            {
                leds[3 - i].IsOn = false;
                Thread.Sleep(ANIMATION_DELAY);
            }
            isAnimating = false;
        }

        void ShowNextLevelAnimation(int level)
        {
            if (isAnimating)
                return;
            isAnimating = true;
            SetAllLEDs(false);
            for (int i = 0; i < level; i++)
            {
                Thread.Sleep(ANIMATION_DELAY);
                SetAllLEDs(true);
                Thread.Sleep(ANIMATION_DELAY * 3);
                SetAllLEDs(false);
            }
            isAnimating = false;
        }

        void ShowSequenceAnimation(int level)
        {
            if (isAnimating)
                return;
            isAnimating = true;
            
            var steps = game.GetStepsForLevel();
            SetAllLEDs(false);            
            for (int i = 0; i < level; i++)
            {
                Thread.Sleep(200);
                Console.WriteLine("ShowSequenceAnimation - 4");
                TurnOnLED(steps[i], 400);
            }

            Console.WriteLine("ShowSequenceAnimation - 5");
            isAnimating = false;
        }

        void ShowGameOverAnimation()
        {
            if (isAnimating)
                return;
            isAnimating = true;

            Thread.Sleep(750);
            //speaker.PlayTone(123.47f, 750);

            for (int i = 0; i < 20; i++)
            {
                SetAllLEDs(false);
                Thread.Sleep(50);
                SetAllLEDs(true);
                Thread.Sleep(50);
            }
            isAnimating = false;
        }

        void ShowGameWonAnimation()
        {
            ShowStartAnimation();
            ShowStartAnimation();
            ShowStartAnimation();
            ShowStartAnimation();
        }
    }
}