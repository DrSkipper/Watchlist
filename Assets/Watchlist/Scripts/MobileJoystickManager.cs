using UnityEngine;
using System.Collections;

public class MobileJoystickManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.SetActive(Application.isMobilePlatform);
	}

}
