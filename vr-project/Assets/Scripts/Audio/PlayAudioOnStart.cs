using UnityEngine;

public class PlayAudioOnStart : MonoBehaviour
{
    private PlayAudioAtPosition playAudioAtPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playAudioAtPosition = GetComponent<PlayAudioAtPosition>();
        playAudioAtPosition.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
