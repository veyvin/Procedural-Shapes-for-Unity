using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralSphere))]
public class ProceduralSphereEditor : Editor
{
	private ProceduralSphere shape;
	private SerializedProperty radius;
	private SerializedProperty segments;

	[MenuItem("GameObject/Create Other/Procedural Sphere")]
	private static void CreateGameObject()
	{
		var go = new GameObject("Sphere");
		go.AddComponent<ProceduralSphere>();
		Selection.activeGameObject = go;
	}

	private void OnEnable()
	{
		shape = (ProceduralSphere)target;
		radius = serializedObject.FindProperty("radius");
		segments = serializedObject.FindProperty("segments");

		var meshFilter = shape.GetComponent<MeshFilter>();
		var mesh = meshFilter.sharedMesh;
		if (mesh == null)
		{
			shape.UpdateMesh();
		}
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(radius);
		if (radius.floatValue < 0)
		{
			radius.floatValue = 0;
		}

		EditorGUILayout.PropertyField(segments);
		if (segments.intValue < 1)
		{
			segments.intValue = 1;
		}

		bool propertyChanged = serializedObject.ApplyModifiedProperties();
		if (propertyChanged)
		{
			shape.UpdateMesh();
		}

		if (GUILayout.Button("Bake"))
		{
			var mesh = shape.GetComponent<MeshFilter>().sharedMesh;
			CreateMeshAsset(mesh);
			DestroyImmediate(serializedObject.targetObject);
		}

		if (GUILayout.Button("Create mesh asset"))
		{
			var mesh = shape.GetComponent<MeshFilter>().sharedMesh;
			CreateMeshAsset(mesh);
		}
	}

	private void CreateMeshAsset(Mesh mesh)
	{
		var assetName = GenerateAssetName();
		assetName = AssetDatabase.GenerateUniqueAssetPath(assetName + ".asset");
		AssetDatabase.CreateAsset(mesh, assetName);
		AssetDatabase.SaveAssets();
		Selection.activeObject = mesh;
	}

	private string GenerateAssetName()
	{
		return string.Format("Assets/Sphere ({0})", segments.intValue);
	}
}