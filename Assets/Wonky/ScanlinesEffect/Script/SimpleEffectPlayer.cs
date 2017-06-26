using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEffectPlayer : MonoBehaviour {

	public GameObject mainCam;
	private bool sceneSwitch = true;
	private bool effectReady = true;

	void Start(){
		Application.LoadLevelAdditive("SampleScene1");
	}

	public void EffectPlay(){
		if(effectReady) {
			StartCoroutine(SwitchTime());
		}
	}

	IEnumerator SwitchTime(){
		effectReady = false;
		mainCam.GetComponent<ImageEffectScanlines>().EffectPlay();
		yield return new WaitForSeconds(1.4f);
		if(sceneSwitch) {
			sceneSwitch = false;
			SwitchScene("SampleScene1", "SampleScene2");
		} else {
			sceneSwitch = true;
			SwitchScene("SampleScene2", "SampleScene1");
		}
		yield return new WaitForSeconds(2.0f);
		effectReady = true;
	}

	public void SwitchScene(string sc1, string sc2){
		Application.UnloadLevel(sc1);
		Application.LoadLevelAdditive(sc2);
	}
}
