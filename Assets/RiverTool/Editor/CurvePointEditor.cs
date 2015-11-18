using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[UnityEditor.CustomEditor(typeof(CurvePoint))]
public class CurvePointEditor : UnityEditor.Editor {
	public override void OnInspectorGUI() {
		CurvePoint rt = (CurvePoint)target;
		if(GUILayout.Button("Add Point"))
		{
//			int i = rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points.IndexOf( rt.gameObject);
			GameObject pn = new GameObject("P" + (rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points.Count+1));
			int c = rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points.Count;
			Vector3 p1 = rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points[c-1].transform.localPosition;
			Vector3 p2 = rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points[c-2].transform.localPosition;
			float d = Vector3.Distance(p1, p2);
			pn.transform.parent = rt.gameObject.transform.parent.gameObject.transform;
			pn.transform.localPosition = Vector3.MoveTowards(p1, p2, -d);
			pn.AddComponent<CurvePoint>();
			rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points.Add(pn);
			UnityEditor.Selection.activeGameObject = pn;
		}
		if(GUILayout.Button("Add Point To Next"))
		{
			int i = rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points.IndexOf( rt.gameObject);
			int c = rt.gameObject.transform.parent.transform.childCount;
			if (i> c-2)
				return;
			GameObject pn = new GameObject("P" + (rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points.Count+1));
			Vector3 p1 = rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points[i].transform.localPosition;
			Vector3 p2 = rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points[i+1].transform.localPosition;
			float d = Vector3.Distance(p1, p2);
			pn.transform.parent = rt.gameObject.transform.parent.gameObject.transform;
			pn.transform.localPosition = Vector3.MoveTowards(p1, p2, d/2);
			pn.AddComponent<CurvePoint>();
			rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points.Insert(i+1, pn);
			UnityEditor.Selection.activeGameObject = pn;
		}
		if(GUILayout.Button("Add Point To Previous"))
		{
			int i = rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points.IndexOf(rt.gameObject);
			if(i < 1)
				return;
			GameObject pn = new GameObject("P" + (rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points.Count+1));
			//int c = rt.gameObject.transform.parent.transform.childCount;
			Vector3 p1 = rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points[i].transform.localPosition;
			Vector3 p2 = rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points[i-1].transform.localPosition;
			float d = Vector3.Distance(p1, p2);
			pn.transform.parent = rt.gameObject.transform.parent.gameObject.transform;
			pn.transform.localPosition = Vector3.MoveTowards(p1, p2, d/2);
			pn.AddComponent<CurvePoint>();
			rt.gameObject.transform.parent.gameObject.GetComponent<RiverTool>().points.Insert(i, pn);
			
			UnityEditor.Selection.activeGameObject = pn;
		}
		DrawDefaultInspector();
//		Debug.Log(target.ToString() + "\n"  + sw.levelList.Count + "\n"+ target.name);
	}
}
