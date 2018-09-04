using UnityEngine;

public class GlowObject : MonoBehaviour {

	public Color GlowColor = Color.white;
	public float LerpFactor = 9f;

	public Renderer[] Renderers	{ get; private set;	}
	public Color CurrentColor { get { return currentColor; } }

	Color currentColor;
	Color targetColor;

	void Start(){
		Renderers = GetComponentsInChildren<Renderer>();
		enabled = false; //No reason to run unless activated
	}

	void OnMouseEnter(){
		EnableGlow();
	}
	void OnMouseExit(){
		DisableGlow();
	}
	public void EnableGlow(){
		enabled = true;
		targetColor = GlowColor;
		GlowController.Inst.RegisterObject(this);
	}
	public void DisableGlow(){
		enabled = true;
		targetColor = Color.black; //Black is transparent for the glow shader
	}

	void Update(){ //Update color, disable script if it reaches target color
		currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * LerpFactor);
		if (currentColor == targetColor){
			if (targetColor == Color.black)
				GlowController.Inst.DeRegisterObject(this);
			else
				GlowController.Inst.RebuildCommandBuffer(); //Rebuild at final color update if glowing
			enabled = false;
		} else
			GlowController.Inst.RebuildCommandBuffer();
	}
}