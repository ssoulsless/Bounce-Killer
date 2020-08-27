using UnityEngine;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    private SettingsManager settingManager;

    [SerializeField] Slider effectsSlider;
    [SerializeField] Slider musicSlider;

    public float effectsVolume;
    public float musicVolume;
    private void Start()
    {
        settingManager = this.gameObject.GetComponent<SettingsManager>();
        if (PlayerPrefs.HasKey(settingManager.effectsKey))
        {
            effectsVolume = PlayerPrefs.GetFloat(settingManager.effectsKey);
        }
        else
        {
            effectsVolume = 1f;
        }
        if (PlayerPrefs.HasKey(settingManager.musicKey))
        {
            musicVolume = PlayerPrefs.GetFloat(settingManager.musicKey);
        }
        else
        {
            musicVolume = 1f;
        }
        effectsSlider.value = effectsVolume;
        musicSlider.value = musicVolume;
    }
}
