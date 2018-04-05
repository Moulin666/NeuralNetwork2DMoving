using UnityEngine;

public class HexagonAnimator : MonoBehaviour
{
	private bool increasingSize = true;
	private Material material;

	private void Start()
	{
		material = GetComponent<Renderer>().material;
		material.color = Color.grey;
	}

	private void Update()
	{
		float delta = Time.deltaTime;
		Vector3 angles = transform.eulerAngles;

		angles.z += delta * 50f;
		transform.eulerAngles = angles;

		Vector3 localScale = transform.localScale;

		if (increasingSize == true)
		{
			localScale += new Vector3(delta, delta, 0f);

			if (localScale.x >= 2f)
			{
				increasingSize = false;
			}
		}
		else if (increasingSize == false)
		{
			localScale -= new Vector3(delta, delta, 0f);

			if (localScale.x <= 1f)
			{
				increasingSize = true;
			}
		}

		transform.localScale = localScale;
	}
}
