using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class IRiverTool : RiverTool
{

#if UNITY_EDITOR

	[UnityEditor.MenuItem ("Tools/RiverTool/Add New")]
	static void AddNewRiver () {
		GameObject r = new GameObject();
		string scene = System.IO.Path.GetFileNameWithoutExtension(UnityEditor.EditorApplication.currentScene);
		string str = scene + GameObject.FindObjectsOfType(typeof(RiverTool)).Length.ToString();
		r.name = str + "_River";
		IRiverTool rt= r.AddComponent<IRiverTool>();
		rt.init();

		r.transform.position = UnityEditor.SceneView.currentDrawingSceneView.camera.transform.position 
			+ (UnityEditor.SceneView.currentDrawingSceneView.camera.transform.forward*10);

		GameObject p1 = new GameObject("P1");
		p1.transform.parent = r.transform;
		p1.transform.localPosition = Vector3.zero;
		GameObject p2 = new GameObject("P2");
		p2.transform.parent = r.transform;
		p2.transform.localPosition = Vector3.forward;
		GameObject p3 = new GameObject("P3");
		p3.transform.parent = r.transform;
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
	public override string getSceneName()
	{
		return System.IO.Path.GetFileNameWithoutExtension(UnityEditor.EditorApplication.currentScene);
	}
#endif

}
