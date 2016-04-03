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
    [Node(false, "Modifier/Exponent")]
    public class ExponentNode : NodeBase
    {
        double exponent = 1.0f;

        protected override string ModuleName
        {
            get
            {
                return "Exponent";
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
                return 1;
            }
        }

        protected override ModuleBase CreateAndReadyModule()
        {
            return new Exponent(exponent, Inputs[0].GetValue<ModuleBase>());
        }

        protected override void DrawModuleProperties()
        {
            exponent = EditorGUILayout.DoubleField("Exponent", exponent);
        }
    }
}