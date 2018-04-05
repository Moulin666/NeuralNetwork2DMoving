using System;
using System.Collections.Generic;

public class RNetwork : IComparable<RNetwork>
{
	#region Variables

	private int[] layers;

	private float[][] neurons;
	private float[][][] weights;
	private float fitness;

	#endregion

	#region Constructors

	public RNetwork(int[] layers)
	{
		this.layers = new int[layers.Length];

		for (int i = 0; i < layers.Length; i++)
		{
			this.layers[i] = layers[i];
		}

		Initialize();
	}

	public RNetwork(RNetwork rNetworkCopy)
	{
		layers = new int[rNetworkCopy.layers.Length];

		for (int i = 0; i < rNetworkCopy.layers.Length; i++)
		{
			layers[i] = rNetworkCopy.layers[i];
		}

		Initialize();

		CopyWeights(rNetworkCopy.weights);
	}

	#endregion

	#region Public methods

	public float[] FeedForward(float[] inputs)
	{
		for (int i = 0; i < inputs.Length; i++)
		{
			neurons[0][i] = inputs[i];
		}

		for (int i = 1; i < layers.Length; i++)
		{
			for (int j = 0; j < neurons[i].Length; j++)
			{
				float value = 0.25f;

				for (int k = 0; k < neurons[i - 1].Length; k++)
				{
					value += weights[i - 1][j][k] * neurons[i - 1][k];
				}

				neurons[i][j] = (float)Math.Tanh(value);
			}
		}

		return neurons[neurons.Length - 1];
	}

	public void Mutate()
	{
		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					weights[i][j][k] = MutateWeight(weights[i][j][k]);
				}
			}
		}
	}

	public int CompareTo(RNetwork other)
	{
		if (other == null)
			return 1;
		else if (fitness > other.fitness)
			return 1;
		else if (fitness < other.fitness)
			return -1;
		else
			return 0;
	}

	public void AddFitness(float value)
	{
		fitness += value;
	}

	public void SetFitness(float value)
	{
		fitness = value;
	}

	public float GetFitness()
	{
		return fitness;
	}

	#endregion

	#region Private methods

	private void Initialize()
	{
		// Neuron init
		List<float[]> neuronList = new List<float[]>();

		for (int i = 0; i < layers.Length; i++)
		{
			neuronList.Add(new float[layers[i]]);
		}

		neurons = neuronList.ToArray();

		// Weight init
		List<float[][]> weightList = new List<float[][]>();

		for (int i = 1; i < layers.Length; i++)
		{
			List<float[]> layerWeightList = new List<float[]>();
			int neuronsPriviousLayer = layers[i - 1];

			for (int j = 0; j < neurons[i].Length; j++)
			{
				float[] neuronWeight = new float[neuronsPriviousLayer];

				for (int k = 0; k < neuronsPriviousLayer; k++)
				{
					neuronWeight[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
				}

				layerWeightList.Add(neuronWeight);
			}

			weightList.Add(layerWeightList.ToArray());
		}

		weights = weightList.ToArray();
	}

	private float MutateWeight(float weight)
	{
		float randomNumber = UnityEngine.Random.Range(0f, 100f);

		if (randomNumber <= 2f)
		{
			weight *= -1f;
		}
		else if (randomNumber <= 4f)
		{
			weight = UnityEngine.Random.Range(-0.5f, 0.5f);
		}
		else if (randomNumber <= 6f)
		{
			float factor = UnityEngine.Random.Range(0, 1f) + 1f;
			weight *= factor;
		}
		else if (randomNumber <= 8f)
		{
			float factor = UnityEngine.Random.Range(0, 1f);
			weight *= factor;
		}

		return weight;
	}

	private void CopyWeights(float[][][] copyWeights)
	{
		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					weights[i][j][k] = copyWeights[i][j][k];
				}
			}
		}
	}

	#endregion
}
