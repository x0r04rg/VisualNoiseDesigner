using UnityEngine;
using UnityEditor;
using System;
using LibNoise;
using LibNoise.Generator;
using NodeEditorFramework;

namespace VisualNoiseDesigner
{
    [Serializable]
    [Node(false, "Generator/Voronoi")]
    public class VoronoiNode : NodeBase
    {
        public int seed = 1;
        public float frequency = 1.0f;
        public float displacement = 1.0f;
        public bool enableDistance = false;

        protected override string ModuleName
        {
            get
            {
                return "Voronoi";
            }
        }

        protected override int ModulePropertiesCount
        {
            get
            {
                return 4;
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            Voronoi voronoi = new Voronoi();
            voronoi.Seed = seed;
            voronoi.Frequency = frequency;
            voronoi.Displacement = displacement;
            voronoi.UseDistance = enableDistance;
            return voronoi;
        }

        protected override void DrawModuleProperties()
        {
            seed = EditorGUILayout.IntField("Seed", seed);
            frequency = EditorGUILayout.FloatField("Frequency", frequency);
            displacement = EditorGUILayout.FloatField("Displacement", displacement);
            enableDistance = EditorGUILayout.Toggle("Enable Distance", enableDistance);
        }
    }
}