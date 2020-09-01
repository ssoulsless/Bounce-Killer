using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource effects;
    public string effectsKey = "effects";
    public string musicKey = "music";

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(effectsKey, effects.volume);
        PlayerPrefs.SetFloat(musicKey, music.volume);
        PlayerPrefs.Save();
    }
}
