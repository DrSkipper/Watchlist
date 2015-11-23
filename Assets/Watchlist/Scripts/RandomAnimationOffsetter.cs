using UnityEngine;

public class RandomAnimationOffsetter : MonoBehaviour
{
	void Awake()
    {
        this.GetComponent<Animator>().SetFloat("Offset", Random.Range(0.0f, 1.0f));
    }
}
