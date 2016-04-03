using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using LibNoise;
using LibNoise.Operator;
using NodeEditorFramework;

namespace VisualNoiseDesigner
{
    [Serializable]
    [Node(false, "Combiner/Power")]
    public class PowerNode : NodeBase
    {
        protected override string ModuleName
        {
            get
            {
                return "Power";
            }
        }

        protected override string[] ModuleInputs
        {
            get
            {
                return new string[] { "Value", "Power" };
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            return new Power(Inputs[0].GetValue<ModuleBase>(), Inputs[1].GetValue<ModuleBase>());
        }
    }
}