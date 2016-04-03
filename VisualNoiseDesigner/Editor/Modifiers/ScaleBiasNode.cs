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
    [Node(false, "Modifier/ScaleBias")]
    public class ScaleBiasNode : NodeBase
    {
        float bias = 0.0f;
        float scale = 1.0f;

        protected override string ModuleName
        {
            get
            {
                return "ScaleBias";
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
                return 2;
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            return new ScaleBias(scale, bias, Inputs[0].GetValue<ModuleBase>());
        }

        protected override void DrawModuleProperties()
        {
            scale = EditorGUILayout.FloatField("Scale", scale);
            bias = EditorGUILayout.FloatField("Bias", bias);
        }
    }
}