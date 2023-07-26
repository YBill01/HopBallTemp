using DG.Tweening;
using System;
using UnityEngine;

public class Game : MonoBehaviour
{
	public static Game Instance { get; private set; }

	[SerializeField]
	private GameSpeedPattern speedPattern;

	[SerializeField]
	private float speed = 1.0f;

	[Space(10)]
	[SerializeField]
	private Camera gameCamera;

	public RoadBehaviour road;
	public BallBehaviour ball;

	public Score score;

	[Space(10)]
	[SerializeField]
	private float slowMotionTimeRatio = 0.5f;
	[SerializeField]
	private float slowMotionTimeIn;
	[SerializeField]
	private float slowMotionTimeOut;
	[SerializeField]
	private float slowMotionTimeDuration;

	[Space(10)]
	[SerializeField]
	private float comboFOVNormal = 60.0f;
	[SerializeField]
	private float comboFOVMax = 70.0f;
	[SerializeField]
	private float comboFOVTimeDuration = 10.0f;
	[SerializeField]
	private AnimationCurve comboFOVCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

	private bool _isPlayed;
	private bool _isGameOver;
	private bool _isSlowMotion;
	private bool _isInputEnabled;

	private bool _initialized;
	private bool _paused;
	private bool _pausedFading;
	private float _deltaTimeRatio;

	private float _speedTimer;

	private float _comboFOVTimer;

	private Tutorial _tutorial;
	

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			throw new System.Exception("An instance of this singleton already exists.");
		}
		Instance = this;

		score = new Score();
		score.highPoints = PlayerPrefs.GetInt("highPoints", 0);

		_tutorial = new Tutorial();
		_tutorial.enabled = PlayerPrefs.GetInt("tutorial", 0) != 1;

		//_isTutorial = true;// TODO remote this...
		//_tutorialProgress = 0;
	}

	private void Start()
	{
		


	}

	private void OnEnable()
	{
		GameEvents.roadReadyToPlay += OnRoadReadyToPlay;
		GameEvents.interactedOnPlatform += OnInteractedOnPlatform;
		GameEvents.inputSwipe += OnInputSwipe;
	}
	private void OnDisable()
	{
		GameEvents.roadReadyToPlay -= OnRoadReadyToPlay;
		GameEvents.interactedOnPlatform -= OnInteractedOnPlatform;
		GameEvents.inputSwipe -= OnInputSwipe;
	}

	public void Init()
	{
		if (!_tutorial.enabled)
		{
			if (_tutorial.loserCount >= 3)
			{
				_tutorial.enabled = true;
				_tutorial.loserCount = 0;
			}
		}

		_isPlayed = false;
		_initialized = true;
		_paused = false;
		_deltaTimeRatio = 1.0f;
		_speedTimer = 0.0f;
		_comboFOVTimer = 0.0f;

		score.Init();

		road.spawnPattern = new SpawnRandomPattern();
		road.Init();
		ball.SetInteractPoint(road.GetInteractPoint());
		ball.Init();



		GameEvents.init?.Invoke();
	}

	private void Update()
	{
		if (!_initialized || !_isPlayed)
		{
			return;
		}

		float deltaTime = Time.deltaTime * _deltaTimeRatio;

		if (_tutorial.enabled)
		{
			_tutorial.UpdateProgress(deltaTime);
		}

		_speedTimer += deltaTime;
		speed = speedPattern.speedStart + ((speedPattern.speedEnd - speedPattern.speedStart) * speedPattern.speedCurve.Evaluate(_speedTimer / speedPattern.speedDuration));
		road.speed = speed;
		ball.speed = speed;

		road.GameUpdate(deltaTime);
		ball.GameUpdate(deltaTime);

		if (score.combo.IsActivated)
		{
			_comboFOVTimer += deltaTime;
			float t = Mathf.Clamp01(_comboFOVTimer / comboFOVTimeDuration);
			gameCamera.fieldOfView = comboFOVNormal + ((comboFOVMax - comboFOVNormal) * comboFOVCurve.Evaluate(t));
		}

		if (_isGameOver)
		{
			GameEvents.gameOver?.Invoke();

			GameStop();
		}
	}


	public void GamePlay()
	{
		Init();
		_isPlayed = true;
		_isGameOver = false;
		_isInputEnabled = true;

		SoundController.Instance.PlayMusic(SoundController.Instance.mainMusic02);
	}
	public void GameStop()
	{

		_isPlayed = false;
		_isInputEnabled = false;

		road.Clear();
		//Init();

		SoundController.Instance.PlayMusic(SoundController.Instance.mainMusic01);
	}
	public void GameOver()
	{
		_isInputEnabled = false;

		ApplySlowMotion(() => _isGameOver = true);
	}

	public void GameReStart()
	{
		GameStop();
		GamePlay();
	}
	/*public void GameStart()
	{
		Init();
		_isPlayed = true;
	}*/

	public void Pause()
	{
		if (_pausedFading || _paused)
		{
			return;
		}
		_paused = true;

		_pausedFading = true;
		DOTween.To(() => _deltaTimeRatio, x => _deltaTimeRatio = x, 0.0f, 0.5f)
			.OnComplete(() => _pausedFading = false);
	}
	public void Resume()
	{
		if (_pausedFading || !_paused)
		{
			return;
		}
		_paused = false;

		_pausedFading = true;
		DOTween.To(() => _deltaTimeRatio, x => _deltaTimeRatio = x, 1.0f, 0.25f)
			.OnComplete(() => _pausedFading = false);
	}

	public void ApplySlowMotion(Action onSlowDone = null)
	{
		if (_isSlowMotion)
		{
			return;
		}
		_isSlowMotion = true;

		DOTween.To(() => _deltaTimeRatio, x => _deltaTimeRatio = x, slowMotionTimeRatio, slowMotionTimeIn)
			.OnComplete(() =>
				DOVirtual.DelayedCall(slowMotionTimeDuration, () =>
				{
					_isSlowMotion = false;
					onSlowDone?.Invoke();
					DOTween.To(() => _deltaTimeRatio, x => _deltaTimeRatio = x, 1.0f, slowMotionTimeOut);
				})
		);
	}

	private void OnRoadReadyToPlay(RoadBehaviour road)
	{
		PlatformBehaviour platform = road.GetNextPlatform();
		ball.SetNextPlatform(platform);
	}

	private void OnInteractedOnPlatform(BallBehaviour ball, PlatformBehaviour platform)
	{
		if (ball.interactType == platform.interactType)
		{
			score.UpdateScore(ball.interactType, platform.bonus.bonusType, true);

			platform.OnInteract(true);

			/*if (platform.bonusType != BonusType.None)
			{
				gameCamera.DOShakePosition(0.5f, 0.1f);
			}*/
			GameEvents.interactResult?.Invoke(score, true, platform.bonus.bonusType != BonusType.None);

			switch (platform.bonus.bonusType)
			{
				case BonusType.None:
					break;
				case BonusType.Apple:
					SoundController.Instance.PlaySFX(SoundController.Instance.sfxBonusCollectedApple);

					break;
				case BonusType.Cake:
					SoundController.Instance.PlaySFX(SoundController.Instance.sfxBonusCollectedCake);

					break;
				default:
					break;
			}
			
			if (score.combo.IsActivated)
			{
				platform.OnApplyComboVFX();
			}

			/*if (platform.interactType == InteractType.Green && score.combo.counterGreen >= 5)
			{
				platform.OnApplyComboVFX();
			}
			if (platform.interactType == InteractType.Red && score.combo.counterRed >= 5)
			{
				platform.OnApplyComboVFX();
			}*/
		}
		else
		{
			score.UpdateScore(platform.interactType, platform.bonus.bonusType, false);

			ball.OnDefeatInteract();
			platform.OnInteract(false);
			road.ClearBonuses();

			SoundController.Instance.PlaySFX(SoundController.Instance.sfxBonusHide);

			GameEvents.interactResult?.Invoke(score, false, false);

			if (score.lives == 0)
			{
				SoundController.Instance.StopMusic();
				if (score.points > score.highPoints)
				{
					score.highPoints = score.points;

					PlayerPrefs.SetInt("highPoints", score.highPoints);
				}

				if (score.points == 0)
				{
					_tutorial.loserCount++;
				}
				
				GameOver();

				SoundController.Instance.PlaySFX(SoundController.Instance.defeat);
			}

			_comboFOVTimer = 0.0f;
			gameCamera.DOFieldOfView(comboFOVNormal, 0.4f);
			gameCamera.DOShakePosition(0.5f, 0.1f);
		}

		ball.SetInteractType(InteractType.White);
		PlatformBehaviour nextPlatform = road.GetNextPlatform();
		ball.SetNextPlatform(nextPlatform);


		//////////////
		if (score.combo.counterGreen >= Score.COMBO_START_COUNT)
		{
			if (!road.HasBonus(InteractType.Green))
			{
				PlatformBehaviour bPlatform = road.GetNextPlatformAt(5, InteractType.Green);
				bPlatform.bonus.SetBonus(BonusType.Apple);

				SoundController.Instance.PlaySFX(SoundController.Instance.sfxBonusShow);
			}
		}
		if (score.combo.counterRed >= Score.COMBO_START_COUNT)
		{
			if (!road.HasBonus(InteractType.Red))
			{
				PlatformBehaviour bPlatform = road.GetNextPlatformAt(5, InteractType.Red);
				bPlatform.bonus.SetBonus(BonusType.Cake);

				SoundController.Instance.PlaySFX(SoundController.Instance.sfxBonusShow);
			}
		}

		SoundController.Instance.PlaySFX(UnityEngine.Random.value < 0.5f ? SoundController.Instance.sfxBallInteract01 : SoundController.Instance.sfxBallInteract02);
	}

	private void OnInputSwipe(int swipe)
	{
		if (!_isPlayed || !_isInputEnabled)
		{
			return;
		}

		if (_tutorial.enabled)
		{
			if (_tutorial.inputLock || _pausedFading)
			{
				return;
			}

			if (_tutorial.step == 2 && swipe == 1)
			{
				_tutorial.NextStep();
			}
			else if (_tutorial.step == 5 && swipe == -1)
			{
				_tutorial.NextStep();
			}
			/*else
			{
				return;
			}*/
		}

		if (_paused && !_tutorial.enabled)
		{
			return;
		}

		switch (swipe)
		{
			case -1:
				ball.SetInteractType(InteractType.Red);
				break;
			case 1:
				ball.SetInteractType(InteractType.Green);
				break;
		}
	}

}

public enum InteractType
{
	White,
	Green,
	Red
}

public enum BonusType
{
	None,
	Apple,
	Cake
}