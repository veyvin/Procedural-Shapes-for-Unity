using UnityEngine;

public partial class MeshBuilder
{
	public static MeshBuilder CreateTorusSmooth(float flipAngle, float phaseAngle, float radius, float width, int segments, int sectors, bool collisionMesh)
	{
		var meshBuilder = new MeshBuilder(collisionMesh);
		var vertices = meshBuilder.vertices;
		var indices = meshBuilder.indices;

		var normals = meshBuilder.normals;
		var uvs = meshBuilder.uvs;
		var colors = meshBuilder.colors;

		for (int segment = 0; segment <= segments; segment++)
		{
			var center = new Vector3(radius, 0, 0);
			var yRotation = Quaternion.Euler(0, 360f * segment / segments, 0);
			var zRotation = Quaternion.Euler(0, 0, phaseAngle + flipAngle * segment / segments);

			for (int sector = 0; sector <= sectors; sector++)
			{
				float angle = Mathf.PI * 2f / sectors * sector;
				var normal = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

				var position = center + zRotation * (width * normal);
				vertices.Add(yRotation * position);

				if (collisionMesh == false)
				{
					var uv = new Vector2(1 - segment / (float)segments, sector / (float)sectors);

					normals.Add(yRotation * zRotation * normal);
					uvs.Add(uv);
					colors.Add(color);
				}
			}
		}

		for (int segment = 0; segment < segments; segment++)
		{
			for (int sector = 0; sector < sectors; sector++)
			{
				int firstVertexIndex = segment * (sectors + 1) + sector;
				indices.Add(firstVertexIndex + 1);
				indices.Add(firstVertexIndex);
				indices.Add(firstVertexIndex + sectors + 1);
				indices.Add(firstVertexIndex + sectors + 2);
				indices.Add(firstVertexIndex + 1);
				indices.Add(firstVertexIndex + sectors + 1);
			}
		}

		return meshBuilder;
	}

	public static MeshBuilder CreateTorusFlat(float flipAngle, float phaseAngle, float radius, float width, int segments, int sectors, bool collisionMesh)
	{
		var meshBuilder = new MeshBuilder(collisionMesh);
		var vertices = meshBuilder.vertices;
		var indices = meshBuilder.indices;

		var normals = meshBuilder.normals;
		var uvs = meshBuilder.uvs;
		var colors = meshBuilder.colors;

		var deltaAngle = Mathf.PI * 2 / sectors;

		for (int segment = 0; segment <= segments; segment++)
		{
			var center = new Vector3(radius, 0, 0);
			var yRotation = Quaternion.Euler(0, 360f * segment / segments, 0);
			var zRotation = Quaternion.Euler(0, 0, phaseAngle + flipAngle * segment / segments);

			for (int sector = 0; sector < sectors; sector++)
			{
				var angle = Mathf.PI * 2 / sectors * sector;

				var normal1 = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
				var normal2 = new Vector3(Mathf.Cos(angle + deltaAngle), Mathf.Sin(angle + deltaAngle), 0);

				var position1 = center + zRotation * (width * normal1);
				var position2 = center + zRotation * (width * normal2);

				vertices.Add(yRotation * position1);
				vertices.Add(yRotation * position2);

				if (collisionMesh == false)
				{
					var normal = yRotation * zRotation * new Vector3(Mathf.Cos(angle + deltaAngle / 2), Mathf.Sin(angle + deltaAngle / 2), 0);
					var uv1 = new Vector2(1 - segment / (float)segments, sector / (float)sectors);
					var uv2 = new Vector2(1 - segment / (float)segments, (sector + 1) / (float)sectors);

					normals.Add(normal);
					normals.Add(normal);
					uvs.Add(uv1);
					uvs.Add(uv2);
					colors.Add(color);
					colors.Add(color);
				}
			}
		}

		for (int segment = 0; segment < segments; segment++)
		{
			for (int sector = 0; sector < sectors; sector++)
			{
				int firstVertexIndex1 = segment * sectors * 2 + sector * 2;
				int firstVertexIndex2 = (segment + 1) * sectors * 2 + sector * 2;
				indices.Add(firstVertexIndex1 + 1);
				indices.Add(firstVertexIndex1);
				indices.Add(firstVertexIndex2);
				indices.Add(firstVertexIndex2);
				indices.Add(firstVertexIndex2 + 1);
				indices.Add(firstVertexIndex1 + 1);
			}
		}

		return meshBuilder;
	}
}