using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LevelGenManager : LevelGenBehavior
{
    public delegate void LevelGenerationUpdateDelegate();

	public float StepRunInterval = 0.0f;
	public int StepsRunEachUpdate = int.MaxValue;
	public bool Finished { get { return !_generating; } }
    public LevelGenerationUpdateDelegate UpdateDelegate = null;
    public bool RemoveTips = true;

	public void InitiateGeneration(Object storyOutput)
	{
		_generating = true;
		_timeSinceLastStep = 0.0f;

		//TODO - Create generator based on LevelGenParams
        _generator = this.gameObject.AddComponent<CAGenerator>();
        this.Map.FillCompletely(LevelGenMap.TileType.A);
		_generator.SetupGeneration();

        if (this.UpdateDelegate != null)
            this.UpdateDelegate();
	}

	public void TestGenerator(BaseLevelGenerator generator)
	{
		_generating = true;
		_timeSinceLastStep = 0.0f;

		_generatorRemainsWhenDone = true;
		_generator = generator;
		this.Map.FillCompletely(LevelGenMap.TileType.A);
		_generator.SetupGeneration();

        if (this.UpdateDelegate != null)
            this.UpdateDelegate();
	}

	public void Update()
	{
		if (_generating)
		{
			_timeSinceLastStep += Time.deltaTime;
			if (_timeSinceLastStep > this.StepRunInterval)
			{
				_timeSinceLastStep = 0.0f;
				_generator.RunGenerationFrames(this.StepsRunEachUpdate);

                if (_generator.IsFinished)
                {
                    _generating = false;

                    if (this.RemoveTips)
                        removeTips();
                }

                if (this.UpdateDelegate != null)
                    this.UpdateDelegate();
			}
		}
	}

	public void HaltGeneration()
	{
		_generating = false;

		if (_generator != null)
		{
			if (!_generatorRemainsWhenDone)
				Destroy(_generator);
			_generator = null;
		}

		_generatorRemainsWhenDone = false;
	}

	public void Cleanup()
	{
		this.HaltGeneration();
	}

	public LevelGenOutput GetOutput()
	{
        return _generator.GetOutput();
	}

	/**
	 * Private
	 */
	private bool _generating;
	private bool _generatorRemainsWhenDone;
	private float _timeSinceLastStep;
	private BaseLevelGenerator _generator;

    private void removeTips()
    {

    }
}
