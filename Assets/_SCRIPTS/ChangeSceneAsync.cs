using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAsync : MonoBehaviour {
	public delegate void OnProgress(float progress);
	public OnProgress onProgress;
	public string sceneToLoad = "main";
	private float progress;
	public float Progress 
	{ 
		get {
			return progress;
		} 
		private set {
			progress = value;
			if (onProgress != null) {
				onProgress(value);
			}
		} 
	}

	void Start () 
	{
		StartCoroutine(LoadMainSceneAsync());
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		Debug.Log("OnSceneLoaded " + scene.name);
   		//do stuff
		
  	}

	IEnumerator LoadMainSceneAsync()
    {
		Debug.Log("LoadMainSceneAsync");
		// WTF unity...
		// got to wait, so current scene shows up
		// on desktop its fast enough as is
		if (GameManager.IS_MOBILE) {
			yield return new WaitForSeconds(1);
		}
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
		asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
			// Debug.Log("Load progress... " + asyncLoad.progress);
			// apparently .8f or .9f is fully loaded for whatever goddamn reason
			const float fullyLoaded = .9f;
			Progress = Mathf.Min(asyncLoad.progress / fullyLoaded, 1);
			asyncLoad.allowSceneActivation = asyncLoad.progress >= fullyLoaded;
            yield return null;
        }
    }
}
