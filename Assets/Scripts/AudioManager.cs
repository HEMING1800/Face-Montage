using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource backgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Change the background musci pitch.
    public IEnumerator PitchChange()
    {
        for (float i = 0.3f; i <= 1; i += 0.005f)
        {
            backgroundMusic.pitch = i;
            Debug.Log("Pitch" + backgroundMusic.pitch);
            yield return null;
        }

    }
}
