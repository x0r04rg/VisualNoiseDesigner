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
    [Node(false, "Modifier/Abs")]
    public class AbsNode : NodeBase
    {
        protected override string ModuleName
        {
            get
            {
                return "Abs";
            }
        }

        protected override string[] ModuleInputs
        {
            get
            {
                return new string[] { "Input" };
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            return new Abs(Inputs[0].GetValue<ModuleBase>());
        }
    }
}