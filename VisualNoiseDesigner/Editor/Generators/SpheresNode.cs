using UnityEngine;
using UnityEditor;
using System;
using LibNoise;
using LibNoise.Generator;
using NodeEditorFramework;

namespace VisualNoiseDesigner
{
    [Serializable]
    [Node(false, "Generator/Spheres")]
    public class SpheresNode : NodeBase
    {
        public float frequency = 1.0f;

        protected override string ModuleName
        {
            get
            {
                return "Spheres";
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
            Spheres spheres = new Spheres();
            spheres.Frequency = frequency;
            return spheres;
        }

        protected override void DrawModuleProperties()
        {
            frequency = EditorGUILayout.FloatField("Frequency", frequency);
        }
    }
}