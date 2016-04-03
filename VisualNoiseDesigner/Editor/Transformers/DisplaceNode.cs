using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using LibNoise;
using LibNoise.Operator;
using NodeEditorFramework;

namespace VisualNoiseDesigner
{
    [Serializable]
    [Node(false, "Transform/Displace")]
    public class DisplaceNode : NodeBase
    {
        protected override string ModuleName
        {
            get
            {
                return "Displace";
            }
        }

        protected override string[] ModuleInputs
        {
            get
            {
                return new string[] { "Source", "X", "Y", "Z" };
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            return new Displace(GetInput(0), GetInput(1), GetInput(2), GetInput(3));
        }
    }
}