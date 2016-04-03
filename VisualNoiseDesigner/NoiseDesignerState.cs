using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using NodeEditorFramework;

namespace VisualNoiseDesigner
{
    [Serializable]
    public class NoiseDesignerState : NodeEditorState
    {
        public Vector2 textureSize = new Vector2(256, 256);
    }
}