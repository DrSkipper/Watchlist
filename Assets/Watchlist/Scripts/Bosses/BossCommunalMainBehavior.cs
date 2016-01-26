using UnityEngine;
using System.Collections.Generic;

public class BossCommunalMainBehavior : VoBehavior
{
    public int[] PossibleRotationDirections = { 1, -1 };
    public float RotationSpeed = 180.0f;
    public LayerMask LevelGeomLayerMask;
    public List<GameObject> SubBosses;
    public GameObject EndFlowObject;

    void Start()
    {
        _rotationAxis = new Vector3(0, 0, 1);
        _rotationDirection = this.PossibleRotationDirections[Random.Range(0, this.PossibleRotationDirections.Length)];

        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            GameObject subBoss = this.SubBosses[i];
            subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
        }
    }

    void Update()
    {
        float additionalAngle = this.RotationSpeed * Time.deltaTime;
        _currentAngle = (_currentAngle + additionalAngle * _rotationDirection) % 360.0f;
        this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, _rotationAxis);
    }

    /**
     * Private
     */
    private int _rotationDirection;
    private Vector3 _rotationAxis;
    private float _currentAngle;

    private void SubBossKilled(Damagable died)
    {
        this.SubBosses.Remove(died.gameObject);

        if (this.SubBosses.Count == 0)
        {
            this.EndFlowObject.GetComponent<WinCondition>().EndLevel();
        }
    }
}
