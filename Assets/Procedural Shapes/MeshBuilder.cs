using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class MeshBuilder
{
	private const int INITIAL_CAPACITY = 1000;

	public readonly List<Vector3> vertices;
	public readonly List<int> indices;
	public readonly List<Vector3> normals;
	public readonly List<Color32> colors;
	public readonly List<Vector2> uvs;
	public readonly bool collisionMesh;

	public MeshBuilder(bool collisionMesh)
	{
		this.collisionMesh = collisionMesh;

		vertices = new List<Vector3>(INITIAL_CAPACITY);
		indices = new List<int>(INITIAL_CAPACITY);

		if (collisionMesh == false)
		{
			normals = new List<Vector3>(INITIAL_CAPACITY);
			colors = new List<Color32>(INITIAL_CAPACITY);
			uvs = new List<Vector2>(INITIAL_CAPACITY);
		}
	}

	public MeshBuilder(Mesh mesh, bool collisionMesh)
	{
		vertices = new List<Vector3>(mesh.vertices);
		indices = new List<int>(mesh.triangles);

		this.collisionMesh = collisionMesh;
		if (collisionMesh == false)
		{
			normals = new List<Vector3>(mesh.normals);
			colors = new List<Color32>(mesh.colors32);
			uvs = new List<Vector2>(mesh.uv);
		}
	}

	public MeshBuilder(Mesh mesh, int submeshIndex, bool collisionMesh)
	{
		vertices = new List<Vector3>(mesh.vertices);
		indices = new List<int>(mesh.GetIndices(submeshIndex));

		this.collisionMesh = collisionMesh;
		if (collisionMesh == false)
		{
			normals = new List<Vector3>(mesh.normals);
			colors = new List<Color32>(mesh.colors32);
			uvs = new List<Vector2>(mesh.uv);
		}

		var map = new int[vertices.Count];
		int index = 0;
		int vertexCount = vertices.Count;

		for (int i = 0; i < indices.Count; i++)
		{
			map[indices[i]] = -1; // mark indices as used
		}
		for (int i = 0; i < vertexCount; i++)
		{
			if (map[i] == -1)
			{
				map[i] = index; // map used indices
				index++;
			}
			else
			{
				map[i] = -2; // mark other indices redundant
			}
		}
		for (int i = vertexCount - 1; i >= 0; i--)
		{
			if (map[i] == -2)
			{
				vertices.RemoveAt(i);
				if (collisionMesh == false)
				{
					normals.RemoveAt(i);
					colors.RemoveAt(i);
					uvs.RemoveAt(i);
				}
			}
		}
		for (int i = 0; i < indices.Count; i++)
		{
			indices[i] = map[indices[i]];
		}
	}

	public bool IsEmpty()
	{
		return vertices.Count == 0;
	}

	public void Clear()
	{
		vertices.Clear();
		indices.Clear();

		if (collisionMesh == false)
		{
			normals.Clear();
			colors.Clear();
			uvs.Clear();
		}
	}

	public static Mesh ToSubmeshes(IList<MeshBuilder> meshBuilders, Mesh mesh = null)
	{
		var vertices = new List<Vector3>();
		var uvs = new List<Vector2>();
		var normals = new List<Vector3>();
		var colors = new List<Color32>();
		var tangents = new List<Vector4>();

		foreach (var meshBuilder in meshBuilders)
		{
			vertices.AddRange(meshBuilder.vertices);
			uvs.AddRange(meshBuilder.uvs);
			normals.AddRange(meshBuilder.normals);
			colors.AddRange(meshBuilder.colors);
			tangents.AddRange(meshBuilder.CalculateTangents());
		}

		if (mesh != null)
		{
			mesh.Clear(true);
		}
		else
		{
			mesh = new Mesh();
		}

		mesh.vertices = vertices.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.normals = normals.ToArray();
		if (colors.Count == vertices.Count)
		{
			mesh.colors32 = colors.ToArray();
		}
		mesh.tangents = tangents.ToArray();
		mesh.subMeshCount = meshBuilders.Count;

		int vertexShift = 0;
		for (int submeshIndex = 0; submeshIndex < meshBuilders.Count; submeshIndex++)
		{
			var meshBuilder = meshBuilders[submeshIndex];

			// ReSharper disable once AccessToModifiedClosure
			var triangles = meshBuilder.indices.Select(t => t + vertexShift).ToArray();


			mesh.SetTriangles(triangles, submeshIndex);

			vertexShift += meshBuilder.vertices.Count;
		}

		return mesh;
	}

	public static MeshBuilder Combine(params MeshBuilder[] meshBuilders)
	{
		return Combine(meshBuilders as IList<MeshBuilder>);
	}

	public static MeshBuilder Combine(IList<MeshBuilder> meshBuilders)
	{
		var collisionMesh = meshBuilders.Any(mb => mb.collisionMesh);
		var result = new MeshBuilder(collisionMesh);

		foreach (var meshBuilder in meshBuilders)
		{
			result.vertices.AddRange(meshBuilder.vertices);

			if (collisionMesh == false)
			{
				result.uvs.AddRange(meshBuilder.uvs);
				result.normals.AddRange(meshBuilder.normals);
				result.colors.AddRange(meshBuilder.colors);
			}
		}

		int vertexShift = 0;
		foreach (var meshBuilder in meshBuilders)
		{
			int shift = vertexShift;
			var triangles = meshBuilder.indices.Select(t => t + shift);
			result.indices.AddRange(triangles);

			vertexShift += meshBuilder.vertices.Count;
		}

		return result;
	}

	public static Vector4[] CalculateTangents(IList<Vector3> vertices, IList<Vector3> normals, IList<Vector2> uvs, IList<int> indices)
	{
		int triangleCount = indices.Count;
		int vertexCount = vertices.Count;

		var tan1 = new Vector3[vertexCount];
		var tan2 = new Vector3[vertexCount];

		var tangents = new Vector4[vertexCount];

		for (int a = 0; a < triangleCount; a += 3)
		{
			int i1 = indices[a + 0];
			int i2 = indices[a + 1];
			int i3 = indices[a + 2];

			var p1 = vertices[i1];
			var p2 = vertices[i2];
			var p3 = vertices[i3];

			var uv1 = uvs[i1];
			var uv2 = uvs[i2];
			var uv3 = uvs[i3];

			float x1 = p2.x - p1.x;
			float y1 = p2.y - p1.y;
			float z1 = p2.z - p1.z;

			float x2 = p3.x - p1.x;
			float y2 = p3.y - p1.y;
			float z2 = p3.z - p1.z;

			float u1 = uv2.x - uv1.x;
			float v1 = uv2.y - uv1.y;

			float u2 = uv3.x - uv1.x;
			float v2 = uv3.y - uv1.y;

			float r = 1.0f / (u1 * v2 - u2 * v1);

			var sdir = new Vector3((v2 * x1 - v1 * x2) * r, (v2 * y1 - v1 * y2) * r, (v2 * z1 - v1 * z2) * r);
			var tdir = new Vector3((u1 * x2 - u2 * x1) * r, (u1 * y2 - u2 * y1) * r, (u1 * z2 - u2 * z1) * r);

			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;

			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;
		}

		for (int a = 0; a < vertexCount; a++)
		{
			var n = normals[a];
			var t = tan1[a];

			var tmp = (t - n * Vector3.Dot(n, t)).normalized;
			float w = Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f ? -1.0f : 1.0f;
			tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z, w);
		}

		return tangents;
	}

	public Mesh ToMesh(Mesh mesh = null)
	{
		if (mesh != null)
		{
			mesh.Clear();
		}
		else
		{
			mesh = new Mesh();
		}

		mesh.vertices = vertices.ToArray();
		mesh.triangles = indices.ToArray();

		if (collisionMesh == false)
		{
			mesh.normals = normals.ToArray();
			mesh.uv = uvs.ToArray();
			mesh.colors32 = colors.ToArray();
			mesh.tangents = CalculateTangents();
		}

		return mesh;
	}

	public void Transform(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		for (int i = 0; i < vertices.Count; i++)
		{
			var pos = vertices[i];
			pos.Scale(scale);
			vertices[i] = position + rotation * pos;
		}

		if (collisionMesh == false)
		{
			for (int i = 0; i < normals.Count; i++)
			{
				normals[i] = rotation * normals[i];
			}
		}
	}

	public void Transform(Vector3 position, Quaternion rotation)
	{
		Transform(position, rotation, Vector3.one);
	}

	public int Append(MeshBuilder meshBuilder, Vector3 position, Quaternion rotation, float scale)
	{
		if (collisionMesh != meshBuilder.collisionMesh)
		{
			throw new Exception("collisionMesh flags are not equal");
		}

		int result;
		if (collisionMesh)
		{
			result = Append(position, rotation, scale, meshBuilder.vertices, meshBuilder.indices);
		}
		else
		{
			result = Append(position, rotation, scale, meshBuilder.vertices, meshBuilder.normals, meshBuilder.uvs, meshBuilder.colors, meshBuilder.indices);
		}

		return result;
	}

	public int Append(MeshBuilder meshBuilder, Vector3 position)
	{
		if (collisionMesh != meshBuilder.collisionMesh)
		{
			throw new Exception("collisionMesh flags are not equal");
		}
		int result;
		if (collisionMesh)
		{
			result = Append(position, Quaternion.identity, 1, meshBuilder.vertices, meshBuilder.indices);
		}
		else
		{
			result = Append(position, Quaternion.identity, 1, meshBuilder.vertices, meshBuilder.normals, meshBuilder.uvs, meshBuilder.colors, meshBuilder.indices);
		}

		return result;
	}

	#region Private

	private Vector4[] CalculateTangents()
	{
		if (collisionMesh)
		{
			throw new Exception("Can't calculate tangents for collision mesh");
		}
		return CalculateTangents(vertices, normals, uvs, indices);
	}

	private int Append(
		Vector3 position,
		Quaternion rotation,
		float scale,
		IList<Vector3> extraVertices,
		IList<Vector3> extraNormals,
		IList<Vector2> extraUvs,
		IList<Color32> extraColors,
		IList<int> extraTriangles)
	{
		int v = vertices.Count;

		foreach (var vertex in extraVertices)
		{
			vertices.Add(position + rotation * vertex * scale);
		}

		colors.AddRange(extraColors);

		foreach (var normal in extraNormals)
		{
			normals.Add(rotation * normal);
		}

		uvs.AddRange(extraUvs);

		foreach (int triangle in extraTriangles)
		{
			indices.Add(v + triangle);
		}

		return v;
	}

	private int Append(
		Vector3 position,
		Quaternion rotation,
		float scale,
		IList<Vector3> extraVertices,
		IList<int> extraTriangles)
	{
		int v = vertices.Count;

		foreach (var vertex in extraVertices)
		{
			vertices.Add(position + rotation * vertex * scale);
		}

		foreach (int triangle in extraTriangles)
		{
			indices.Add(v + triangle);
		}

		return v;
	}

	private int Append(
		Vector3 position,
		IList<Vector3> extraVertices,
		IList<Vector3> extraNormals,
		IList<Vector2> extraUvs,
		IList<Color32> extraColors,
		IList<int> extraTriangles)
	{
		return Append(position, Quaternion.identity, 1f, extraVertices, extraNormals, extraUvs, extraColors, extraTriangles);
	}

	private int Append(
		Vector3 position,
		IList<Vector3> extraVertices,
		IList<int> extraTriangles)
	{
		return Append(position, Quaternion.identity, 1f, extraVertices, extraTriangles);
	}

	#endregion
}