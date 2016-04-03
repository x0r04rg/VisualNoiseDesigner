using UnityEngine;
using UnityEditor;
using System;
using LibNoise;
using LibNoise.Generator;
using NodeEditorFramework;

namespace VisualNoiseDesigner
{
    [Serializable]
    [Node(false, "Generator/Const")]
    public class ConstNode : NodeBase
    {
        public float value = 0.0f;

        protected override string ModuleName
        {
            get
            {
                return "Const Noise";
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
            Const c = new Const();
            c.Value = value;
            return c;
        }

        protected override void DrawModuleProperties()
        {
            value = EditorGUILayout.FloatField("Value", value);
        }
    }
}