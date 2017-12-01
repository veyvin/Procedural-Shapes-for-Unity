using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralTorus))]
public class ProceduralTorusEditor : Editor
{
	private ProceduralTorus shape;
	private SerializedProperty smoothNormals;
	private SerializedProperty radius;
	private SerializedProperty width;
	private SerializedProperty segments;
	private SerializedProperty sectors;
	private SerializedProperty flipAngle;
	private SerializedProperty phaseAngle;

	[MenuItem("GameObject/Create Other/Procedural Torus")]
	private static void CreateGameObject()
	{
		var go = new GameObject("Torus");
		go.AddComponent<ProceduralTorus>();
		Selection.activeGameObject = go;
	}

	private void OnEnable()
	{
		shape = (ProceduralTorus)target;
		radius = serializedObject.FindProperty("radius");
		smoothNormals = serializedObject.FindProperty("smoothNormals");
		width = serializedObject.FindProperty("width");
		segments = serializedObject.FindProperty("segments");
		sectors = serializedObject.FindProperty("sectors");
		flipAngle = serializedObject.FindProperty("flipAngle");
		phaseAngle = serializedObject.FindProperty("phaseAngle");

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

		EditorGUILayout.PropertyField(smoothNormals);
		EditorGUILayout.PropertyField(flipAngle);
		EditorGUILayout.PropertyField(phaseAngle);

		EditorGUILayout.PropertyField(radius);
		if (radius.floatValue < 0)
		{
			radius.floatValue = 0;
		}

		EditorGUILayout.PropertyField(width);
		if (width.floatValue < 0)
		{
			width.floatValue = 0;
		}

		EditorGUILayout.PropertyField(segments);
		if (segments.intValue < 3)
		{
			segments.intValue = 3;
		}

		EditorGUILayout.PropertyField(sectors);
		if (sectors.intValue < 2)
		{
			sectors.intValue = 2;
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
		return string.Format("Assets/Torus ({0})", sectors);
	}
}