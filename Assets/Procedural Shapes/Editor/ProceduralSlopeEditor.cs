using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralSlope))]
public class ProceduralSlopeEditor : Editor
{
	private ProceduralSlope shape;
	private SerializedProperty sizeX;
	private SerializedProperty sizeY;
	private SerializedProperty sizeZ;

	[MenuItem("GameObject/Create Other/Procedural Slope")]
	private static void CreateGameObject()
	{
		var go = new GameObject("Slope");
		go.AddComponent<ProceduralSlope>();
		Selection.activeGameObject = go;
	}

	private void OnEnable()
	{
		shape = (ProceduralSlope)target;
		sizeX = serializedObject.FindProperty("sizeX");
		sizeY = serializedObject.FindProperty("sizeY");
		sizeZ = serializedObject.FindProperty("sizeZ");

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

		EditorGUILayout.PropertyField(sizeX);
		if (sizeX.floatValue < 0)
		{
			sizeX.floatValue = 0;
		}

		EditorGUILayout.PropertyField(sizeY);
		if (sizeY.floatValue < 0)
		{
			sizeY.floatValue = 0;
		}

		EditorGUILayout.PropertyField(sizeZ);
		if (sizeZ.floatValue < 0)
		{
			sizeZ.floatValue = 0;
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
		return "Assets/Slope";
	}
}