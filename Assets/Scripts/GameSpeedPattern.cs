using UnityEngine;

[CreateAssetMenu(menuName = "BunnyHopBall/GameSpeedPattern", fileName = "GameSpeedPattern NEW", order = int.MinValue)]
public class GameSpeedPattern : ScriptableObject
{
	public float speedStart;
	public float speedEnd;
	public float speedDuration;
	public AnimationCurve speedCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
}