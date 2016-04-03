using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using LibNoise;

namespace VisualNoiseDesigner
{
    public class UnityCurve : ModuleBase
    {
        AnimationCurve curve;

        public AnimationCurve Curve
        {
            get { return curve; }
            set { curve = value; }
        }

        public UnityCurve() : base(1)
        {
            curve = new AnimationCurve();
        }

        public UnityCurve(ModuleBase input, AnimationCurve curve) : base(1)
        {
            Modules[0] = input;
            this.curve = curve;
        }

        public override double GetValue(double x, double y, double z)
        {
            Debug.Assert(Modules[0] != null);

            var value = Modules[0].GetValue(x, y, z);
            var normalizedValue = (value + 1) * 0.5;

            var res = curve.Evaluate((float)normalizedValue);

            return res * 2 - 1;
        }

        
    }
}