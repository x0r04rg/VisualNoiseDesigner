using UnityEngine;
using UnityEditor;
using System;
using LibNoise;
using LibNoise.Generator;
using NodeEditorFramework;

namespace VisualNoiseDesigner
{
    [Serializable]
    [Node(false, "Generator/Billow")]
    public class BillowNode : NodeBase
    {
        public QualityMode quality = QualityMode.Medium;
        public int seed = 0;
        public float frequency = 1.0f;
        public int octaves = 6;
        public float lacunarity = 2.0f;
        public float persistance = 0.5f;

        protected override int ModulePropertiesCount
        {
            get
            {
                return 6;
            }
        }

        protected override string ModuleName
        {
            get
            {
                return "Billow";
            }
        }

        protected override void DrawModuleProperties()
        {
            quality = (QualityMode)EditorGUILayout.EnumPopup(new GUIContent("Quality"), quality);
            seed = EditorGUILayout.IntField("Seed", seed);
            frequency = EditorGUILayout.FloatField("Frequency", frequency);
            octaves = EditorGUILayout.IntSlider("Octaves", octaves, 1, 30);
            lacunarity = EditorGUILayout.FloatField("Lacunarity", lacunarity);
            persistance = EditorGUILayout.FloatField("Persistance", persistance);
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            Billow billow = new Billow();
            billow.Quality = quality;
            billow.Seed = seed;
            billow.Frequency = frequency;
            billow.OctaveCount = octaves;
            billow.Lacunarity = lacunarity;
            billow.Persistence = persistance;
            return billow;
        }
    }
}