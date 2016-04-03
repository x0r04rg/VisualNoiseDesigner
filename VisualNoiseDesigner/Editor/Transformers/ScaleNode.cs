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
    [Node(false, "Transform/Scale")]
    public class ScaleNode : NodeBase
    {
        public Vector3 scale = Vector3.one;

        protected override string ModuleName
        {
            get
            {
                return "Scale";
            }
        }

        protected override int ModulePropertiesCount
        {
            get
            {
                return 2;
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
            return new Scale(scale.x, scale.y, scale.z, GetInput(0));
        }

        protected override void DrawModuleProperties()
        {
            scale = EditorGUILayout.Vector3Field("Scale", scale);
        }
    }
}