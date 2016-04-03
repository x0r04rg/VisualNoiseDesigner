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
    [Node(false, "Combiner/Min")]
    public class MinNode : NodeBase
    {
        protected override string ModuleName
        {
            get
            {
                return "Min";
            }
        }

        protected override string[] ModuleInputs
        {
            get
            {
                return new string[] { "Input 1", "Input 2" };
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            return new Min(Inputs[0].GetValue<ModuleBase>(), Inputs[1].GetValue<ModuleBase>());
        }
    }
}