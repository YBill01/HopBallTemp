using UnityEngine;

public class SpawnRandomPattern : SpawnPattern
{
	private const float PLATFORM_LENGTH = 1.2f;
	private const float PLATFORM_SPACING = 1.2f;
	private const int WHITE_PLATFORM_MATCH = 11;

	private readonly float[] _positionVariants = { -0.75f, 0.0f, 0.75f };

	public override bool TryGetSpawnData(float distance, int count, out Vector3 position, out InteractType interactType)
	{
		position = Vector3.zero;
		interactType = InteractType.White;

		float length = (PLATFORM_LENGTH + PLATFORM_SPACING) * count;

		if (distance >= length)
		{
			switch (count)
			{
				case 0:
					interactType = InteractType.White;
					position = new Vector3(_positionVariants[1], 0.0f, PLATFORM_LENGTH + PLATFORM_SPACING);

					break;
				case 1:
				case 2:
				case 3:
					interactType = InteractType.Green;
					position = new Vector3(_positionVariants[1], 0.0f, PLATFORM_LENGTH + PLATFORM_SPACING);

					break;
				case 4:
				case 5:
				case 6:
					interactType = InteractType.Red;
					position = new Vector3(_positionVariants[1], 0.0f, PLATFORM_LENGTH + PLATFORM_SPACING);

					break;
				default:
					if ((count % WHITE_PLATFORM_MATCH) == 0)
					{
						interactType = InteractType.White;
						position = new Vector3(_positionVariants[1], 0.0f, PLATFORM_LENGTH + PLATFORM_SPACING);
					}
					else
					{
						if (UnityEngine.Random.value < 0.5f)
						{
							interactType = InteractType.Green;
						}
						else
						{
							interactType = InteractType.Red;
						}
						position = new Vector3(_positionVariants[UnityEngine.Random.Range(0, _positionVariants.Length)], 0.0f, PLATFORM_LENGTH + PLATFORM_SPACING);
					}

					break;
			}

			return true;
		}

		return false;
	}
}