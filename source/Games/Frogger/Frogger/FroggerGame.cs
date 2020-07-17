using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Frogger
{
    public partial class FroggerGame
    {
        //each lane has a velocity (float)
        //each lane shows one obstical type
        //safe or not safe

        public float[] LaneSpeeds { get; private set; } = new float[6] { 1.0f, -2.0f, 1.5f, -1.0f, 1.5f, -2.0f };
        public byte[,] LaneData { get; private set; } = new byte[6, 32]
        {
            //docks
            {1,2,3,0,1,2,3,0,0,0,1,2,3,0,1,2,3,0,0,0,1,2,3,0,0,0,0,1,2,3,0,0 },//logs
            {0,0,1,3,0,0,0,1,3,0,0,0,1,3,0,0,1,3,0,0,0,1,3,0,1,3,0,0,1,3,0,0 },//logs
            {1,2,3,0,1,2,3,0,0,0,1,2,3,0,1,2,3,0,0,0,1,2,3,0,0,0,0,1,2,3,0,0 },//logs
            {0,0,1,3,0,1,3,0,0,0,0,0,0,0,0,0,1,3,0,0,0,0,0,0,1,3,0,0,1,3,0,0 },//trucks
            {0,0,1,2,0,0,0,1,2,0,0,0,1,2,0,0,1,2,0,0,0,1,2,0,1,2,0,0,1,2,0,0 },//cars
            {1,2,3,0,0,0,0,0,0,0,0,1,2,3,0,0,0,0,0,1,2,3,0,0,0,0,0,1,2,3,0,0 },//trucks
            //start
        };

        public double GameTime { get; private set; }

        public byte LaneLength => 32;
        public byte Columns => 16;
        public byte Rows => 8;

        public byte FrogX { get; private set; }
        public byte FrogY { get; private set; }

        public byte Lives { get; private set; }

        public FroggerGame()
        {
            Reset();
        }

        void Reset()
        { 
            gameStart = DateTime.Now;
            FrogX = (byte)(Columns / 2);
            FrogY = (byte)(Rows - 1);
            Lives = 3;
        }

        DateTime gameStart;
        public void Update()
        {
            GameTime = (DateTime.Now - gameStart).TotalSeconds;
        }

        public void OnUp()
        {
            if(FrogY > 0) { FrogY--; }
        }

        public void OnDown()
        {
            if (FrogY < Rows - 1) { FrogY++; }
        }

        public void OnLeft()
        {
            if (FrogX > 0) { FrogX--; }
        }

        public void OnRight()
        {
            if (FrogY < Columns - 1) { FrogX++; }
        }
    }
}