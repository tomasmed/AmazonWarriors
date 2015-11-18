using UnityEngine;
using UnityEditor;

public class WeatherMenu : Editor {
	[MenuItem ("Weather FX/Rain", false, 25)]
	public static void CreateRain () {
		GameObject pre = (GameObject) Resources.Load ("Rain FX/Prefabs/Rain");
		GameObject ins = (GameObject) Instantiate (pre);

		ins.name = "Rain";
		ins.transform.DetachChildren ();
	}

	[MenuItem ("Weather FX/Soft Rain", false, 25)]
	public static void CreateSoftRain () {
		GameObject pre = (GameObject) Resources.Load ("Rain FX/Prefabs/Soft Rain");
		GameObject ins = (GameObject) Instantiate (pre);

		ins.name = "Soft Rain";
		ins.transform.DetachChildren ();
	}

	[MenuItem ("Weather FX/Heavy Rain", false, 25)]
	public static void CreateHeavyRain () {
		GameObject pre = (GameObject) Resources.Load ("Rain FX/Prefabs/Heavy Rain");
		GameObject ins = (GameObject) Instantiate (pre);

		ins.name = "Heavy Rain";
		ins.transform.DetachChildren ();
	}

	[MenuItem ("Weather FX/Snow", false, 40)]
	public static void CreateSnow () {
		GameObject pre = (GameObject) Resources.Load ("Snow FX/Prefabs/Snow");
		GameObject ins = (GameObject) Instantiate (pre);

		ins.name = "Snow";
		ins.transform.DetachChildren ();
	}
}
