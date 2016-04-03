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
    [Node(false, "Modifier/Invert")]
    public class InvertNode : NodeBase
    {
        protected override string ModuleName
        {
            get
            {
                return "Invert";
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
            return new Invert(Inputs[0].GetValue<ModuleBase>());
        }
    }
}