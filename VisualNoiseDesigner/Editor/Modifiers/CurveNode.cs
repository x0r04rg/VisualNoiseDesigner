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
    [Node(false, "Modifier/Curve")]
    public class CurveNode : NodeBase
    {
        public AnimationCurve curve = new AnimationCurve();

        protected override string ModuleName
        {
            get
            {
                return "Curve";
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
                return new string[] { "Input" };
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            return new UnityCurve(Inputs[0].GetValue<ModuleBase>(), curve);
        }

        protected override void DrawModuleProperties()
        {
            curve = EditorGUILayout.CurveField("Curve", curve);
        }
    }
}