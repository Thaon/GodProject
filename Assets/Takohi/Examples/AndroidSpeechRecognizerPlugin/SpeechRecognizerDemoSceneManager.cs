using UnityEngine;
using System.Collections.Generic;

public class SpeechRecognizerDemoSceneManager : MonoBehaviour
{
	private SpeechRecognizerManager _speechManager = null;
	private bool _isListening = false;
	private string _message = "";

	#region MONOBEHAVIOUR
	
	void Start ()
	{
		if (Application.platform != RuntimePlatform.Android) {
			Debug.Log ("Speech recognition is only available on Android platform.");
			return;
		}

		if (!SpeechRecognizerManager.IsAvailable ()) {
			Debug.Log ("Speech recognition is not available on this device.");
			return;
		}

		// We pass the game object's name that will receive the callback messages.
		_speechManager = new SpeechRecognizerManager (gameObject.name);
	}

	void OnDestroy ()
	{
		if (_speechManager != null)
			_speechManager.Release ();
	}

	#endregion

	#region SPEECH_CALLBACKS

	void OnSpeechEvent (string e)
	{
		switch (int.Parse (e)) {
		case SpeechRecognizerManager.EVENT_SPEECH_READY:
			DebugLog ("Ready for speech");
			break;
		case SpeechRecognizerManager.EVENT_SPEECH_BEGINNING:
			DebugLog ("User started speaking");
			break;
		case SpeechRecognizerManager.EVENT_SPEECH_END:
			DebugLog ("User stopped speaking");
			break;
		}
	}

	void OnSpeechResults (string results)
	{
		_isListening = false;

		// Need to parse
		string[] texts = results.Split (new string[] { SpeechRecognizerManager.RESULT_SEPARATOR }, System.StringSplitOptions.None);
		;

		DebugLog ("Speech results:\n   " + string.Join ("\n   ", texts));
	}

	void OnSpeechError (string error)
	{
		switch (int.Parse (error)) {
		case SpeechRecognizerManager.ERROR_AUDIO:
			DebugLog ("Error during recording the audio.");
			break;
		case SpeechRecognizerManager.ERROR_CLIENT:
			DebugLog ("Error on the client side.");
			break;
		case SpeechRecognizerManager.ERROR_INSUFFICIENT_PERMISSIONS:
			DebugLog ("Insufficient permissions. Do the RECORD_AUDIO and INTERNET permissions have been added to the manifest?");
			break;
		case SpeechRecognizerManager.ERROR_NETWORK:
			DebugLog ("A network error occured. Make sure the device has internet access.");
			break;
		case SpeechRecognizerManager.ERROR_NETWORK_TIMEOUT:
			DebugLog ("A network timeout occured. Make sure the device has internet access.");
			break;
		case SpeechRecognizerManager.ERROR_NO_MATCH:
			DebugLog ("No recognition result matched.");
			break;
		case SpeechRecognizerManager.ERROR_NOT_INITIALIZED:
			DebugLog ("Speech recognizer is not initialized.");
			break;
		case SpeechRecognizerManager.ERROR_RECOGNIZER_BUSY:
			DebugLog ("Speech recognizer service is busy.");
			break;
		case SpeechRecognizerManager.ERROR_SERVER:
			DebugLog ("Server sends error status.");
			break;
		case SpeechRecognizerManager.ERROR_SPEECH_TIMEOUT:
			DebugLog ("No speech input.");
			break;
		default:
			break;
		}

		_isListening = false;
	}

	#endregion

	#region GUI
    
	void OnGUI ()
	{
		float originalScreenWidth = 400f;
		float originalScreenHeight = (originalScreenWidth / Screen.width) * Screen.height;
		float scale = Screen.width / originalScreenWidth;
		Matrix4x4 svMat = GUI.matrix; // save current matrix
		GUI.matrix = Matrix4x4.Scale (Vector3.one * scale);

		GUI.skin.label.fontStyle = FontStyle.Bold;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label (new Rect (0f, 0f, originalScreenWidth, originalScreenHeight * 0.05f), "Android Speech Recognizer Plugin");

		GUI.skin.label.fontStyle = FontStyle.Normal;
		GUI.skin.label.alignment = TextAnchor.LowerLeft;
		Rect layoutRect = new Rect (originalScreenWidth * 0.05f, originalScreenHeight * 0.1f, originalScreenWidth * 0.9f, originalScreenHeight * 0.85f);
		GUILayout.BeginArea (layoutRect, GUI.skin.box);
        
		GUILayout.BeginVertical ();
        
		GUILayout.Label ("Available: " + SpeechRecognizerManager.IsAvailable ());

		if (SpeechRecognizerManager.IsAvailable ()) {
			GUILayout.Label (_message);
			
			GUILayout.Space (35f);

			if (!_isListening && GUILayout.Button ("Start Listening")) {
				_isListening = true;
				_speechManager.StartListening (3, "en-US"); // Use english and return maximum three results.
				// _speechManager.StartListening (); // No parameters will use the device default language and returns maximum 5. results
			}

			/*
			 * Speech captured so far will be recognized as if the user had stopped speaking at this point. 
			 * This does not need to be called, as it will automatically stop the recognizer listening when it determines speech has completed.
			 */
			if (_isListening && GUILayout.Button ("Stop Listening")) {
				_speechManager.StopListening ();
				_isListening = false;
			}

			/*
			 * Cancels the speech recognition.
			 */
			if (_isListening && GUILayout.Button ("Cancel")) {
				_speechManager.CancelListening ();
				_isListening = false;
			}
		}

		GUILayout.EndVertical ();
        
		GUILayout.EndArea ();

		GUI.matrix = svMat;
	}

	#endregion

	#region DEBUG

	private void DebugLog (string message)
	{
		Debug.Log (message);
		_message = message;
	}

	#endregion
}
