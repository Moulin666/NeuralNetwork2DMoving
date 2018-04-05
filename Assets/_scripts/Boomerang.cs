using UnityEngine;

public class Boomerang : MonoBehaviour
{
	#region Variables

	private bool initilized = false;
	private Transform hexagon;

	private RNetwork rNetwork;
	private Rigidbody2D rigidbody;
	private Material[] materials;

	#endregion

	#region Unity Methods

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		materials = new Material[transform.childCount];

		for (int i = 0; i < materials.Length; i++)
			materials[i] = transform.GetChild(i).GetComponent<Renderer>().material;
	}

	private void FixedUpdate()
	{
		if (initilized == true)
		{
			float distance = Vector2.Distance(transform.position, hexagon.position);
			float angle = transform.eulerAngles.z % 360f;
			float[] inputs = new float[1];

			Vector2 deltaVector = (hexagon.position - transform.position).normalized;
			float rad = Mathf.Atan2(deltaVector.y, deltaVector.x);

			if (distance > 20f)
				distance = 20f;

			for (int i = 0; i < materials.Length; i++)
				materials[i].color = new Color(distance / 20f, (1f - (distance / 20f)), (1f - (distance / 20f)));

			if (angle < 0f)
				angle += 360f;

			rad *= Mathf.Rad2Deg;
			rad = rad % 360;

			if (rad < 0)
			{
				rad = 360 + rad;
			}

			rad = 90f - rad;

			if (rad < 0f)
			{
				rad += 360f;
			}

			rad = 360 - rad;
			rad -= angle;

			if (rad < 0)
				rad = 360 + rad;

			if (rad >= 180f)
			{
				rad = 360 - rad;
				rad *= -1f;
			}

			rad *= Mathf.Deg2Rad;

			inputs[0] = rad / (Mathf.PI);

			float[] output = rNetwork.FeedForward(inputs);

			rigidbody.velocity = 2.5f * transform.up;
			rigidbody.angularVelocity = 500f * output[0];

			rNetwork.AddFitness((1f - Mathf.Abs(inputs[0])));
		}
	}

	#endregion

	public void Init(RNetwork rNetwork, Transform hexagon)
	{
		this.hexagon = hexagon;
		this.rNetwork = rNetwork;

		initilized = true;
	}
}
