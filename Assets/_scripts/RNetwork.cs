using System;
using System.Collections.Generic;

public class RNetwork : IComparable<RNetwork>
{
	#region Variables

	private int[] _layers;

	private float[][] _neurons;
	private float[][][] _weights;
	private float _fitness;

	#endregion

	#region Constructors

	public RNetwork(int[] layers)
	{
		_layers = new int[layers.Length];

		for (int i = 0; i < layers.Length; i++)
			_layers[i] = layers[i];

		Initialize();
	}

	public RNetwork(RNetwork rNetworkCopy)
	{
		_layers = new int[rNetworkCopy._layers.Length];

		for (int i = 0; i < rNetworkCopy._layers.Length; i++)
			_layers[i] = rNetworkCopy._layers[i];

		Initialize();

		CopyWeights(rNetworkCopy._weights);
	}

	#endregion

	#region Public methods

	public float[] FeedForward(float[] inputs)
	{
		for (int i = 0; i < inputs.Length; i++)
			_neurons[0][i] = inputs[i];

		for (int i = 1; i < _layers.Length; i++)
			for (int j = 0; j < _neurons[i].Length; j++)
			{
				float value = 0.25f;

				for (int k = 0; k < _neurons[i - 1].Length; k++)
					value += _weights[i - 1][j][k] * _neurons[i - 1][k];

				_neurons[i][j] = (float)Math.Tanh(value);
			}

		return _neurons[_neurons.Length - 1];
	}

	public void Mutate()
	{
		for (int i = 0; i < _weights.Length; i++)
			for (int j = 0; j < _weights[i].Length; j++)
				for (int k = 0; k < _weights[i][j].Length; k++)
					_weights[i][j][k] = MutateWeight(_weights[i][j][k]);
	}

	public int CompareTo(RNetwork other)
	{
		if (other == null)
			return 1;
		else if (_fitness > other._fitness)
			return 1;
		else if (_fitness < other._fitness)
			return -1;
		else
			return 0;
	}

	public void AddFitness(float value) => _fitness += value;

    public void SetFitness(float value) => _fitness = value;

    public float GetFitness() => _fitness;

    #endregion

	#region Private methods

	private void Initialize()
	{
		// Neurons initialize.
		List<float[]> neuronList = new List<float[]>();

		for (int i = 0; i < _layers.Length; i++)
			neuronList.Add(new float[_layers[i]]);

		_neurons = neuronList.ToArray();

		// Weights init initialize.
		List<float[][]> weightList = new List<float[][]>();

		for (int i = 1; i < _layers.Length; i++)
		{
			List<float[]> layerWeightList = new List<float[]>();
			int neuronsPriviousLayer = _layers[i - 1];

			for (int j = 0; j < _neurons[i].Length; j++)
			{
				float[] neuronWeight = new float[neuronsPriviousLayer];

				for (int k = 0; k < neuronsPriviousLayer; k++)
					neuronWeight[k] = UnityEngine.Random.Range(-0.5f, 0.5f);

				layerWeightList.Add(neuronWeight);
			}

			weightList.Add(layerWeightList.ToArray());
		}

		_weights = weightList.ToArray();
	}

	private float MutateWeight(float weight)
	{
		float randomNumber = UnityEngine.Random.Range(0f, 100f);

		if (randomNumber <= 2f)
			weight *= -1f;
		else if (randomNumber <= 4f)
			weight = UnityEngine.Random.Range(-0.5f, 0.5f);
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
	    for (int i = 0; i < _weights.Length; i++)
	        for (int j = 0; j < _weights[i].Length; j++)
	            for (int k = 0; k < _weights[i][j].Length; k++)
	                _weights[i][j][k] = copyWeights[i][j][k];
	}

	#endregion
}
