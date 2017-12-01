using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralSphere : ProceduralShape
{
	public float radius = 1;
	public int segments = 4;

	protected override Mesh CreateMesh(Mesh mesh = null)
	{
		if (mesh == null)
		{
			mesh = new Mesh();
		}
		else
		{
			mesh.Clear();
		}

		var meshBuilder = MeshBuilder.CreateSphericalCube(radius, segments, false);

		if (meshBuilder.vertices.Count <= 65535)
		{
			mesh = meshBuilder.ToMesh(mesh);
			if (mesh.name == string.Empty)
			{
				mesh.name = "Sphere";
			}
		}

		return mesh;
	}
}