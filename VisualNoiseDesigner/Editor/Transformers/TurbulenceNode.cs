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
    [Node(false, "Transform/Turbulence")]
    public class TurbulenceNode : NodeBase
    {
        public int seed = 0;
        public float frequency = 1.0f;
        public float power = 1.0f;
        public int roughness = 3;

        protected override string ModuleName
        {
            get
            {
                return "Turbulence";
            }
        }

        protected override int ModulePropertiesCount
        {
            get
            {
                return 4;
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
            var turbulence = new Turbulence(power, GetInput(0));
            turbulence.Seed = seed;
            turbulence.Frequency = frequency;
            turbulence.Roughness = roughness;
            return turbulence;
        }

        protected override void DrawModuleProperties()
        {
            seed = EditorGUILayout.IntField("Seed", seed);
            frequency = EditorGUILayout.FloatField("Frequency", frequency);
            power = EditorGUILayout.FloatField("Power", power);
            roughness = EditorGUILayout.IntField("Roughness", roughness);
        }
    }
}