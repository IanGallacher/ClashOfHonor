using UnityEngine;
using System.Collections;

public class NZZ_Soundtrack : MonoBehaviour {
	public AudioClip Soundtrack;
	private AudioSource SoundSource;
	
	void Awake() { 
		DontDestroyOnLoad(gameObject); 
		SoundSource = gameObject.AddComponent<AudioSource>(); 
		//SoundSource.playOnAwake = false; 
		//SoundSource.rolloffMode = AudioRolloffMode.Logarithmic; 
		//SoundSource.loop = true; 
	}
	
	private void Start() { 
		SoundSource.clip = Soundtrack; 
		SoundSource.Play(); 
	}
}
