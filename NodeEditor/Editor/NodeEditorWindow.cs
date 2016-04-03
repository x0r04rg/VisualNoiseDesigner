using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using VisualNoiseDesigner;
using LibNoise;

namespace NodeEditorFramework
{
	public class NodeEditorWindow : EditorWindow 
	{
		// Information about current instance
		private static NodeEditorWindow _editor;
		public static NodeEditorWindow editor { get { AssureEditor (); return _editor; } }
		public static void AssureEditor () { if (_editor == null) CreateEditor (); }

		// Opened Canvas
		public NodeCanvas mainNodeCanvas;
		public NodeEditorState mainEditorState;
		public static NodeCanvas MainNodeCanvas { get { return editor.mainNodeCanvas; } }
		public static NodeEditorState MainEditorState { get { return editor.mainEditorState; } }
		public void AssureCanvas () { if (mainNodeCanvas == null) NewNodeCanvas (); }
		public static string openedCanvasPath;
		public string tempSessionPath;

		// GUI
		public static int sideWindowWidth = 400;
		private static Texture iconTexture;
		public Rect sideWindowRect { get { return new Rect (position.width - sideWindowWidth, 0, sideWindowWidth, position.height); } }
		public Rect canvasWindowRect { get { return new Rect (0, 0, position.width - sideWindowWidth, position.height); } }

        Texture2D preview = new Texture2D(256, 256);
        NoiseCalculation previewCalculation;
        bool previewNeedsUpdate = true;
        NodeBase lastSelected;
        TerrainData terrain;

		#region General 

		[MenuItem ("Window/Visual Noise Designer")]
		public static void CreateEditor () 
		{
            _editor = GetWindow<NodeEditorWindow>(typeof(SceneView));
			_editor.minSize = new Vector2 (800, 600);
			NodeEditor.ClientRepaints += _editor.Repaint;
			NodeEditor.initiated = NodeEditor.InitiationError = false;

			iconTexture = ResourceManager.LoadTexture (EditorGUIUtility.isProSkin? "Textures/Icon_Dark.png" : "Textures/Icon_Light.png");
			_editor.titleContent = new GUIContent ("Noise Designer", iconTexture);
            if(_editor.mainNodeCanvas != null && _editor.mainNodeCanvas.nodes != null && _editor.mainNodeCanvas.nodes.Count > 0)
                NodeEditor.RecalculateAll(_editor.mainNodeCanvas);
        }
		
		/// <summary>
		/// Handle opening canvas when double-clicking asset
		/// </summary>
		[UnityEditor.Callbacks.OnOpenAsset(1)]
		public static bool AutoOpenCanvas (int instanceID, int line) 
		{
			if (Selection.activeObject != null && Selection.activeObject.GetType () == typeof(NodeCanvas))
			{
				string NodeCanvasPath = AssetDatabase.GetAssetPath (instanceID);
				NodeEditorWindow.CreateEditor ();
				EditorWindow.GetWindow<NodeEditorWindow> ().LoadNodeCanvas (NodeCanvasPath);
				return true;
			}
			return false;
		}

		public void OnDestroy () 
		{
			NodeEditor.ClientRepaints -= _editor.Repaint;

	#if UNITY_EDITOR
			// Remove callbacks
			EditorLoadingControl.lateEnteredPlayMode -= LoadCache;
			EditorLoadingControl.justLeftPlayMode -= LoadCache;
			EditorLoadingControl.justOpenedNewScene -= LoadCache;

			NodeEditorCallbacks.OnAddNode -= SaveNewNode;
	#endif
		}

		// Following section is all about caching the last editor session

		private void OnEnable () 
		{
			tempSessionPath = Path.GetDirectoryName (AssetDatabase.GetAssetPath (MonoScript.FromScriptableObject (this)));
			LoadCache ();

	#if UNITY_EDITOR
			// This makes sure the Node Editor is reinitiated after the Playmode changed
			EditorLoadingControl.lateEnteredPlayMode -= LoadCache;
			EditorLoadingControl.lateEnteredPlayMode += LoadCache;

			EditorLoadingControl.justLeftPlayMode -= LoadCache;
			EditorLoadingControl.justLeftPlayMode += LoadCache;

			EditorLoadingControl.justOpenedNewScene -= LoadCache;
			EditorLoadingControl.justOpenedNewScene += LoadCache;

			NodeEditorCallbacks.OnAddNode -= SaveNewNode;
			NodeEditorCallbacks.OnAddNode += SaveNewNode;
	#endif
		}

        void UpdatePreviewTextures()
        {
            foreach (NodeBase node in mainNodeCanvas.nodes)
                if (node.NeedsUpdate) node.UpdatePreviewTexture();
        }

        public static void NodeUpdated(NodeBase node)
        {
            if (ReferenceEquals(MainEditorState.selectedNode, node))
            {
                editor.previewCalculation = null;
                editor.previewNeedsUpdate = true;
            }
        }

		#endregion

		#region GUI

		private void OnGUI () 
		{
			// Initiation
			NodeEditor.checkInit ();
			if (NodeEditor.InitiationError) 
			{
				GUILayout.Label ("Node Editor Initiation failed! Check console for more information!");
				return;
			}
			AssureEditor ();
			AssureCanvas ();

			// Specify the Canvas rect in the EditorState
			mainEditorState.canvasRect = canvasWindowRect;
			// If you want to use GetRect:
//			Rect canvasRect = GUILayoutUtility.GetRect (600, 600);
//			if (Event.current.type != EventType.Layout)
//				mainEditorState.canvasRect = canvasRect;

			// Perform drawing with error-handling
			try
			{
				NodeEditor.DrawCanvas (mainNodeCanvas, mainEditorState);
			}
			catch (UnityException e)
			{ // on exceptions in drawing flush the canvas to avoid locking the ui.
				NewNodeCanvas ();
				NodeEditor.ReInit (true);
				Debug.LogError ("Unloaded Canvas due to exception when drawing!");
				Debug.LogException (e);
			}

			// Draw Side Window
			sideWindowWidth = Math.Min (600, Math.Max (200, (int)(position.width / 5)));
			NodeEditorGUI.StartNodeGUI ();
			GUILayout.BeginArea (sideWindowRect, GUI.skin.box);
			DrawSideWindow ();
			GUILayout.EndArea ();
			NodeEditorGUI.EndNodeGUI ();

            UpdatePreviewTextures();
		}

		private void DrawSideWindow () 
		{
			GUILayout.Label (new GUIContent ("Visual Noise Designer (" + mainNodeCanvas.name + ")", "Opened Design path: " + openedCanvasPath), NodeEditorGUI.nodeLabelBold);

			if (GUILayout.Button (new GUIContent ("Save Design")))
			{
				string path = EditorUtility.SaveFilePanelInProject ("Save Noise Design", "Noise Design", "asset", "", NodeEditor.editorPath + "Resources/Saves/");
				if (!string.IsNullOrEmpty (path))
					SaveNodeCanvas (path);
			}

			if (GUILayout.Button (new GUIContent ("Load Design"))) 
			{
				string path = EditorUtility.OpenFilePanel ("Load Noise Design", NodeEditor.editorPath + "Resources/Saves/", "asset");
				if (!path.Contains (Application.dataPath)) 
				{
					if (!string.IsNullOrEmpty (path))
						ShowNotification (new GUIContent ("You should select an asset inside your project folder!"));
				}
				else
				{
					path = path.Replace (Application.dataPath, "Assets");
					LoadNodeCanvas (path);
				}
			}

			if (GUILayout.Button (new GUIContent ("New Design")))
				NewNodeCanvas ();

            if (mainEditorState.selectedNode != null)
                if (Event.current.type != EventType.Ignore)
                {
                    try
                    {
                        DrawSelectedNodeDetails((NodeBase)mainEditorState.selectedNode);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("Exception while drawing node details: " + ex.Message);
                    }
                }
        }

        void DrawSelectedNodeDetails(NodeBase node)
        {
            if (previewNeedsUpdate || lastSelected != node)
            {
                preview = new Texture2D(230, 230);
                if (node.Module != null && previewCalculation == null)
                    previewCalculation = new NoiseCalculation(node.Module, 230, 230);
                previewNeedsUpdate = false;
                lastSelected = node;
            }

            if (previewCalculation != null && previewCalculation.Done)
            {
                preview = previewCalculation.Noise.GetTexture();
                previewCalculation = null;
            }

            var state = mainEditorState as NoiseDesignerState;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Selected Node:");
            GUILayout.Box(preview);
            EditorGUILayout.Space();
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Apply to terrain:");
            terrain = EditorGUILayout.ObjectField("TerrainData", terrain, typeof(TerrainData), false) as TerrainData;
            if (GUILayout.Button("Apply"))
            {
                Noise2D noise = new Noise2D(terrain.heightmapWidth, terrain.heightmapHeight, node.Module);
                noise.GeneratePlanar(4.0f, 10.0f, 1.0f, 5.0f);
                terrain.SetHeights(0, 0, noise.GetNormalizedData());
            }
            EditorGUILayout.Space();
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Export as texture:");
            state.textureSize = EditorGUILayout.Vector2Field("Texture Size", state.textureSize);
            if (GUILayout.Button("Save as PNG"))
            {
                var path = EditorUtility.SaveFilePanelInProject("Save as PNG", "noise", "png", "");
                if (!string.IsNullOrEmpty(path))
                {
                    Noise2D noise = new Noise2D((int)state.textureSize.x, (int)state.textureSize.y, node.Module);
                    noise.GeneratePlanar(4.0f, 10.0f, 1.0f, 5.0f);
                    var texture = noise.GetTexture();
                    File.WriteAllBytes(path, texture.EncodeToPNG());
                    AssetDatabase.Refresh();
                }
            }
        }

		#endregion

		#region Cache

		private void SaveNewNode (Node node) 
		{
			if (!mainNodeCanvas.nodes.Contains (node))
				throw new UnityException ("Cache system: Writing new Node to save file failed as Node is not part of the Cache!");
			string path = tempSessionPath + "/LastSession.asset";
			if (AssetDatabase.GetAssetPath (mainNodeCanvas) != path)
				throw new UnityException ("Cache system error: Current Canvas is not saved as the temporary cache!");
			NodeEditorSaveManager.AddSubAsset (node, path);
			foreach (NodeKnob knob in node.nodeKnobs)
				NodeEditorSaveManager.AddSubAsset (knob, path);

			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}

		private void SaveCache () 
		{
			string canvasName = mainNodeCanvas.name;
			EditorPrefs.SetString ("NodeEditorLastSession", canvasName);
			NodeEditorSaveManager.SaveNodeCanvas (tempSessionPath + "/LastSession.asset", false, mainNodeCanvas, mainEditorState);
			mainNodeCanvas.name = canvasName;

			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}

		private void LoadCache () 
		{
			string lastSessionName = EditorPrefs.GetString ("NodeEditorLastSession");
			string path = tempSessionPath + "/LastSession.asset";
			mainNodeCanvas = NodeEditorSaveManager.LoadNodeCanvas (path, false);
			if (mainNodeCanvas == null)
				NewNodeCanvas ();
			else 
			{
				mainNodeCanvas.name = lastSessionName;
				List<NodeEditorState> editorStates = NodeEditorSaveManager.LoadEditorStates (path, false);
				if (editorStates == null || editorStates.Count == 0 || (mainEditorState = editorStates.Find (x => x.name == "MainEditorState")) == null )
				{ // New NodeEditorState
					mainEditorState = CreateInstance<NodeEditorState> ();
					mainEditorState.canvas = mainNodeCanvas;
					mainEditorState.name = "MainEditorState";
					NodeEditorSaveManager.AddSubAsset (mainEditorState, path);
					AssetDatabase.SaveAssets ();
					AssetDatabase.Refresh ();
				}
			}
		}

		private void DeleteCache () 
		{
			string lastSession = EditorPrefs.GetString ("NodeEditorLastSession");
			if (!String.IsNullOrEmpty (lastSession))
			{
				AssetDatabase.DeleteAsset (tempSessionPath + "/" + lastSession);
				AssetDatabase.Refresh ();
			}
			EditorPrefs.DeleteKey ("NodeEditorLastSession");
		}

		#endregion

		#region Save/Load
		
		/// <summary>
		/// Saves the mainNodeCanvas and it's associated mainEditorState as an asset at path
		/// </summary>
		public void SaveNodeCanvas (string path) 
		{
			NodeEditorSaveManager.SaveNodeCanvas (path, true, mainNodeCanvas, mainEditorState);
			Repaint ();
		}
		
		/// <summary>
		/// Loads the mainNodeCanvas and it's associated mainEditorState from an asset at path
		/// </summary>
		public void LoadNodeCanvas (string path) 
		{
			// Load the NodeCanvas
			mainNodeCanvas = NodeEditorSaveManager.LoadNodeCanvas (path, true);
			if (mainNodeCanvas == null) 
			{
				Debug.Log ("Error loading NodeCanvas from '" + path + "'!");
				NewNodeCanvas ();
				return;
			}
			
			// Load the associated MainEditorState
			List<NodeEditorState> editorStates = NodeEditorSaveManager.LoadEditorStates (path, true);
			if (editorStates.Count == 0) 
			{
				mainEditorState = ScriptableObject.CreateInstance<NodeEditorState> ();
				Debug.LogError ("The save file '" + path + "' did not contain an associated NodeEditorState!");
			}
			else 
			{
				mainEditorState = editorStates.Find (x => x.name == "MainEditorState");
				if (mainEditorState == null) mainEditorState = editorStates[0];
			}
			mainEditorState.canvas = mainNodeCanvas;

			openedCanvasPath = path;
			NodeEditor.RecalculateAll (mainNodeCanvas);
			SaveCache ();
			Repaint ();
		}

		/// <summary>
		/// Creates and opens a new empty node canvas
		/// </summary>
		public void NewNodeCanvas () 
		{
			// New NodeCanvas
			mainNodeCanvas = CreateInstance<NodeCanvas> ();
			mainNodeCanvas.name = "New Canvas";
			// New NodeEditorState
			mainEditorState = CreateInstance<NoiseDesignerState> ();
			mainEditorState.canvas = mainNodeCanvas;
			mainEditorState.name = "MainEditorState";

			openedCanvasPath = "";
			SaveCache ();
		}
		
		#endregion
	}
}