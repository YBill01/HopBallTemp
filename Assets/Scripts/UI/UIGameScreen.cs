using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIGameScreen : UIScreen
{
	[SerializeField]
	private RectTransform heart;

	[SerializeField]
	private RectTransform[] hearts;

	[Space(10)]
	[SerializeField]
	private UIComboComponent combo;
	[SerializeField]
	private RectTransform comboText;

	[Space(10)]
	[SerializeField]
	private TMP_Text pointsText;

	[Space(10)]
	[SerializeField]
	private RectTransform tutorAnimation01;
	[SerializeField]
	private RectTransform tutorAnimation02;
	[SerializeField]
	private RectTransform tutorAnimation03;


	//private Animator _heartAnimator;
	private int _heartLives;





	private void Awake()
	{
		//_heartAnimator = heart.GetComponent<Animator>();
	}

	private void OnEnable()
	{
		GameEvents.init += OnInit;
		GameEvents.gameOver += OnGameOver;
		GameEvents.interactResult += OnInteractResult;
		GameEvents.tutorial += OnTutorial;
	}



	private void OnDisable()
	{
		GameEvents.init -= OnInit;
		GameEvents.gameOver -= OnGameOver;
		GameEvents.interactResult -= OnInteractResult;
		GameEvents.tutorial -= OnTutorial;
	}



	private void OnInit()
	{
		pointsText.text = Game.Instance.score.points.ToString();

		//_heartAnimator.SetTrigger("Heart_3");
		_heartLives = Score.MAX_LIVES;
		foreach (var heart in hearts)
		{
			heart.GetComponent<Animator>().SetTrigger("True");
		}

		combo.Init();

		tutorAnimation01.gameObject.SetActive(false);
		tutorAnimation02.gameObject.SetActive(false);
		tutorAnimation03.gameObject.SetActive(false);
	}
	private void OnGameOver()
	{
		Close();
		ui.OpenScreen(typeof(UIScoreScreen));
	}

	private void OnInteractResult(Score score, bool result, bool bonus)
	{
		pointsText.text = Game.Instance.score.points.ToString();

		int count = score.lives - _heartLives;
		while (count != 0)
		{
			bool incrementFlag = count > 0;
			count -= count < 0 ? -1 : 1;
			Animator currentHeartAnimator = hearts[(_heartLives + count + (incrementFlag ? 1 : 0)) - 1].GetComponent<Animator>();
			currentHeartAnimator.SetTrigger(incrementFlag ? "ToTrue" : "ToFalse");
		}
		_heartLives = score.lives;

		/*if (score.lives != _heartLives)
		{
			if (score.lives > _heartLives)
			{
				Animator currentHeartAnimator = hearts[score.lives - 1].GetComponent<Animator>();
			}
			else
			{
				Animator currentHeartAnimator = hearts[_heartLives].GetComponent<Animator>();
			}
			
			switch (score.lives)
			{
				case 0:
					currentHeartAnimator = hearts[0].GetComponent<Animator>();
					
					break;
				case 1:
					currentHeartAnimator = hearts[1].GetComponent<Animator>();
					*//*_heartAnimator.SetTrigger("Heart_2-1");
					if (!result)
					{
						heart.DOShakeAnchorPos(0.5f, 25.0f);
					}*//*

					break;
				case 2:
					currentHeartAnimator = hearts[2].GetComponent<Animator>();
					*//*_heartAnimator.SetTrigger(result ? "Heart_1-2" : "Heart_3-2");
					if (!result)
					{
						heart.DOShakeAnchorPos(0.5f, 25.0f);
					}*//*

					break;
				case 3:
					currentHeartAnimator = hearts[0].GetComponent<Animator>();
					//_heartAnimator.SetTrigger("Heart_2-3");

					break;
				default:
					break;
			}

			_heartLives = score.lives;
		}*/

		if (result)
		{
			combo.UpdateProgress(score);

			if (bonus)
			{
				comboText.GetComponentInChildren<TMP_Text>(true).text = $"x{Score.BONUS_POINTS}";
				comboText.GetComponent<Animator>().SetTrigger("Show");

				//Debug.Log($"x{Score.BONUS_POINTS}");
			}
		}
		else
		{
			combo.ClearProgress();

			pointsText.rectTransform.DOShakeAnchorPos(0.5f, 50.0f);

			//heart.DOShakeScale(0.5f, 0.5f);
		}

	}

	private void OnTutorial(Tutorial tutorial)
	{
		switch (tutorial.step)
		{
			case 2:
				tutorAnimation01.gameObject.SetActive(false);
				tutorAnimation02.gameObject.SetActive(true);
				tutorAnimation03.gameObject.SetActive(false);

				break;
			case 3:
			case 4:
				tutorAnimation01.gameObject.SetActive(false);
				tutorAnimation02.gameObject.SetActive(false);
				tutorAnimation03.gameObject.SetActive(false);

				break;
			case 5:
				tutorAnimation01.gameObject.SetActive(true);
				tutorAnimation02.gameObject.SetActive(false);
				tutorAnimation03.gameObject.SetActive(false);

				break;
			case 6:
				tutorAnimation01.gameObject.SetActive(false);
				tutorAnimation02.gameObject.SetActive(false);
				tutorAnimation03.gameObject.SetActive(false);

				break;
			case 7:
				tutorAnimation01.gameObject.SetActive(false);
				tutorAnimation02.gameObject.SetActive(false);
				tutorAnimation03.gameObject.SetActive(true);

				break;
			case 8:
				tutorAnimation01.gameObject.SetActive(false);
				tutorAnimation02.gameObject.SetActive(false);
				tutorAnimation03.gameObject.SetActive(false);

				break;
		}
	}
}