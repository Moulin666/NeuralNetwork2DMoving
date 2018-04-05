using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	#region Public variables

	public GameObject BoomerangObject;
	public GameObject Hexagon;

	#endregion

	#region Private variables

	private bool isTraning = false;

	private int populationSize = 50;
	public int generationNumber = 0;
	private int[] layers = new int[] { 1, 10, 10, 1 };

	private List<RNetwork> rNetworks;
	private List<Boomerang> boomerangList = null;

	private bool leftMouseDown = false;

	#endregion

	#region Unity methods

	private void Update()
	{
		if (isTraning == false)
		{
			if (generationNumber == 0)
			{
				InitBoomerangNeuralNetworks();
			}
			else
			{
				rNetworks.Sort();

				for (int i = 0; i < populationSize / 2; i++)
				{
					rNetworks[i] = new RNetwork(rNetworks[i + (populationSize / 2)]);
					rNetworks[i].Mutate();

					rNetworks[i + (populationSize / 2)] = new RNetwork(rNetworks[i + (populationSize / 2)]);
				}

				for (int i = 0; i < populationSize; i++)
				{
					rNetworks[i].SetFitness(0f);
				}
			}

			generationNumber++;
			isTraning = true;

			Invoke("Timer", 15f);

			CreateBoomerangBodies();
		}

		if (Input.GetMouseButtonDown(0))
		{
			leftMouseDown = true;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			leftMouseDown = false;
		}

		if (leftMouseDown == true)
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Hexagon.transform.position = mousePosition;
		}
	}

	#endregion

	#region Private methods

	private void Timer()
	{
		isTraning = false;
	}

	private void CreateBoomerangBodies()
	{
		if (boomerangList != null)
		{
			for (int i = 0; i < boomerangList.Count; i++)
			{
				Destroy(boomerangList[i].gameObject);
			}
		}

		boomerangList = new List<Boomerang>();

		for (int i = 0; i < populationSize; i++)
		{
			Boomerang boomerang = Instantiate(BoomerangObject,
				new Vector3(Random.Range(-10f, 10f),
				Random.Range(-10f, 10f), 0),
				BoomerangObject.transform.rotation).GetComponent<Boomerang>();

			boomerang.Init(rNetworks[i], Hexagon.transform);

			boomerangList.Add(boomerang);
		}
	}

	private void InitBoomerangNeuralNetworks()
	{
		if (populationSize % 2 != 0)
		{
			populationSize = 20;
		}

		rNetworks = new List<RNetwork>();

		for (int i = 0; i < populationSize; i++)
		{
			RNetwork rNetwork = new RNetwork(layers);
			rNetwork.Mutate();

			rNetworks.Add(rNetwork);
		}
	}

	#endregion
}
