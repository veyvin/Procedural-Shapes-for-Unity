using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralCylinder))]
public class ProceduralCylinderEditor : Editor
{
	private ProceduralCylinder shape;
	private SerializedProperty pivotPosition;
	private SerializedProperty smoothNormals;
	private SerializedProperty height;
	private SerializedProperty radius;
	private SerializedProperty segments;
	private SerializedProperty sectors;

	[MenuItem("GameObject/Create Other/Procedural Cylinder")]
	private static void CreateGameObject()
	{
		var go = new GameObject("Cylinder");
		go.AddComponent<ProceduralCylinder>();
		Selection.activeGameObject = go;
	}

	private void OnEnable()
	{
		shape = (ProceduralCylinder)target;
		pivotPosition = serializedObject.FindProperty("pivotPosition");
		smoothNormals = serializedObject.FindProperty("smoothNormals");
		height = serializedObject.FindProperty("height");
		radius = serializedObject.FindProperty("radius");
		segments = serializedObject.FindProperty("segments");
		sectors = serializedObject.FindProperty("sectors");

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

		EditorGUILayout.PropertyField(pivotPosition);
		EditorGUILayout.PropertyField(smoothNormals);

		EditorGUILayout.PropertyField(height);
		if (height.floatValue < 0)
		{
			height.floatValue = 0;
		}

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

		EditorGUILayout.PropertyField(sectors);
		if (sectors.intValue < 3)
		{
			sectors.intValue = 3;
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
		return string.Format("Assets/Cylinder ({0}x{1})", segments.intValue, sectors.intValue);
	}
}