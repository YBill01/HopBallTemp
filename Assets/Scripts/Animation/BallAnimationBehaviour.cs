using UnityEngine;

public class BallAnimationBehaviour : MonoBehaviour
{
	public void OnSwitchColor()
	{
		Material material = GetComponent<SkinnedMeshRenderer>().material;
		material.SetColor("_Color", material.GetColor("_Overlap_Color"));
	}
}