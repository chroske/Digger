using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VisibleCollider : MonoBehaviour
{

	private Mesh SphereMesh;
	private Mesh CubeMesh;
	private Mesh CapsuleMesh;

	// Gizmoの色
	public Color color = new Color(1, 0, 0, 0.2f);

	void Awake()
	{

		// プリミティブのメッシュを取得
		SphereMesh = GetPrimitiveMesh(PrimitiveType.Sphere);
		CubeMesh = GetPrimitiveMesh(PrimitiveType.Cube);
		CapsuleMesh = GetPrimitiveMesh(PrimitiveType.Capsule);

	}

	void OnDrawGizmos()
	{

		SphereCollider sc = GetComponent<SphereCollider>();
		BoxCollider bc = GetComponent<BoxCollider>();
		CapsuleCollider cc = GetComponent<CapsuleCollider>();
		MeshCollider mc = GetComponent<MeshCollider>();

		Gizmos.color = color;

		// SphereCollider
		if (sc && sc.enabled)
		{

			Vector3 offset = transform.right * sc.center.x + transform.up * sc.center.y + transform.forward * sc.center.z;

			Vector3 scale = Vector3.one * sc.radius * 2;

			DrawMesh(SphereMesh, offset, scale);

		}

		// BoxCollider
		if (bc && bc.enabled)
		{

			Vector3 offset = transform.right * bc.center.x + transform.up * bc.center.y + transform.forward * bc.center.z;

			DrawMesh(CubeMesh, offset, bc.size);

		}

		// MeshCollider
		if (mc && mc.enabled) DrawMesh(mc.sharedMesh, Vector3.zero, transform.localScale);

		// CapsuleCollider
		if (cc && cc.enabled)
		{

			Vector3 offset = transform.right * cc.center.x + transform.up * cc.center.y + transform.forward * cc.center.z;

			Vector3 size = new Vector3(cc.radius / 0.5f, cc.height / 2, cc.radius / 0.5f);

			Quaternion dir;

			switch (cc.direction)
			{
			case 0:
				dir = Quaternion.Euler(Vector3.forward * 90);
				break;
			case 1:
				dir = Quaternion.Euler(Vector3.up * 90);
				break;
			case 2:
				dir = Quaternion.Euler(Vector3.right * 90);
				break;
			default:
				dir = Quaternion.Euler(Vector3.zero);
				break;
			}

			Gizmos.DrawMesh(CapsuleMesh, transform.position + offset, transform.rotation * dir, size);

		}


	}

	private Mesh GetPrimitiveMesh(PrimitiveType type)
	{

		GameObject gameObject = GameObject.CreatePrimitive(type);
		Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
		DestroyImmediate(gameObject);

		return mesh;

	}

	private void DrawMesh(Mesh mesh, Vector3 positionOffset, Vector3 scale)
	{

		Gizmos.DrawMesh(mesh, transform.position + positionOffset, transform.rotation, scale);

	}

}