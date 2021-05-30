using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    //[SerializeField] private TMP_Text fpsText;
    [SerializeField] private GameObject fpsTextG;
    private float deltaTime;

    bool isFPS;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = 1;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    private void Update()
    {
        if (isFPS)
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            //fpsText.text = Mathf.Ceil(fps).ToString();
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
        QualitySettings.SetQualityLevel(index);
    }
    public void Fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void Vsync(bool isVsync)
    {
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
}
