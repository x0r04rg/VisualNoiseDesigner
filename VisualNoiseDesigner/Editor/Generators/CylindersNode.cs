using UnityEngine;
using UnityEditor;
using System;
using LibNoise;
using LibNoise.Generator;
using NodeEditorFramework;

namespace VisualNoiseDesigner
{
    [Serializable]
    [Node(false, "Generator/Cylinders")]
    public class CylindersNode : NodeBase
    {
        public float frequency = 1.0f;

        protected override string ModuleName
        {
            get
            {
                return "Cylinders";
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
            Cylinders cylinders = new Cylinders();
            cylinders.Frequency = frequency;
            return cylinders;
        }

        protected override void DrawModuleProperties()
        {
            frequency = EditorGUILayout.FloatField("Frequency", frequency);
        }
    }
}