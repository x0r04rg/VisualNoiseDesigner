using System;
using System.Threading;
using LibNoise;

namespace VisualNoiseDesigner
{
    public class NoiseCalculation
    {
        ModuleBase module;
        int width;
        int height;

        public bool Done
        {
            get; set;
        }

        public Noise2D Noise
        {
            get; set;
        }

        public NoiseCalculation(ModuleBase module, int width, int height)
        {
            Done = false;

            this.module = module;
            this.width = width;
            this.height = height;

            ThreadPool.QueueUserWorkItem(Calculate);
        }

        void Calculate(object state)
        {
            Noise = new Noise2D(width, height, module);
            Noise.GeneratePlanar(4.0f, 10.0f, 1.0f, 5.0f);

            Done = true;
        }
    }
}