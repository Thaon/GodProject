using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpeechRecognizer : MonoBehaviour
{
	private SpeechRecognizerManager _speechManager = null;
	private bool _isListening = false;
	private string _message = "";

    private GodScript m_gs;

    public Text m_text;

	#region MONOBEHAVIOUR
	
	void Start ()
	{
        m_gs = FindObjectOfType<GodScript>() as GodScript;

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
        m_gs.ParseCommand(results);

		DebugLog ("Command:\n   " + results);
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

    void StartListening()
    {
        if (!_isListening)
        {
            m_text.text = "LISTENING!";
            _isListening = true;
            _speechManager.StartListening(1, "en-US");
        }
    }

    void StopListening()
    {
        if (_isListening)
        {
            m_text.text = "";
            _speechManager.StopListening();
            _isListening = false;
        }
    }

    public void Listen()
    {
        if (!_isListening)
            StartListening();
        else
            StopListening();
    }

	#region DEBUG

	private void DebugLog (string message)
	{
		Debug.Log (message);
		_message = message;
	}

	#endregion
}
