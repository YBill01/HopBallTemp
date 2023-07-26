using DynamicShadowProjector;
using UnityEngine;

public class BonusBehaviour : MonoBehaviour
{

	[SerializeField]
	private GameObject apple;
	[SerializeField]
	private GameObject cake;

	[SerializeField]
	private DrawTargetObject decalProjector;

	[SerializeField]
	private float emission;



	public BonusType bonusType { get; private set; }

	private Animator _animator;

	private MeshRenderer[] _appleMeshRenderer;
	private MeshRenderer[] _cakeMeshRenderer;

	private void Awake()
	{
		_animator = GetComponent<Animator>();

		_appleMeshRenderer = apple.GetComponentsInChildren<MeshRenderer>();
		_cakeMeshRenderer = cake.GetComponentsInChildren<MeshRenderer>();
	}

	public void Init()
	{
		_animator.Rebind();
		//_animator.Update(0.0f);
		gameObject.SetActive(false);
		bonusType = BonusType.None;
		apple.SetActive(false);
		cake.SetActive(false);
		//decalProjector.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (gameObject.activeSelf)
		{
			if (apple.activeSelf)
			{
				foreach (MeshRenderer renderer in _appleMeshRenderer)
				{
					foreach (Material material in renderer.materials)
					{
						material.SetFloat("_Emission_Intensity", emission);
					}
				}
			}
			if (cake.activeSelf)
			{
				foreach (MeshRenderer renderer in _cakeMeshRenderer)
				{
					foreach (Material material in renderer.materials)
					{
						material.SetFloat("_Emission_Intensity", emission);
					}
				}
			}
		}
	}

	public void Show()
	{
		if (bonusType != BonusType.None)
		{
			_animator.SetTrigger("Show");
			if (apple.activeSelf)
			{

			}
		}

		
	}
	public void Hide()
	{
		/*if (bonusType != BonusType.None)
		{
			_animator.SetTrigger("Hide");
		}*/
		if (gameObject.activeSelf)
		{
			//decalProjector.gameObject.SetActive(false);
			_animator.SetTrigger("Hide");
		}
		
	}
	public void Defeat()
	{
		if (bonusType != BonusType.None)
		{
			_animator.SetTrigger("Defeat");
		}


	}
	public void Collect()
	{
		if (bonusType != BonusType.None)
		{
			_animator.SetTrigger("Collect");
		}


	}

	public void SetBonus(BonusType bonusType)
	{
		this.bonusType = bonusType;

		switch (bonusType)
		{
			case BonusType.None:
				Hide();

				break;
			case BonusType.Apple:
				gameObject.SetActive(true);
				apple.SetActive(true);
				cake.SetActive(false);
				//decalProjector.gameObject.SetActive(true);

				Show();

				break;
			case BonusType.Cake:
				gameObject.SetActive(true);
				apple.SetActive(false);
				cake.SetActive(true);
				//decalProjector.gameObject.SetActive(true);

				Show();

				break;
			default:
				break;
		}

		decalProjector.SetCommandBufferDirty();
	}
}