using System;
using UnityEngine;

public partial class MeshBuilder
{
	public static MeshBuilder CreateConeFlat(float height, float lowerRadius, float upperRadius, int segments, int sectors, bool collisionMesh)
	{
		var meshBuilder = new MeshBuilder(collisionMesh);
		var vertices = meshBuilder.vertices;
		var indices = meshBuilder.indices;

		var normals = meshBuilder.normals;
		var uvs = meshBuilder.uvs;
		var colors = meshBuilder.colors;

		for (int segment = 0; segment <= segments; segment++)
		{
			var center = new Vector3(0, height * segment / segments, 0);
			float currentRadius = Mathf.Lerp(lowerRadius, upperRadius, segment / (float)segments);

			var dAngle = Math.PI * 2 / sectors;
			var angle = 0d;

			for (int sector = 0; sector < sectors; sector++)
			{
				double dx1 = Math.Cos(angle);
				double dz1 = Math.Sin(angle);
				double dx2 = Math.Cos(angle + dAngle);
				double dz2 = Math.Sin(angle + dAngle);

				var normal1 = new Vector3((float)dx1, 0, (float)dz1);
				var normal2 = new Vector3((float)dx2, 0, (float)dz2);
				var position1 = center + currentRadius * normal1;
				var position2 = center + currentRadius * normal2;

				normal1.y = (lowerRadius - upperRadius) / height;
				normal2.y = (lowerRadius - upperRadius) / height;
				var normal = (normal1 + normal2).normalized;

				var uv1 = new Vector2(sector / (float)sectors, segment / (float)segments);
				var uv2 = new Vector2((sector + 1) / (float)sectors, segment / (float)segments);

				vertices.Add(position1);
				vertices.Add(position2);
				if (collisionMesh == false)
				{
					normals.Add(normal);
					normals.Add(normal);
					uvs.Add(uv1);
					uvs.Add(uv2);
					colors.Add(color);
					colors.Add(color);
				}

				angle += dAngle;
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

		if (lowerRadius > 0)
		{
			var lowerCap = CreateDisc(lowerRadius, sectors, false, collisionMesh);
			meshBuilder.Append(lowerCap, Vector3.zero);
		}

		if (upperRadius > 0)
		{
			var upperCap = CreateDisc(upperRadius, sectors, true, collisionMesh);
			meshBuilder.Append(upperCap, new Vector3(0, height, 0));
		}

		return meshBuilder;
	}

	public static MeshBuilder CreateConeSmooth(float height, float lowerRadius, float upperRadius, int segments, int sectors, bool collisionMesh)
	{
		var meshBuilder = new MeshBuilder(collisionMesh);
		var vertices = meshBuilder.vertices;
		var indices = meshBuilder.indices;

		var normals = meshBuilder.normals;
		var uvs = meshBuilder.uvs;
		var colors = meshBuilder.colors;

		for (int segment = 0; segment <= segments; segment++)
		{
			var center = new Vector3(0, height * segment / segments, 0);
			float currentRadius = Mathf.Lerp(lowerRadius, upperRadius, segment / (float)segments);

			for (int sector = 0; sector <= sectors; sector++)
			{
				double angle = Math.PI * 2 / sectors * sector;
				double dx = Math.Cos(angle);
				double dz = Math.Sin(angle);

				var normal = new Vector3((float)dx, 0, (float)dz);
				var position = center + currentRadius * normal;
				normal.y = (lowerRadius - upperRadius) / height;
				normal.Normalize();
				var uv = new Vector2(sector / (float)sectors, segment / (float)segments);

				vertices.Add(position);
				if (collisionMesh == false)
				{
					normals.Add(normal);
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

		if (lowerRadius > 0)
		{
			var lowerCap = CreateDisc(lowerRadius, sectors, false, collisionMesh);
			meshBuilder.Append(lowerCap, Vector3.zero);
		}

		if (upperRadius > 0)
		{
			var upperCap = CreateDisc(upperRadius, sectors, true, collisionMesh);
			meshBuilder.Append(upperCap, new Vector3(0, height, 0));
		}

		return meshBuilder;
	}

	public static MeshBuilder CreateDisc(float radius, int sectors, bool looksUp, bool collisionMesh)
	{
		var meshBuilder = new MeshBuilder(collisionMesh);
		var vertices = meshBuilder.vertices;
		var indices = meshBuilder.indices;

		var normals = meshBuilder.normals;
		var uvs = meshBuilder.uvs;
		var colors = meshBuilder.colors;

		for (int sector = 0; sector <= sectors; sector++)
		{
			double angle = Math.PI * 2 / sectors * sector;
			double dx = Math.Cos(angle);
			double dz = Math.Sin(angle);
			var normal = new Vector3((float)dx, 0, (float)dz);

			vertices.Add(radius * normal);

			if (collisionMesh == false)
			{
				uvs.Add(new Vector2((float)(-.5f + dx), (float)(-.5f + dz)));
				normals.Add(looksUp ? Vector3.up : Vector3.down);
				colors.Add(color);
			}
		}

		for (int sector = 0; sector < sectors - 2; sector++)
		{
			if (looksUp)
			{
				indices.Add(0);
				indices.Add(sector + 2);
				indices.Add(sector + 1);
			}
			else
			{
				indices.Add(0);
				indices.Add(sector + 1);
				indices.Add(sector + 2);
			}
		}

		return meshBuilder;
	}
}