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
    [Node(false, "Selector/Blend")]
    public class BlendNode : NodeBase
    {
        protected override string ModuleName
        {
            get
            {
                return "Blend";
            }
        }

        protected override string[] ModuleInputs
        {
            get
            {
                return new string[] { "Input 1", "Input 2", "Control" };
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            return new Blend(GetInput(0), GetInput(1), GetInput(2));
        }
    }
}