using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIController : MonoBehaviour
{

	[SerializeField]
	private UIScreen[] screens;

	/*[SerializeField]
	private RectTransform heart;*/


	/*[SerializeField]
	private TMP_Text pointsText;
	[SerializeField]
	private TMP_Text comboText;
	[SerializeField]
	private TMP_Text livesText;*/





	/*[SerializeField]
	private RectTransform tutorImage01;
	[SerializeField]
	private RectTransform tutorImage02;*/


	//private Animator _heartAnimator;
	//private int _heartLives;

	/*private void Awake()
	{
		_heartAnimator = heart.GetComponent<Animator>();
	}*/
	private void Start()
	{
		SoundController.Instance.audioMixer.SetFloat("music", Mathf.Log10(PlayerPrefs.GetFloat("musicVolume", 1.0f)) * 20);
		SoundController.Instance.audioMixer.SetFloat("sfx", Mathf.Log10(PlayerPrefs.GetFloat("sfxVolume", 1.0f)) * 20);

		OpenScreen(typeof(UIMainScreen));

		SoundController.Instance.PlayMusic(SoundController.Instance.mainMusic01);
	}

	/*private void OnEnable()
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
	}*/

	/*private void OnInit()
	{
		_heartAnimator.SetTrigger("Heart_3");
		_heartLives = Score.MAX_LIVES;

		pointsText.text = $"points {0}";
		comboText.text = $"combo <color=\"green\">{0}</color> <color=\"red\">{0}</color>";
		livesText.text = $"lives {0}";

		tutorImage01.gameObject.SetActive(false);
		tutorImage02.gameObject.SetActive(false);
	}
	private void OnGameOver()
	{

	}

	private void OnInteractResult(Score score, bool result)
	{
		pointsText.text = $"points {score.points}";
		comboText.text = $"combo <color=\"green\">{score.combo.counterGreen}</color> <color=\"red\">{score.combo.counterRed}</color>";
		livesText.text = $"lives {score.lives}";


		if (score.lives != _heartLives)
		{
			switch (score.lives)
			{
				case 0:

					break;
				case 1:
					_heartAnimator.SetTrigger("Heart_2-1");

					break;
				case 2:
					_heartAnimator.SetTrigger(result ? "Heart_1-2" : "Heart_3-2");

					break;
				case 3:
					_heartAnimator.SetTrigger("Heart_2-3");

					break;
				default:
					break;
			}

			_heartLives = score.lives;
		}






		*//*if (!result)
		{




			//heart.DOShakeScale(0.5f, 0.5f);
		}*//*
	}*/

	/*private void OnTutorial(Tutorial tutorial)
	{
		switch (tutorial.step)
		{
			case 2:
				tutorImage01.gameObject.SetActive(false);
				tutorImage02.gameObject.SetActive(true);

				break;
			case 3:
			case 4:
				tutorImage01.gameObject.SetActive(false);
				tutorImage02.gameObject.SetActive(false);

				break;
			case 5:
				tutorImage01.gameObject.SetActive(true);
				tutorImage02.gameObject.SetActive(false);

				break;
			case 6:
				tutorImage01.gameObject.SetActive(false);
				tutorImage02.gameObject.SetActive(false);

				break;
			case 7:
				tutorImage01.gameObject.SetActive(false);
				tutorImage02.gameObject.SetActive(false);

				break;
		}
	}*/


	public void OpenSettingsScreen()
	{

		OpenScreen(typeof(UISettingsScreen));
	}
	public void OpenScoreScreen()
	{
		//OpenScreen(typeof(UISettingsScreen));
	}

	public void OpenScreen(Type screenType)
	{
		foreach (var screen in screens)
		{
			if (screen.GetType() == screenType)
			{
				screen.Open();
			}
		}
	}

}