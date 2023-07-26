public class Score
{
	public const int MAX_LIVES = 3;
	public const int DEFAULT_LIVES = 3;
	public const int BONUS_POINTS = 2;
	public const int BONUS_LIVES = 1;
	public const int COMBO_START_COUNT = 5;

	public int highPoints;
	public int prevHighPoints;
	public int platforms;
	public int points;
	public int lives;

	public Combo combo;

	public void Init()
	{
		prevHighPoints = highPoints;
		platforms = 0;
		points = 0;
		lives = DEFAULT_LIVES;
		combo = new Combo();
	}

	public void UpdateScore(InteractType interactType, BonusType bonusType, bool result)
	{
		platforms++;

		if (result)
		{
			points += bonusType != BonusType.None ? BONUS_POINTS : 1;
			lives += bonusType != BonusType.None ? BONUS_LIVES : 0;
			lives = lives >= MAX_LIVES ? MAX_LIVES : lives;
			switch (interactType)
			{
				case InteractType.White:
					combo.counterWhite++;

					break;
				case InteractType.Green:
					combo.counterGreen++;

					break;
				case InteractType.Red:
					combo.counterRed++;

					break;
				default:
					break;
			}
		}
		else
		{
			lives -= interactType == InteractType.White ? 0 : 1;
			lives = lives > 0 ? lives : 0;
			if (lives > 0)
			{
				points -= bonusType != BonusType.None ? BONUS_POINTS : 1;
			}
			points = points > 0 ? points : 0;
			combo.Clear();
		}
	}

	public struct Combo
	{
		public int counterWhite;
		public int counterGreen;
		public int counterRed;

		public bool IsActivated
		{
			get
			{
				return counterGreen >= COMBO_START_COUNT || counterRed >= COMBO_START_COUNT;
			}
		}

		public void Clear()
		{
			counterWhite = 0;
			counterGreen = 0;
			counterRed = 0;
		}
	}
}