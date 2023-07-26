using UnityEngine;

public class UIComboComponent : MonoBehaviour
{
	[SerializeField]
	private UIComboElement apple;
	[SerializeField]
	private UIComboElement cake;

	public void Init()
	{
		apple.Init();
		cake.Init();
	}


	public void UpdateProgress(Score score)
	{
		apple.UpdateProgress(score);
		cake.UpdateProgress(score);
	}

	public void ClearProgress()
	{
		apple.ClearProgress();
		cake.ClearProgress();
	}

}