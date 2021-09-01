using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FiroozehGameService.Core;
using FiroozehGameService.Models;
using FiroozehGameService.Models.BasicApi;


public class Settings : MonoBehaviour
{
    public GameObject settingPanel, Setting, AboutUsPannel, Loading, LoginOrSingupPanel;
    public Sprite[] SettingsButtonsSprites;
    public Button MusicBtn, SoundEffectBtn, VibrateBtn, AccountBtn, AboutUsBtn, LogOutBtn;
    public Button ChangeNickname, ChangePassword;
    public TMP_InputField NicknameInput, OldPasswordinput, NewPasswordInput;
    public TMP_Text Error, MenuUserNickname;
    [SerializeField] public AudioHandler AH;
    [SerializeField] private LoginOrSingUp loginOrSingup;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Music")==1)
            MusicBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[0];
        else
            MusicBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[1];

        if (PlayerPrefs.GetInt("SoundEffect") == 1)
            SoundEffectBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[2];
        else
            SoundEffectBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[3];

        if (PlayerPrefs.GetInt("Vibrate") == 1)
            VibrateBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[4];
        else
            VibrateBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[4];

        SettingsListener();
    }

    private void SettingsListener()
    {
        MusicBtn.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetInt("Music") == 1) 
            { 
                PlayerPrefs.SetInt("Music", 0); 
                MusicBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[1]; 
                AH.MusicOnOff();
            }
            else 
            { 
                PlayerPrefs.SetInt("Music", 1); 
                MusicBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[0]; 
                AH.MusicOnOff();
            }
            AH.Click();
        });
        
        SoundEffectBtn.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetInt("SoundEffect") == 1)
            {
                PlayerPrefs.SetInt("SoundEffect",0);
                SoundEffectBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[3];
            }
            else
            {
                PlayerPrefs.SetInt("SoundEffect",1);
                SoundEffectBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[2];
                AH.Click();
            }
        });
        
        VibrateBtn.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetInt("Vibrate") == 1)
            {
                PlayerPrefs.SetInt("Vibrate",0);
                VibrateBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[5];
            }
            else
            {
                PlayerPrefs.SetInt("Vibrate",1);
                VibrateBtn.GetComponent<Image>().sprite = SettingsButtonsSprites[4];
                Vibration.Vibrate(40);
            }
        });
        
        AccountBtn.onClick.AddListener(() =>
        {
            if (NicknameInput.gameObject.activeInHierarchy)
            {
                AccountBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                NicknameInput.gameObject.SetActive(false);
                OldPasswordinput.gameObject.SetActive(false);
                NewPasswordInput.gameObject.SetActive(false);
                //LogOutBtn.gameObject.SetActive(false);
                Loading.SetActive(false);
                Error.text = "";
                Error.gameObject.SetActive(false);
            }
            else
            {
                AccountBtn.GetComponent<Image>().color = new Color32(209, 255, 205, 255);
                NicknameInput.gameObject.SetActive(true);
                OldPasswordinput.gameObject.SetActive(true);
                NewPasswordInput.gameObject.SetActive(true);
                //LogOutBtn.gameObject.SetActive(true);
            }
        });
        
        AboutUsBtn.onClick.AddListener(() =>
        {
            AboutUsPannel.transform.GetChild(0).transform.localPosition=new Vector2(0,-179);
            AboutUsPannel.SetActive(true);
        });
        
        ChangeNickname.onClick.AddListener(async () =>
        {
            try
            {
                if (string.IsNullOrEmpty(NicknameInput.text))
                    Error.text = "Invalid Input!";
                else
                {
                    Loading.SetActive(true);
                    EditUserProfile Profile = new EditUserProfile(nickName: NicknameInput.text);
                    await GameService.Player.EditCurrentPlayerProfile(Profile);
                    PlayerPrefs.SetString("UserNickname", NicknameInput.text);
                    MenuUserNickname.text = NicknameInput.text;
                    Loading.SetActive(false);
                }
            }
            catch (Exception e)
            {
                Error.gameObject.SetActive(true);
                if (Loading.activeInHierarchy)
                    Loading.SetActive(false);

                Vibration.Vibrate(40);
                if (e is GameServiceException) Error.text = "Game Server Error: " + e.Message;
                else Error.text = e.Message;
            }
        });
        
        ChangePassword.onClick.AddListener(async () =>
        {
            try
            {
                if(!string.IsNullOrEmpty(OldPasswordinput.text) && !string.IsNullOrEmpty(NewPasswordInput.text))
                {
                    Loading.SetActive(true); 
                    await GameService.Player.ChangePassword(OldPasswordinput.text, NewPasswordInput.text);
                    Loading.SetActive(false);
                }
                else
                    Error.text = "Invalid Input!";
                
            }
            catch (Exception e)
            {
                Error.gameObject.SetActive(true);
                if (Loading.activeInHierarchy)
                    Loading.SetActive(false);

                Vibration.Vibrate(40);
                if (e is GameServiceException) Error.text = "Game Server Error: " + e.Message;
                else Error.text = e.Message;
            }
        });
        
        /*LogOutBtn.onClick.AddListener(async () =>
        {
            GameService.Logout();
            settingPanel.SetActive(false);
            Setting.SetActive(false);
            PlayerPrefs.DeleteKey("UserToken");
            LoginOrSingupPanel.SetActive(true);
            loginOrSingup.ChangeLoginOrSingupBtn.enabled = true;
            loginOrSingup.ForgetPwBtn.enabled = true;
            loginOrSingup.LoginOrSingupBtn.enabled = true;
        });*/
    }

    public void OpenCloseSettings()
    {
        if (Setting.activeInHierarchy)
        {
            if (AboutUsPannel.activeInHierarchy)
            {
                AboutUsPannel.SetActive(false);
            }
            else
            {
                settingPanel.SetActive(false);
                Setting.SetActive(false);
                AccountBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                NicknameInput.gameObject.SetActive(false);
                OldPasswordinput.gameObject.SetActive(false);
                NewPasswordInput.gameObject.SetActive(false);
                LogOutBtn.gameObject.SetActive(false);
            }
        }
        else
        {
            AH.Click();
            settingPanel.SetActive(true);
            Setting.SetActive(true);
            
        }
    }
}
