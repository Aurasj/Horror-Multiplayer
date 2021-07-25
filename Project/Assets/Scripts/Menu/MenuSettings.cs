using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class MenuSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Toggle shadowsToggle;
    [SerializeField] private Toggle fpsToggle;
    [SerializeField] private GameObject fpsTextG;
    private float deltaTime;

    bool isFPS;
    bool isVsync;

    Resolution[] resolutions;

    private void Start()
    {
        //Resolutions
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            //if (resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //Quality
        int qualityIndex = PlayerPrefs.GetInt("QualityIndex", 2);
        qualityDropdown.value = qualityIndex;
        Quality(qualityIndex);

        //Fullscreen
        bool isFullscreen = PlayerPrefs.GetInt("isFullscreen", 1) != 0;
        fullscreenToggle.isOn = isFullscreen;
        Fullscreen(isFullscreen);

        //FPS
        bool isFPS = PlayerPrefs.GetInt("isFPS") != 0;
        fpsToggle.isOn = isFPS;
        FPS(isFPS);

        //Shadows
        bool isShadows = PlayerPrefs.GetInt("isShadows", 1) != 0;
        shadowsToggle.isOn = isShadows;
        Shadows(isShadows);
    }
    private void Update()
    {
        if (isFPS)
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsTextG.GetComponent<TMP_Text>().text = Mathf.Ceil(fps).ToString();
        }
    }
    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void Quality(int index)
    {
        PlayerPrefs.SetInt("QualityIndex", index);
        QualitySettings.SetQualityLevel(index);

        //Vsync
        isVsync = PlayerPrefs.GetInt("isVsync") != 0;
        vsyncToggle.isOn = isVsync;
        Vsync(isVsync);
    }
    public void Fullscreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt("isFullscreen", (isFullscreen ? 1 : 0));
        Screen.fullScreen = isFullscreen;
    }
    public void Vsync(bool isVsync)
    {
        PlayerPrefs.SetInt("isVsync", (isVsync ? 1 : 0));
        if (isVsync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }
    public void FPS(bool isFPSS)
    {
        PlayerPrefs.SetInt("isFPS", (isFPSS ? 1 : 0));
        if (isFPSS)
        {
            fpsTextG.SetActive(true);
            isFPS = true;
        }
        else
        {
            fpsTextG.SetActive(false);
            isFPS = false;
        }
    }
    public void Shadows(bool isShadows)
    {
        PlayerPrefs.SetInt("isShadows", (isShadows ? 1 : 0));
        if (isShadows)
        {
            QualitySettings.shadows = ShadowQuality.All;
        }
        else
        {
            QualitySettings.shadows = ShadowQuality.Disable;
        }
    }
}
