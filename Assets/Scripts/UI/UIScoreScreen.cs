using TMPro;
using UnityEngine;

public class UIScoreScreen : UIScreen
{

	[SerializeField]
	private RectTransform newHighScoreComponent;
	[SerializeField]
	private TMP_Text newScoreText;
	[SerializeField]
	private RectTransform highScoreComponent;
	[SerializeField]
	private TMP_Text scoreText;
	[SerializeField]
	private TMP_Text highScoreText;

	[SerializeField]
	private Transform jumpingBallScene;




	public override void Open()
	{
		base.Open();

		newHighScoreComponent.gameObject.SetActive(false);
		highScoreComponent.gameObject.SetActive(false);

		newScoreText.text = Game.Instance.score.highPoints.ToString();
		scoreText.text = Game.Instance.score.points.ToString();
		highScoreText.text = Game.Instance.score.highPoints.ToString();

		if (Game.Instance.score.highPoints > Game.Instance.score.prevHighPoints)
		{
			newHighScoreComponent.gameObject.SetActive(true);
			highScoreComponent.gameObject.SetActive(false);

			SoundController.Instance.PlaySFX(SoundController.Instance.newRecord);
		}
		else
		{
			newHighScoreComponent.gameObject.SetActive(false);
			highScoreComponent.gameObject.SetActive(true);
		}

		jumpingBallScene.gameObject.SetActive(true);
	}
	public override void Close()
	{
		base.Close();

		ui.OpenScreen(typeof(UIMainScreen));
	}

}