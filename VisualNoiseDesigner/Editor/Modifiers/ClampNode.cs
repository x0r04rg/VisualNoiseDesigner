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
    [Node(false, "Modifier/Clamp")]
    public class ClampNode : NodeBase
    {
        public float lowerBound = -1;
        public float upperBound = 1;

        protected override string ModuleName
        {
            get
            {
                return "Clamp";
            }
        }

        protected override string[] ModuleInputs
        {
            get
            {
                return new string[] { "Input" };
            }
        }

        protected override int ModulePropertiesCount
        {
            get
            {
                return 1;
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            return new Clamp(Inputs[0].GetValue<ModuleBase>());
        }

        protected override void DrawModuleProperties()
        {
            var lw = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;
            EditorGUILayout.MinMaxSlider(new GUIContent("Bounds"), ref lowerBound, ref upperBound, -1, 1);
            EditorGUIUtility.labelWidth = lw;
        }
    }
}