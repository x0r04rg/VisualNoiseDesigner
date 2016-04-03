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
    [Node(false, "Selector/Select")]
    public class SelectNode : NodeBase
    {
        public float edgeFalloff = 0;
        public float lowerBound = -1;
        public float upperBound = 1;

        protected override string ModuleName
        {
            get
            {
                return "Select";
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
                return new string[] { "Input 1", "Input 2", "Control" };
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            Select select = new Select(GetInput(0), GetInput(1), GetInput(2));
            select.FallOff = edgeFalloff;
            select.Minimum = lowerBound;
            select.Maximum = upperBound;
            return select;
        }

        protected override void DrawModuleProperties()
        {
            edgeFalloff = EditorGUILayout.FloatField("Edge Falloff", edgeFalloff);
            var lw = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;
            EditorGUILayout.MinMaxSlider(new GUIContent("Bounds"), ref lowerBound, ref upperBound, -1, 1);
            EditorGUIUtility.labelWidth = lw;
        }
    }
}