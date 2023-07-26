using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class BallBehaviour : MonoBehaviour
{
	//[SerializeField]
	//private Game game;

	[Space(10)]
	public float speed = 1.0f;
	[SerializeField]
	private float speedMultiplier = 1.0f;
	[SerializeField]
	private float speedMultiplierStart = 2.0f;
	

	public InteractType interactType { get; private set; }
	
	[Space(10)]
	[SerializeField]
	private float jumpNormalHeight = 2.2f;
	[SerializeField]
	private float jumpDefeatHeight = 1.0f;
	[SerializeField]
	private float interactHeight = 0.5f;

	

	/*[SerializeField]
	private Vector3 gravity = Physics.gravity;
	[SerializeField]
	private float gravityScale = 1.0f;
	[Space(10)]
	[SerializeField]
	private float interactOffset = 1.0f;*/

	[Space(10)]
	[SerializeField]
	private AnimationCurve curveX = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	[SerializeField]
	private AnimationCurve curveY = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	[SerializeField]
	private AnimationCurve curveZ = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

	[Space(10)]
	[SerializeField]
	private Color colorWhite;
	[SerializeField]
	private Color colorGreen;
	[SerializeField]
	private Color colorRed;

	/*[Space(10)]
	[SerializeField]
	private DecalProjector decalProjector;*/

	//private float _velocity;

	private Animator _animator;
	private SkinnedMeshRenderer _renderer;
	private Material _material;

	private float _jumpHeight;

	//private PlatformBehaviour _prevPlatformTarget;
	private PlatformBehaviour _currentPlatformTarget;
	private Vector3 _interactStartPoint = Vector3.zero;
	private Vector3 _interactPrevPoint = Vector3.zero;
	private Vector3 _interactPoint = Vector3.zero;

	private void Awake()
	{
		_animator = GetComponentInChildren<Animator>();
		_renderer = GetComponentInChildren<SkinnedMeshRenderer>();
		_material = _renderer.material;
	}

	public void Init()
	{
		_material.SetColor("_Color", colorWhite);
		_material.SetColor("_Overlap_Color", colorWhite);
		_material.SetFloat("_Overlap_Color_Factor", 0.0f);
		SetInteractType(InteractType.White);

		_jumpHeight = jumpNormalHeight;

		//_prevPlatformTarget = null;
		//_currentPlatformTarget = null;
	}

	public void GameUpdate(float deltaTime)
	{
		if (_currentPlatformTarget == null)
		{
			return;
		}

		float t = Math.Abs((_currentPlatformTarget.transform.position.z - _interactStartPoint.z) / (_interactPoint.z - _interactStartPoint.z));
		t = Mathf.Clamp01(t);

		float x = _interactPrevPoint.x + ((_currentPlatformTarget.transform.position.x - _interactPrevPoint.x) * curveX.Evaluate(t));
		float y = interactHeight + (_jumpHeight * curveY.Evaluate(t));
		float z = _interactPrevPoint.z + ((_currentPlatformTarget.transform.position.z - _interactPrevPoint.z) * curveZ.Evaluate(t));

		transform.position = new Vector3(x, y, z);

		//decalProjector.size = new Vector3(1.0f + (2.0f * curveY.Evaluate(t)), 1.0f + (4.0f * curveY.Evaluate(t)), decalProjector.size.z);
		//decalProjector.fadeFactor = 0.16f - (0.12f * curveY.Evaluate(t));

		if (t == 1.0f)
		{
			_animator.SetFloat("Speed", 1.0f + (((speed / speedMultiplierStart) * speedMultiplier) - (1.0f * speedMultiplier)));
			_animator.SetTrigger("Interact");

			_jumpHeight = jumpNormalHeight;

			GameEvents.interactedOnPlatform?.Invoke(this, _currentPlatformTarget);
		}
	}

	public void OnDefeatInteract()
	{
		_jumpHeight = jumpDefeatHeight;
	}

	public void SetNextPlatform(PlatformBehaviour platform)
	{
		_interactPrevPoint = _currentPlatformTarget != null ? _currentPlatformTarget.transform.position : platform.transform.position;
		_currentPlatformTarget = platform;
		_interactStartPoint = _currentPlatformTarget.transform.position;
	}
	public void SetInteractPoint(Vector3 point)
	{
		_interactPoint = point;
	}

	public void SetInteractType(InteractType interactType)
	{
		if (interactType != this.interactType)
		{
			this.interactType = interactType;

			Color colorResult = colorWhite;
			switch (interactType)
			{
				case InteractType.White:
					colorResult = colorWhite;
					//_material.DOColor(colorWhite, 0.1f);
					break;
				case InteractType.Green:
					colorResult = colorGreen;
					//_material.DOColor(colorGreen, 0.1f);
					break;
				case InteractType.Red:
					colorResult = colorRed;
					//_material.DOColor(colorRed, 0.1f);
					break;
				default:
					break;
			}

			_material.SetColor("_Overlap_Color", colorResult);
			_animator.SetTrigger("SwitchColor");

			/*_material.SetColor("_Overlap_Color", colorResult);
			DOTween.To(() => _material.GetFloat("_Overlap_Color_Factor"), x => _material.SetFloat("_Overlap_Color_Factor", x), 1.0f, 0.1f)
				.OnComplete(() =>
				{
					_material.SetColor("_Color", colorResult);
					_material.SetFloat("_Overlap_Color_Factor", 0.0f);
				});*/
		}
	}

}