using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralPlane))]
public class ProceduralPlaneEditor : Editor
{
	private ProceduralPlane shape;
	private SerializedProperty sizeX;
	private SerializedProperty sizeZ;
	private SerializedProperty segmentsX;
	private SerializedProperty segmentsZ;

	[MenuItem("GameObject/Create Other/Procedural Plane")]
	private static void CreateGameObject()
	{
		var go = new GameObject("Plane");
		go.AddComponent<ProceduralPlane>();
		Selection.activeGameObject = go;
	}

	private void OnEnable()
	{
		shape = (ProceduralPlane)target;
		sizeX = serializedObject.FindProperty("sizeX");
		sizeZ = serializedObject.FindProperty("sizeZ");
		segmentsX = serializedObject.FindProperty("segmentsX");
		segmentsZ = serializedObject.FindProperty("segmentsZ");

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

		EditorGUILayout.PropertyField(sizeZ);
		if (sizeZ.floatValue < 0)
		{
			sizeZ.floatValue = 0;
		}

		EditorGUILayout.PropertyField(segmentsX);
		if (segmentsX.intValue < 1)
		{
			segmentsX.intValue = 1;
		}

		EditorGUILayout.PropertyField(segmentsZ);
		if (segmentsZ.intValue < 1)
		{
			segmentsZ.intValue = 1;
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
		return string.Format("Assets/Plane ({0}x{1})", segmentsX.intValue, segmentsZ.intValue);
	}
}