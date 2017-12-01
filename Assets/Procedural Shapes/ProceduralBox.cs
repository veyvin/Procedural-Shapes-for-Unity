using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralBox : ProceduralShape
{
	public float sizeX = 1;
	public float sizeY = 1;
	public float sizeZ = 1;
	public int segmentsX = 2;
	public int segmentsY = 2;
	public int segmentsZ = 2;

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

		var meshBuilder = MeshBuilder.CreateBox(sizeX, sizeY, sizeZ, segmentsX, segmentsY, segmentsZ, false);
		if (meshBuilder.vertices.Count <= 65535)
		{
			mesh = meshBuilder.ToMesh(mesh);
			if (mesh.name == string.Empty)
			{
				mesh.name = "Box";
			}
		}

		return mesh;
	}
}