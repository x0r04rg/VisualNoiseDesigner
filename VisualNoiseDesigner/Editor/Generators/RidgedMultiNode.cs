using UnityEngine;
using UnityEditor;
using System;
using LibNoise;
using LibNoise.Generator;
using NodeEditorFramework;

namespace VisualNoiseDesigner
{
    [Serializable]
    [Node(false, "Generator/RidgedMultifractal")]
    public class RidgedMultiNode : NodeBase
    {
        public QualityMode quality = QualityMode.Medium;
        public int seed = 0;
        public float frequency = 1.0f;
        public int octaves = 6;
        public float lacunarity = 2.0f;

        protected override string ModuleName
        {
            get
            {
                return "RidgedMultifractal";
            }
        }

        protected override int ModulePropertiesCount
        {
            get
            {
                return 5;
            }
        }

        protected override void DrawModuleProperties()
        {
            quality = (QualityMode)EditorGUILayout.EnumPopup(new GUIContent("Quality"), quality);
            seed = EditorGUILayout.IntField("Seed", seed);
            frequency = EditorGUILayout.FloatField("Frequency", frequency);
            octaves = EditorGUILayout.IntSlider("Octaves", octaves, 1, 30);
            lacunarity = EditorGUILayout.FloatField("Lacunarity", lacunarity);
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            RidgedMultifractal ridgedMulti = new RidgedMultifractal();
            ridgedMulti.Quality = quality;
            ridgedMulti.Seed = seed;
            ridgedMulti.Frequency = frequency;
            ridgedMulti.OctaveCount = octaves;
            ridgedMulti.Lacunarity = lacunarity;
            return ridgedMulti;
        }
    }
}