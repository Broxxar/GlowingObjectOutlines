using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

/// <summary>
/// Creates and maintains a command buffer to set up the textures used in the glowing object image effect.
/// </summary>
public class GlowController : MonoBehaviour
{
	private static GlowController _instance;

	private CommandBuffer _commandBuffer;

	private List<GlowObjectCmd> _glowableObjects = new List<GlowObjectCmd>();
	private Material _glowMat;
	private Material _blurMaterial;
	private Vector2 _blurTexelSize;

	private int _prePassRenderTexID;
	private int _blurPassRenderTexID;
	private int _tempRenderTexID;
	private int _blurSizeID;
	private int _glowColorID;

	/// <summary>
	/// On Awake, we cache various values and setup our command buffer to be called Before Image Effects.
	/// </summary>
	private void Awake()
	{
		_instance = this;

		_glowMat = new Material(Shader.Find("Hidden/GlowCmdShader"));
		_blurMaterial = new Material(Shader.Find("Hidden/Blur"));

		_prePassRenderTexID = Shader.PropertyToID("_GlowPrePassTex");
		_blurPassRenderTexID = Shader.PropertyToID("_GlowBlurredTex");
		_tempRenderTexID = Shader.PropertyToID("_TempTex0");
		_blurSizeID = Shader.PropertyToID("_BlurSize");
		_glowColorID = Shader.PropertyToID("_GlowColor");

		_commandBuffer = new CommandBuffer();
		_commandBuffer.name = "Glowing Objects Buffer"; // This name is visible in the Frame Debugger, so make it a descriptive!
		GetComponent<Camera>().AddCommandBuffer(CameraEvent.BeforeImageEffects, _commandBuffer);
	}

	/// <summary>
	/// TODO: Add a degister method.
	/// </summary>
	public static void RegisterObject(GlowObjectCmd glowObj)
	{
		if (_instance != null)
		{
			_instance._glowableObjects.Add(glowObj);
		}
	}

	/// <summary>
	/// Adds all the commands, in order, we want our command buffer to execute.
	/// Similar to calling sequential rendering methods insde of OnRenderImage().
	/// </summary>
	private void RebuildCommandBuffer()
	{
		_commandBuffer.Clear();

		_commandBuffer.GetTemporaryRT(_prePassRenderTexID, Screen.width, Screen.height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, QualitySettings.antiAliasing);
		_commandBuffer.SetRenderTarget(_prePassRenderTexID);
		_commandBuffer.ClearRenderTarget(true, true, Color.clear);

		print(string.Format("glowable obj count: {0}", _glowableObjects.Count));
		for (int i = 0; i < _glowableObjects.Count; i++)
		{
			_commandBuffer.SetGlobalColor(_glowColorID, _glowableObjects[i].CurrentColor);

			for (int j = 0; j < _glowableObjects[i].Renderers.Length; j++)
			{
				print(string.Format("{0} length: {1}", _glowableObjects[i].name, _glowableObjects[i].Renderers.Length));
				_commandBuffer.DrawRenderer(_glowableObjects[i].Renderers[j], _glowMat);
			}
		}

		_commandBuffer.GetTemporaryRT(_blurPassRenderTexID, Screen.width >> 1, Screen.height >> 1, 0, FilterMode.Bilinear);
		_commandBuffer.GetTemporaryRT(_tempRenderTexID, Screen.width >> 1, Screen.height >> 1, 0, FilterMode.Bilinear);
		_commandBuffer.Blit(_prePassRenderTexID, _blurPassRenderTexID);

		_blurTexelSize = new Vector2(1.5f / (Screen.width >> 1), 1.5f / (Screen.height >> 1));
		_commandBuffer.SetGlobalVector(_blurSizeID, _blurTexelSize);

		for (int i = 0; i < 4; i++)
		{
			_commandBuffer.Blit(_blurPassRenderTexID, _tempRenderTexID, _blurMaterial, 0);
			_commandBuffer.Blit(_tempRenderTexID, _blurPassRenderTexID, _blurMaterial, 1);
		}
	}

	/// <summary>
	/// Rebuild the Command Buffer each frame to account for changes in color.
	/// This could be improved to only rebuild when necessary when colors are changing.
	/// 
	/// Could be further optimized to not include objects which are currently black and not
	/// affect thing the glow image.
	/// </summary>
	private void Update()
	{
		RebuildCommandBuffer();
	}
}
