using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ImageEffectScanlines : MonoBehaviour {

	public Material mat;
	private Animator anim;
    private int effectSlider;
	public float f = 0;

    void Start(){
        anim = this.GetComponent<Animator>();
        effectSlider = Shader.PropertyToID("_distSlider");
		mat.SetFloat(effectSlider, f);
    }

	public void EffectPlay(){
		this.GetComponent<AudioSource>().Play();
		anim.PlayInFixedTime("ScanlineAnimation");
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest){
		if(f == 0) {
			Graphics.Blit(src, dest);
			return;
		}
		mat.SetFloat(effectSlider, f); 
		Graphics.Blit(src, dest, mat);
	}

}
