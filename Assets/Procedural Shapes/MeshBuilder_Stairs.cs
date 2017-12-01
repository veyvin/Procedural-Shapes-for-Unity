using UnityEngine;

public partial class MeshBuilder
{
	public static MeshBuilder CreateStairs(int stairs, float sizeX, float sizeY, float sizeZ, bool collisionMesh)
	{
		var leftSide = CreateStairsSide(stairs, true, collisionMesh);
		leftSide.Transform(new Vector3(-sizeX / 2, 0, 0), Quaternion.identity, new Vector3(sizeX, sizeY, sizeZ));

		var rightSide = CreateStairsSide(stairs, false, collisionMesh);
		rightSide.Transform(new Vector3(sizeX / 2, 0, 0), Quaternion.identity, new Vector3(sizeX, sizeY, sizeZ));

		var farSide = CreatePlane(sizeX, sizeY, 1, 1, collisionMesh);
		farSide.Transform(new Vector3(0, sizeY / 2, sizeZ), Quaternion.Euler(90, 0, 0));

		var bottom = CreatePlane(sizeX, sizeZ, 1, 1, collisionMesh);
		bottom.Transform(new Vector3(0, 0, sizeZ / 2), Quaternion.Euler(180, 0, 0));

		var stairsGeometry = CreateStairs(stairs, collisionMesh);
		stairsGeometry.Transform(new Vector3(-sizeX / 2, 0, 0), Quaternion.identity, new Vector3(sizeX, sizeY, sizeZ));

		return Combine(leftSide, rightSide, farSide, bottom, stairsGeometry);
	}

	private static MeshBuilder CreateStairsSide(int stairs, bool leftSide, bool collisionMesh)
	{
		var meshBuilder = new MeshBuilder(collisionMesh);
		var vertices = meshBuilder.vertices;
		var indices = meshBuilder.indices;
		var normals = meshBuilder.normals;
		var uvs = meshBuilder.uvs;
		var colors = meshBuilder.colors;

		float y = 0;
		float dy = 1f / stairs;

		float z = 0;
		float dz = 1f / stairs;

		for (int i = 0; i < stairs; i++)
		{
			vertices.Add(new Vector3(0, y, z));
			vertices.Add(new Vector3(0, y + dy, z));
			vertices.Add(new Vector3(0, y + dy, z));
			vertices.Add(new Vector3(0, y + dy, z + dz));

			if (collisionMesh == false)
			{
				if (leftSide)
				{
					normals.Add(Vector3.left);
					normals.Add(Vector3.left);
					normals.Add(Vector3.left);
					normals.Add(Vector3.left);
				}
				else
				{
					normals.Add(Vector3.right);
					normals.Add(Vector3.right);
					normals.Add(Vector3.right);
					normals.Add(Vector3.right);
				}

				uvs.Add(new Vector2(1 - z, y));
				uvs.Add(new Vector2(1 - z, y + dy));
				uvs.Add(new Vector2(1 - z, y + dy));
				uvs.Add(new Vector2(1 - z - dz, y + dy));

				colors.Add(color);
				colors.Add(color);
				colors.Add(color);
				colors.Add(color);
			}
			y += dy;
			z += dz;
		}

		vertices.Add(new Vector3(0, 0, 1));
		if (collisionMesh == false)
		{
			if (leftSide)
			{
				normals.Add(Vector3.left);
			}
			else
			{
				normals.Add(Vector3.right);
			}
			uvs.Add(Vector2.zero);
			colors.Add(color);
		}

		int lastVertex = vertices.Count - 1;
		for (int i = 0; i < stairs; i++)
		{
			if (leftSide)
			{
				indices.Add(lastVertex);
				indices.Add(i * 4 + 1);
				indices.Add(i * 4);
				indices.Add(lastVertex);
				indices.Add(i * 4 + 3);
				indices.Add(i * 4 + 2);
			}
			else
			{
				indices.Add(lastVertex);
				indices.Add(i * 4);
				indices.Add(i * 4 + 1);
				indices.Add(lastVertex);
				indices.Add(i * 4 + 2);
				indices.Add(i * 4 + 3);
			}
		}

		return meshBuilder;
	}

	private static MeshBuilder CreateStairs(int stairs, bool collisionMesh)
	{
		var meshBuilder = new MeshBuilder(collisionMesh);
		var vertices = meshBuilder.vertices;
		var indices = meshBuilder.indices;
		var normals = meshBuilder.normals;
		var uvs = meshBuilder.uvs;
		var colors = meshBuilder.colors;

		float y = 0;
		float dy = 1f / stairs;

		float z = 0;
		float dz = 1f / stairs;

		for (int i = 0; i < stairs; i++)
		{
			vertices.Add(new Vector3(0, y, z));
			vertices.Add(new Vector3(1, y, z));
			vertices.Add(new Vector3(0, y + dy, z));
			vertices.Add(new Vector3(1, y + dy, z));
			vertices.Add(new Vector3(0, y + dy, z));
			vertices.Add(new Vector3(1, y + dy, z));
			vertices.Add(new Vector3(0, y + dy, z + dz));
			vertices.Add(new Vector3(1, y + dy, z + dz));

			if (collisionMesh == false)
			{
				normals.Add(Vector3.back);
				normals.Add(Vector3.back);
				normals.Add(Vector3.back);
				normals.Add(Vector3.back);
				normals.Add(Vector3.up);
				normals.Add(Vector3.up);
				normals.Add(Vector3.up);
				normals.Add(Vector3.up);

				uvs.Add(new Vector2(0, y));
				uvs.Add(new Vector2(1, y));
				uvs.Add(new Vector2(0, y + dy / 2));
				uvs.Add(new Vector2(1, y + dy / 2));
				uvs.Add(new Vector2(0, y + dy / 2));
				uvs.Add(new Vector2(1, y + dy / 2));
				uvs.Add(new Vector2(0, y + dy));
				uvs.Add(new Vector2(1, y + dy));

				colors.Add(color);
				colors.Add(color);
				colors.Add(color);
				colors.Add(color);
				colors.Add(color);
				colors.Add(color);
				colors.Add(color);
				colors.Add(color);
			}

			y += dy;
			z += dz;
		}

		for (int i = 0; i < stairs; i++)
		{
			indices.Add(i * 8 + 0);
			indices.Add(i * 8 + 3);
			indices.Add(i * 8 + 1);

			indices.Add(i * 8 + 3);
			indices.Add(i * 8 + 0);
			indices.Add(i * 8 + 2);

			indices.Add(i * 8 + 4);
			indices.Add(i * 8 + 7);
			indices.Add(i * 8 + 5);

			indices.Add(i * 8 + 7);
			indices.Add(i * 8 + 4);
			indices.Add(i * 8 + 6);
		}

		return meshBuilder;
	}
}