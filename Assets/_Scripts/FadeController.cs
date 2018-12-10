using UnityEngine;
using UnityEngine.Events;

public class FadeController : MonoBehaviour {
	public UnityEvent fadeDone;

	public void FadeDoneHandler() {
		fadeDone.Invoke();
	}
}