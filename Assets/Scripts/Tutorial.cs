using UnityEngine;

public class Tutorial
{
	public bool enabled;
	public bool inputLock;
	public int step;

	public int loserCount;

	private float _timer;

	public Tutorial()
	{
		enabled = true;
		inputLock = true;
		step = 0;

		_timer = 0.0f;
	}

	public void UpdateProgress(float deltaTime)
	{
		_timer += deltaTime;

		switch (step)
		{
			case 0:
				NextStep();

				break;
			case 1:
				if (_timer >= 2.4f)
				{
					NextStep();
				}

				break;
			case 2:
				
				break;
			case 3:
				if (Game.Instance.score.platforms >= 4)
				{
					NextStep();
				}

				break;
			case 4:
				if (_timer >= 0.2f)
				{
					NextStep();
				}

				break;
			case 5:
				
				break;
			case 6:
				if (Game.Instance.score.platforms >= 11)
				{
					NextStep();
				}

				break;
			case 7:
				if (_timer >= 0.8f)
				{
					NextStep();
				}

				break;
		}
	}

	public void NextStep()
	{
		switch (step)
		{
			case 0:
				step++;
				inputLock = true;

				break;
			case 1:
				step++;

				_timer = 0.0f;
				inputLock = false;
				Game.Instance.Pause();

				break;
			case 2:
				step++;

				_timer = 0.0f;
				inputLock = false;
				Game.Instance.Resume();

				break;
			case 3:
				step++;

				_timer = 0.0f;

				break;
			case 4:
				step++;

				_timer = 0.0f;
				Game.Instance.Pause();

				break;
			case 5:
				step++;

				_timer = 0.0f;
				Game.Instance.Resume();

				break;
			case 6:
				step++;

				_timer = 0.0f;

				Game.Instance.ApplySlowMotion();

				break;
			case 7:
				step++;

				enabled = false;

				PlayerPrefs.SetInt("tutorial", 1);

				break;
		}

		GameEvents.tutorial?.Invoke(this);
	}
}