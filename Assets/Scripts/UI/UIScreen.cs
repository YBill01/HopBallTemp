using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{

	[SerializeField]
	protected UIController ui;

	public virtual void Open()
	{
		this.gameObject.SetActive(true);
	}
	public virtual void Close()
	{
		this.gameObject.SetActive(false);
	}

}