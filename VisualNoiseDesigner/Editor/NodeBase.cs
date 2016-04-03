using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Threading;
using LibNoise;
using NodeEditorFramework;

namespace VisualNoiseDesigner
{
    [Serializable]
    public abstract class NodeBase : Node
    {
        ModuleBase module;
        Texture2D preview;
        NoiseCalculation previewCalculation;
        public bool NeedsUpdate
        {
            get; set;
        }

        protected abstract string ModuleName
        {
            get;
        }

        protected virtual int ModulePropertiesCount
        {
            get { return 0; }
        }

        protected virtual string[] ModuleInputs
        {
            get { return new string[0]; }
        }

        public override sealed string GetID
        {
            get
            {
                return string.Format("{0}{1}", "visualnoisedesigner-", ModuleName.Replace(" ", "").ToLower());
            }
        }

        public ModuleBase Module
        {
            get { return module; }
        }

        protected ModuleBase GetInput(int index)
        {
            return Inputs[index].GetValue<ModuleBase>();
        }

        public sealed override Node Create(Vector2 pos)
        {
            Node node = CreateInstance(GetType()) as Node;

            node.name = ModuleName;
            node.rect = new Rect(pos.x, pos.y, 200, 145 + (ModulePropertiesCount + (ModuleInputs.Length == 1 ? 0 : ModuleInputs.Length)) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) + 20);
            
            node.CreateOutput("Output", typeof(ModuleBase).AssemblyQualifiedName);

            foreach (var input in ModuleInputs)
                node.CreateInput(input, typeof(ModuleBase).AssemblyQualifiedName);

            NeedsUpdate = false;

            return node;
        }

        protected sealed override void NodeGUI()
        {
            GUILayout.Space(138);
            if (preview == null) preview = new Texture2D(128, 128);
            EditorGUI.DrawPreviewTexture(new Rect((rect.width - 128) / 2, 5, 128, 128), preview);

            int i = 0;
            if (Inputs.Count == 1)
            {
                Inputs[0].SetPosition(rect.height / 2);
            }
            else
            {
                foreach (var input in Inputs)
                {
                    GUILayout.Label(input.name);
                    InputKnob(i++);
                }
            }

            DrawModuleProperties();

            Outputs[0].SetPosition(rect.height / 2);

            if (GUI.changed) NodeEditor.RecalculateFrom(this);
        }

        public sealed override bool Calculate()
        {
            if (allInputsReady())
            {
                module = CreateAndReadyModule();

                NeedsUpdate = true;
                previewCalculation = null;

                Outputs[0].SetValue(module);

                NodeEditorWindow.NodeUpdated(this);

                return true;
            }
            else return false;
        }

        public void UpdatePreviewTexture()
        {
            if (previewCalculation == null)
            {
                previewCalculation = new NoiseCalculation(module, 128, 128);
            }
            else if (previewCalculation.Done)
            {
                preview = previewCalculation.Noise.GetTexture();
                previewCalculation = null;
                NeedsUpdate = false;
            }
        }

        protected virtual void DrawModuleProperties()
        {
        }

        protected abstract ModuleBase CreateAndReadyModule();
    }
}