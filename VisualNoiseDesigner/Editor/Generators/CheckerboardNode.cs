using UnityEngine;
using UnityEditor;
using System;
using LibNoise;
using LibNoise.Generator;
using NodeEditorFramework;

namespace VisualNoiseDesigner
{
    [Serializable]
    [Node(false, "Generator/Checkerboard")]
    public class CheckerboardNode : NodeBase
    {
        protected override string ModuleName
        {
            get
            {
                return "Checkerboard";
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            return new Checker();
        }
    }
}