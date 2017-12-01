using UnityEngine;

public abstract class ProceduralShape : MonoBehaviour
{
	private void Reset()
	{
		UpdateMesh();
	}
	
	public void UpdateMesh()
	{
		var meshFilter = GetComponent<MeshFilter>();
		var mesh = CreateMesh(meshFilter.sharedMesh);
		meshFilter.sharedMesh = mesh;
	}
	
	protected abstract Mesh CreateMesh(Mesh mesh = null);

	/// <summary>
	/// Shows normals
	/// </summary>
	private void _OnDrawGizmosSelected()
	{
		var mesh = GetComponent<MeshFilter>().sharedMesh;
		var normals = mesh.normals;
		var vertices = mesh.vertices;

		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.red;
		for (int i = 0; i < vertices.Length; i++)
		{
			Gizmos.DrawLine(vertices[i], vertices[i] + normals[i] * .1f);
		}
		Gizmos.matrix = Matrix4x4.identity;
	}
}