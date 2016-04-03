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
    [Node(false, "Transform/Translate")]
    public class TranslateNode : NodeBase
    {
        public Vector3 translation = Vector3.zero;

        protected override string ModuleName
        {
            get
            {
                return "Translate";
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
            return new Scale(translation.x, translation.y, translation.z, GetInput(0));
        }

        protected override void DrawModuleProperties()
        {
            translation = EditorGUILayout.Vector3Field("Translation", translation);
        }
    }
}