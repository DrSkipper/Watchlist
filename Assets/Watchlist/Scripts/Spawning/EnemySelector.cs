using UnityEngine;
using System.Collections.Generic;

public class EnemySelector
{
    public class WeightSet
    {
        public Dictionary<int, int> WeightsByEnemyId = new Dictionary<int, int>();
    }

    public EnemySelector()
    {
        _weightSets = new List<WeightSet>();
        _standardWeightOffsets = new WeightSet();
        _baseWeights = new WeightSet();
        _aggregateWeights = new Dictionary<int, int>();

        foreach (EnemyType enemyType in StaticData.EnemyData.EnemyTypeArray)
        {
            _baseWeights.WeightsByEnemyId.Add(enemyType.Id, enemyType.ChoiceWeight);
        }

        _weightSets.Add(_baseWeights);
        _weightSets.Add(_standardWeightOffsets);
    }

    public int ChooseEnemy(int highestDifficulty = 999, bool applyWeightIncrease = true)
    {
        int totalWeights = 0;

        foreach (int enemyId in _baseWeights.WeightsByEnemyId.Keys)
        {
            if (StaticData.EnemyData.EnemyTypes[enemyId].Difficulty <= highestDifficulty)
            {
                int weight = 0;
                foreach (WeightSet weightSet in _weightSets)
                {
                    if (weightSet.WeightsByEnemyId.ContainsKey(enemyId))
                        weight += weightSet.WeightsByEnemyId[enemyId];
                }
                _aggregateWeights[enemyId] = weight;
                totalWeights += weight;
            }
        }

        return chooseEnemy(totalWeights, applyWeightIncrease);
    }

    public int ChooseEnemyOfDifficulty(int guaranteedDifficulty, bool applyWeightIncrease = true)
    {
        int totalWeights = 0;

        foreach (int enemyId in _baseWeights.WeightsByEnemyId.Keys)
        {
            if (StaticData.EnemyData.EnemyTypes[enemyId].Difficulty == guaranteedDifficulty)
            {
                int weight = 0;
                foreach (WeightSet weightSet in _weightSets)
                {
                    if (weightSet.WeightsByEnemyId.ContainsKey(enemyId))
                        weight += weightSet.WeightsByEnemyId[enemyId];
                }
                _aggregateWeights[enemyId] = weight;
                totalWeights += weight;
            }
        }

        return chooseEnemy(totalWeights, applyWeightIncrease);
    }

    public void AddWeightSet(WeightSet weightSet)
    {
        _weightSets.Add(weightSet);
    }

    public bool RemoveWeightSet(WeightSet weightSet)
    {
        return _weightSets.Remove(weightSet);
    }

    /**
     * Private
     */
    private List<WeightSet> _weightSets;
    private WeightSet _baseWeights;
    private WeightSet _standardWeightOffsets;
    private Dictionary<int, int> _aggregateWeights;

    private int chooseEnemy(int totalWeights, bool applyWeightIncrease)
    {
        int weightChoice = Random.Range(0, totalWeights);
        int enemyChoice = 0;
        int weightsSoFar = 0;

        foreach (int enemyId in _aggregateWeights.Keys)
        {
            weightsSoFar += _aggregateWeights[enemyId];

            if (weightChoice <= weightsSoFar)
            {
                enemyChoice = enemyId;
                break;
            }
        }

        if (applyWeightIncrease)
        {
            int prevWeight = _standardWeightOffsets.WeightsByEnemyId.ContainsKey(enemyChoice) ? _standardWeightOffsets.WeightsByEnemyId[enemyChoice] : 0;
            _standardWeightOffsets.WeightsByEnemyId[enemyChoice] = prevWeight + StaticData.EnemyData.EnemyTypes[enemyChoice].ChoiceWeightIncrease;
        }

        _aggregateWeights.Clear();
        return enemyChoice;
    }
}
