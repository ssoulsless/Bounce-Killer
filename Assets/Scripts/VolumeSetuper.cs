using UnityEngine;

public class VolumeSetuper : MonoBehaviour
{
    private float effectsVolume;
    private float musicVolume;

    private AudioSource musicSource;
    [SerializeField] AudioSource[] effectSources;

    private void Start()
    {
        musicSource = gameObject.GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("effects"))
        {
            effectsVolume = PlayerPrefs.GetFloat("effects");
        }
        else
        {
            effectsVolume = 1f;
        }
        if (PlayerPrefs.HasKey("music"))
        {
            musicVolume = PlayerPrefs.GetFloat("music");
        }
        else
        {
            musicVolume = 1f;
        }
        musicSource.volume = musicVolume;
        for (int i = 0; i<effectSources.Length; i++)
        {
            effectSources[i].volume = effectsVolume;
        }
        
    }
}
