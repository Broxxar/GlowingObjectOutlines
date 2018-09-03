using UnityEngine;
using System.Collections.Generic;

public class GlowObject : MonoBehaviour {
	
	public Color GlowColor = Color.white;
	public float LerpFactor = 10;

	List<Material> _materials = new List<Material>();
	Color _currentColor;
	Color _targetColor;

	void Start(){ //Grab materials in self and children to make sure they're all affected by the glow
		var Renderers = GetComponentsInChildren<Renderer>();

		for (int i = 0; i < Renderers.Length; i++){
			_materials.AddRange(Renderers[i].materials);
		}
	}

	void OnMouseEnter(){
		_targetColor = GlowColor;
		enabled = true;
	}

	void OnMouseExit(){
		_targetColor = Color.black;
		enabled = true;
	}

	void Update(){ //Loop over all cached materials and update their color, disable self if we reach our target color.
		_currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

		for (int i = 0; i < _materials.Count; i++){
			_materials[i].SetColor("_GlowColor", _currentColor); //Set shader-specific variable (not material color)
		}

		if (_currentColor == _targetColor){
			enabled = false;
		}
	}
}
