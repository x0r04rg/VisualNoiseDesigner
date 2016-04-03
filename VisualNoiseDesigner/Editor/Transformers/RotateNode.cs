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
    [Node(false, "Transform/Rotate")]
    public class RotateNode : NodeBase
    {
        public Vector3 rotation = Vector3.zero;

        protected override string ModuleName
        {
            get
            {
                return "Rotate";
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
            return new Rotate(rotation.x, rotation.y, rotation.z, GetInput(0));
        }

        protected override void DrawModuleProperties()
        {
            rotation = EditorGUILayout.Vector3Field("Rotation", rotation);
        }
    }
}