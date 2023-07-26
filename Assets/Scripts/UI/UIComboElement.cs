using UnityEngine;

public class UIComboElement : MonoBehaviour
{
	[SerializeField]
	private InteractType interactType;

	[Space(10)]
	[SerializeField]
	private RectTransform line;
	[SerializeField]
	private RectTransform lineBoard;

	private bool _isComboActive;
	private float _progress;

	private Animator _animator;


	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	public void Init()
	{
		_isComboActive = false;
		_progress = 0.0f;
		ApplyProgress();
	}

	public void UpdateProgress(Score score)
	{
		if (_isComboActive)
		{
			return;
		}

		switch (interactType)
		{
			case InteractType.White:
				
				break;
			case InteractType.Green:
				_progress = score.combo.counterGreen / (float)Score.COMBO_START_COUNT;

				break;
			case InteractType.Red:
				_progress = score.combo.counterRed / (float)Score.COMBO_START_COUNT;

				break;
			default:
				break;
		}

		_progress = _progress > 1.0f ? 1.0f : _progress;

		if (_progress == 1.0f)
		{
			_isComboActive = true;

			_animator.SetTrigger("Ready");
		}

		ApplyProgress();
	}

	public void ClearProgress()
	{
		_isComboActive = false;
		_progress = 0.0f;
		_animator.SetTrigger("NoReady");

		ApplyProgress();
	}

	private void ApplyProgress()
	{

		if (_progress > 0.0f)
		{
			if (!line.gameObject.activeSelf)
			{
				line.gameObject.SetActive(true);
			}
			

			float lineWidth = lineBoard.sizeDelta.x * _progress;
			lineWidth = lineWidth > 32 ? lineWidth : 32;

			line.sizeDelta = new Vector2(lineWidth, line.sizeDelta.y);

		}
		else
		{
			if (line.gameObject.activeSelf)
			{
				line.gameObject.SetActive(false);
			}
		}

	}

}