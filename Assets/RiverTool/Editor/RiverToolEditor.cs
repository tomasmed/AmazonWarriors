using UnityEngine;

using System.Collections;
using System.Collections.Generic;

[UnityEditor.CustomEditor(typeof(IRiverTool))]
public class RiverToolEditor : UnityEditor.Editor {
	public override void OnInspectorGUI() {
		RiverTool rt = (RiverTool)target;
		GUILayout.BeginHorizontal();
		if(GUILayout.Button( "Init"))
		{
			string scene = System.IO.Path.GetFileNameWithoutExtension(UnityEditor.EditorApplication.currentScene);
			string str = scene + GameObject.FindObjectsOfType(typeof(RiverTool)).Length.ToString();
			rt.gameObject.name = str + "_River";

			rt.init();

			GameObject p1 = new GameObject("P1");
			p1.transform.parent = rt.transform;
			p1.transform.localPosition = Vector3.zero;
			GameObject p2 = new GameObject("P2");
			p2.transform.parent = rt.transform;
			p2.transform.localPosition = Vector3.forward;
			GameObject p3 = new GameObject("P3");
			p3.transform.parent = rt.transform;
			p3.transform.localPosition = Vector3.forward*2;
			p1.AddComponent<CurvePoint>();
			p2.AddComponent<CurvePoint>();
			p3.AddComponent<CurvePoint>();
			rt.points.Add(p1);
			rt.points.Add(p2);
			rt.points.Add(p3);

			rt.gameObject.AddComponent<TextureAnimator>();
			rt.gameObject.GetComponent<TextureAnimator>().Speed = new Vector2(0.1f, 0);
			
		}
		if(GUILayout.Button( "Delete"))
		{
			DestroyImmediate(rt.gameObject);
			return;
		}
		GUILayout.EndHorizontal();
		if(GUILayout.Button("Add Point"))
		{
			GameObject pn = new GameObject("P" + (rt.points.Count+1));
			int c = rt.points.Count;
			Vector3 p1 = rt.points[c-1].transform.localPosition;
			Vector3 p2 = rt.points[c-2].transform.localPosition;
			float d = Vector3.Distance(p1, p2);
			pn.transform.parent = rt.transform;
			pn.transform.localPosition = Vector3.MoveTowards(p1, p2, -d);
			pn.AddComponent<CurvePoint>();
			rt.points.Add(pn);
			UnityEditor.Selection.activeGameObject = pn;
		}
//		if(GUILayout.Button("Add Point"))
//		{
//			GameObject pn = new GameObject("P" + (rt.points.Count+1));
//			pn.transform.parent = rt.transform;
//			float d = Vector3.Distance(rt.points[rt.points.Count - 2].transform.localPosition, rt.points[rt.points.Count - 1].transform.localPosition);
//			pn.transform.localPosition = Vector3.MoveTowards(rt.points[rt.points.Count - 1].transform.localPosition, rt.points[rt.points.Count - 2].transform.localPosition, -d);
//			pn.AddComponent<CurvePoint>();
//			rt.points.Add(pn);
//			UnityEditor.Selection.activeGameObject = pn;
//			
//		}
		if (GUILayout.Button("Create Prefab"))
		{
			string filePath =UnityEditor.EditorUtility.SaveFilePanelInProject("Save Mesh Asset", rt.gameObject.name, "", "");
			if (filePath == "") return;
			
			UnityEditor.AssetDatabase.CreateAsset(rt.mat, filePath+ ".mat");
			UnityEditor.AssetDatabase.CreateAsset(rt.mesh, filePath+ ".asset");
			
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
			go.transform.position = rt.gameObject.transform.position;
			go.transform.rotation = rt.gameObject.transform.rotation;
			go.transform.localScale = rt.gameObject.transform.localScale;
			go.name = rt.gameObject.name;
			go.GetComponent<MeshFilter>().sharedMesh = rt.mesh;
			go.GetComponent<MeshRenderer>().sharedMaterial = rt.mat;
			go.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = rt.mat.mainTexture;
			go.AddComponent<TextureAnimator>();
			go.GetComponent<TextureAnimator>().Speed = rt.gameObject.GetComponent<TextureAnimator>().Speed;
			DestroyImmediate(go.GetComponent<MeshCollider>());
			UnityEditor.PrefabUtility.CreatePrefab(filePath+ ".prefab", go);
			UnityEditor.AssetDatabase.Refresh();
			return;
			
			
		}
		DrawDefaultInspector();
//		Debug.Log(target.ToString() + "\n"  + sw.levelList.Count + "\n"+ target.name);
	}
}
