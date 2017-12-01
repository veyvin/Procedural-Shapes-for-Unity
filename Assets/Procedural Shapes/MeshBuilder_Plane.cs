using UnityEngine;

public partial class MeshBuilder
{
	private static readonly Color color = Color.white;

	public static MeshBuilder CreatePlane(float sizeX, float sizeZ, int segmentsX, int segmentsZ, bool collisionMesh)
	{
		float dx = sizeX / segmentsX;
		float dy = sizeZ / segmentsZ;
		float x0 = -sizeX / 2;
		float z0 = -sizeZ / 2;

		var meshBuilder = new MeshBuilder(collisionMesh);
		var vertices = meshBuilder.vertices;
		var indices = meshBuilder.indices;

		var normals = meshBuilder.normals;
		var uvs = meshBuilder.uvs;
		var colors = meshBuilder.colors;

		for (int iz = 0; iz < segmentsZ + 1; iz++)
		{
			for (int ix = 0; ix < segmentsX + 1; ix++)
			{
				var position = new Vector3(x0 + ix * dx, 0, z0 + iz * dy);
				var uv = new Vector2(ix / (float)segmentsX, iz / (float)segmentsZ);
				var normal = Vector3.up;

				vertices.Add(position);
				if (collisionMesh == false)
				{
					normals.Add(normal);
					uvs.Add(uv);
					colors.Add(color);
				}
			}
		}

		for (int iz = 0; iz < segmentsZ; iz++)
		{
			for (int ix = 0; ix < segmentsX; ix++)
			{
				int firstVertexIndex = iz * (segmentsX + 1) + ix;
				indices.Add(firstVertexIndex + 1);
				indices.Add(firstVertexIndex);
				indices.Add(firstVertexIndex + segmentsX + 1);
				indices.Add(firstVertexIndex + segmentsX + 2);
				indices.Add(firstVertexIndex + 1);
				indices.Add(firstVertexIndex + segmentsX + 1);
			}
		}

		return meshBuilder;
	}
}