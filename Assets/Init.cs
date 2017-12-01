using UnityEngine;

internal class Init : MonoBehaviour
{
	private void Start()
	{
		// Камера
		var camera = CreateCamera();
		var inputController = camera.gameObject.AddComponent<UserInput>();
		inputController.sensitivity = .5f;

		// Плоскость
		var plane = CreatePlane(Vector3.zero, size: 10);

		// Кубики
		var boxTexture = CreateTexture(size: 32);
		var boxMaterial = CreateDiffuseMaterial(Color.white, boxTexture);
		int radius = 5;
		for (int i = 0; i < 50; i++)
		{
			var p = radius * Random.insideUnitCircle;
			var position = new Vector3(p.x, p.y, Random.Range(-.5f, .5f));
			var box = CreateBox(position, boxMaterial);
			box.transform.parent = plane.transform;
		}

		// Источники света
		var lightSource = CreateLightSource(Color.magenta);
		var rotation = lightSource.gameObject.AddComponent<Rotation>();
		rotation.center = new Vector3(0, 0, -2);
		rotation.radius = 3;
		rotation.angularSpeed = .1f;

		lightSource = CreateLightSource(Color.cyan);
		rotation = lightSource.gameObject.AddComponent<Rotation>();
		rotation.center = new Vector3(0, 0, -2);
		rotation.radius = 3;
		rotation.angularSpeed = -.5f;

		lightSource = CreateLightSource(Color.yellow);
		rotation = lightSource.gameObject.AddComponent<Rotation>();
		rotation.center = new Vector3(0, 0, -2);
		rotation.radius = 3;
		rotation.angularSpeed = .3f;

		// Удаляем этот объект, он больше не нужен
		Destroy(gameObject);
	}

	private static Camera CreateCamera()
	{
		var cameraObject = new GameObject("Camera");
		cameraObject.transform.position = new Vector3(0, 0, -10);
		cameraObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

		var camera = cameraObject.AddComponent<Camera>();
		camera.clearFlags = CameraClearFlags.SolidColor;
		camera.backgroundColor = new Color(0, .1f, .1f);
		camera.renderingPath = RenderingPath.DeferredLighting;

		return camera;
	}

	private static Light CreateLightSource(Color color)
	{
		var lightObject = new GameObject("Light");

		var light = lightObject.AddComponent<Light>();
		light.type = LightType.Point;
		light.shadows = LightShadows.Hard;
		light.color = color;

		return light;
	}

	private static Texture2D CreateTexture(int size)
	{
		var colors = new Color32[size * size];
		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				colors[x + y * size] = new Color32((byte)(x * 4), (byte)(y * 8), (byte)((x * 4) ^ (y * 4)), 1);
			}
		}

		var texture = new Texture2D(size, size);
		texture.SetPixels32(colors);
		texture.Apply(true);

		return texture;
	}

	private static Material CreateDiffuseMaterial(Color color, Texture2D texture)
	{
		var shader = Shader.Find("Diffuse");

		var material = new Material(shader);
		material.SetColor("_Color", color);
		material.SetTexture("_MainTex", texture);

		return material;
	}

	private static GameObject CreateBox(Vector3 position, Material material)
	{
		var boxObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		boxObject.transform.position = position;

		var renderer = boxObject.GetComponent<Renderer>();
		renderer.sharedMaterial = material;

		return boxObject;
	}

	private static GameObject CreatePlane(Vector3 position, float size)
	{
		var planeObject = new GameObject("Plane");
		planeObject.transform.position = position - new Vector3(size / 2, size / 2, 0);

		var meshFilter = planeObject.AddComponent<MeshFilter>();
		var mesh = CreatePlaneMesh(size);
		meshFilter.sharedMesh = mesh;

		var texture = CreateTexture(512);
		var material = CreateDiffuseMaterial(Color.white, texture);
		var renderer = planeObject.AddComponent<MeshRenderer>();
		renderer.sharedMaterial = material;

		return planeObject;
	}

	private static Mesh CreatePlaneMesh(float size)
	{
		var vertices = new[]
						{
							new Vector3(0, 0, 0),
							new Vector3(0, size, 0),
							new Vector3(size, size, 0),
							new Vector3(size, 0, 0),
						};
		var normals = new[]
					{
						Vector3.back,
						Vector3.back,
						Vector3.back,
						Vector3.back,
					};
		var uvs = new[]
				{
					new Vector2(0, 0),
					new Vector2(0, 1),
					new Vector2(1, 1),
					new Vector2(1, 0),
				};
		var triangles = new[] { 0, 1, 2, 0, 2, 3 };

		var mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uvs;

		return mesh;
	}
}