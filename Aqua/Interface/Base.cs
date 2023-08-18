using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System;
using PrismAPI.Audio;
using PrismAPI.Graphics;
using PrismAPI.Hardware.GPU;
using PrismAPI.UI;
using PrismAPI.UI.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Aqua.Interface
{
    public class Base
    {
        public static Display canvas;
        public static ushort width = 800,
            height = 600;

        static Color mouseColor = Color.Black;

        static Window window;

        public static void Initialize()
        {
            AudioPlayer.Init();

            canvas = Display.GetDisplay(width, height);

            MouseManager.ScreenWidth = width;
            MouseManager.ScreenHeight = height;

            window = new Window(canvas, "Testing... windows. - Window 1", 100, 100, 500, 300);

            Kernel.guiStarted = true;
        }

        public static void Update()
        {
            canvas.Clear(Color.DeepBlue);

            // LOW LAYER
            canvas.DrawString(
                0,
                0,
                $"{canvas.GetFPS()} FPS - GUI 0.1",
                default,
                Color.White
            );

            // MID LAYER
            window.Update();

            // HIGH LAYER
            canvas.DrawFilledRectangle(
                (int)MouseManager.X,
                (int)MouseManager.Y,
                16,
                16,
                0,
                mouseColor
            );

            if (MouseManager.MouseState == MouseState.Left)
                mouseColor = Color.Black;
            else
                mouseColor = Color.White;

            canvas.Update();
            Heap.Collect();
        }
    }
}
