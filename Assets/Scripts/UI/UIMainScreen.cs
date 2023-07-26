using TMPro;
using UnityEngine;

public class UIMainScreen : UIScreen
{
	[SerializeField]
	private UIMainScreenBack back;

	[SerializeField]
	private TMP_Text scoreText;

	[SerializeField]
	private Transform jumpingBallScene;

	private void OnEnable()
	{
		back.swipe += OnSwiped;
	}
	private void OnDisable()
	{
		back.swipe -= OnSwiped;
	}

	private void OnSwiped()
	{
		Close();
		ui.OpenScreen(typeof(UIGameScreen));
		Game.Instance.GamePlay();
	}

	public override void Open()
	{
		base.Open();

		scoreText.text = Game.Instance.score.highPoints.ToString();

		jumpingBallScene.gameObject.SetActive(true);
	}
	public override void Close()
	{
		base.Close();

		jumpingBallScene.gameObject.SetActive(false);
	}
}