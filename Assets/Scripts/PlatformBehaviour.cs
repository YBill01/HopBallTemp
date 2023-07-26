using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
	/*[SerializeField]
	private Material whiteMaterial;
	[SerializeField]
	private Material greenMaterial;
	[SerializeField]
	private Material redMaterial;

	[Space(10)]
	[SerializeField]
	private Material whiteMaterialDecal;
	[SerializeField]
	private Material greenMaterialDecal;
	[SerializeField]
	private Material redMaterialDecal;*/

	[SerializeField]
	private Color colorWhite;
	[SerializeField]
	private Color colorGreen;
	[SerializeField]
	private Color colorRed;

	[Space(10)]
	public BonusBehaviour bonus;

	[Space(10)]
	[SerializeField]
	private SpriteRenderer shadowRenderer;
	[Space(10)]
	[SerializeField]
	private ParticleSystem comboParticleSystem;
	/*[Space(10)]
	[SerializeField]
	private DecalProjector decalProjectorWhite;
	[SerializeField]
	private DecalProjector decalProjectorGreen;
	[SerializeField]
	private DecalProjector decalProjectorRed;*/

	public InteractType interactType { get; private set; }
	//public BonusType bonusType { get; private set; }
	public bool isInteracted { get; private set; }

	private Animator _animator;
	private SkinnedMeshRenderer _renderer;


	private void Awake()
	{
		_animator = GetComponentInChildren<Animator>();
		_renderer = GetComponentInChildren<SkinnedMeshRenderer>();

		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		_renderer.SetPropertyBlock(materialPropertyBlock);
	}

	public void Init()
	{
		isInteracted = false;
		interactType = InteractType.White;
		bonus.Init();
	}

	public void OnInteract(bool result)
	{
		isInteracted = true;

		if (result)
		{
			bonus.Collect();
		}
		else
		{
			bonus.Defeat();
		}

		_animator.SetTrigger("Interact");
	}
	public void OnApplyComboVFX()
	{
		//comboParticleSystem.Stop();
		comboParticleSystem.Play();
	}


	public void SetInteractType(InteractType interactType)
	{
		this.interactType = interactType;

		Color colorResult = colorWhite;
		switch (interactType)
		{
			case InteractType.White:
				colorResult = colorWhite;
				
				//_renderer.material = whiteMaterial;
				//decalProjector.material = whiteMaterialDecal;
				/*decalProjectorWhite.gameObject.SetActive(true);
				decalProjectorGreen.gameObject.SetActive(false);
				decalProjectorRed.gameObject.SetActive(false);*/

				break;
			case InteractType.Green:
				colorResult = colorGreen;
				
				//_renderer.material = greenMaterial;
				//decalProjector.material = greenMaterialDecal;
				/*decalProjectorWhite.gameObject.SetActive(false);
				decalProjectorGreen.gameObject.SetActive(true);
				decalProjectorRed.gameObject.SetActive(false);*/

				break;
			case InteractType.Red:
				colorResult = colorRed;
				
				//_renderer.material = redMaterial;
				//decalProjector.material = redMaterialDecal;
				/*decalProjectorWhite.gameObject.SetActive(false);
				decalProjectorGreen.gameObject.SetActive(false);
				decalProjectorRed.gameObject.SetActive(true);*/

				break;
			default:
				break;
		}

		_renderer.material.SetColor("_Color", colorResult);
		shadowRenderer.color = colorResult * 0.25f;
		ParticleSystem.MainModule psMain = comboParticleSystem.main;
		psMain.startColor = new Color(colorResult.r, colorResult.g, colorResult.b, 0.75f);

		//decalProjector.material.SetColor("_Color", _renderer.material.GetColor("_Color"));

	}


}