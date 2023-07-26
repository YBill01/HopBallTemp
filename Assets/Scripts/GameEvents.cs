using System;

public static class GameEvents
{
	public static Action init;
	public static Action gameOver;
	public static Action<RoadBehaviour> roadReadyToPlay;
	public static Action<BallBehaviour, PlatformBehaviour> interactedOnPlatform;
	public static Action<Score, bool, bool> interactResult;
	public static Action<int> inputSwipe;
	public static Action<Tutorial> tutorial;


}