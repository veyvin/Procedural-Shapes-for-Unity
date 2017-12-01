using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralTorus : ProceduralShape
{
	public bool smoothNormals = true;
	public float flipAngle = 0f;
	public float phaseAngle = 45f;
	public float radius = 1f;
	public float width = .25f;
	public int segments = 32;
	public int sectors = 4;

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

		var meshBuilder = smoothNormals ? MeshBuilder.CreateTorusSmooth(flipAngle, phaseAngle, radius, width, segments, sectors, false) :
			MeshBuilder.CreateTorusFlat(flipAngle, phaseAngle, radius, width, segments, sectors, false);

		if (meshBuilder.vertices.Count <= 65535)
		{
			mesh = meshBuilder.ToMesh(mesh);
			if (mesh.name == string.Empty)
			{
				mesh.name = "Torus";
			}
		}

		return mesh;
	}
}