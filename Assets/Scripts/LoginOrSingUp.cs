using System;
using FiroozehGameService.Core;
using FiroozehGameService.Models;
using FiroozehGameService.Models.BasicApi.DBaaS;
using FiroozehGameService.Models.BasicApi.DBaaS.Aggregations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginOrSingUp : MonoBehaviour
{
    [SerializeField] private Menu menu;
    public TMP_InputField NickName, Mail, Password;
    public Text Error;
    public GameObject LoginOrSingUpPanel,Loading;
    public Button LoginOrSingupBtn, ChangeLoginOrSingupBtn, ForgetPwBtn;
    [SerializeField] public AudioHandler AH;
    [SerializeField] private QuizHandler quizHandler;

    private async void Start()
    {
        AH.MusicOnOff();
        if (PlayerPrefs.HasKey("UserToken"))
        {
            try
            {
                Loading.SetActive(true);
                await GameService.LoginOrSignUp.LoginWithToken(PlayerPrefs.GetString("UserToken"));

                //daryaft tedad id haye DataBase be komak neveshtan shart
                var Qaggregation = TableAggregation.Of("612a213d0a314700191c2518")
                    .WithProject(new ProjectAggregation("id")).Build();
                var LQaggregation = TableAggregation.Of("612a22a50a314700191c2519")
                    .WithProject(new ProjectAggregation("id")).Build();
                var Qdata = await GameService.Table.GetTableItems<Data.Quiz> (Qaggregation);
                var LQdata = await GameService.Table.GetTableItems<Data.Quiz> (LQaggregation);
                menu.QuizCount = Qdata.Count;
                menu.LegendryQuizCount = LQdata.Count;

                menu.GetData();
                Loading.SetActive(false);
                LoginOrSingUpPanel.SetActive(false);
            }
            catch (Exception e)
            {
                Error.gameObject.SetActive(true);
                if (Loading.activeInHierarchy)
                    Loading.SetActive(false);
                
                Vibration.Vibrate(40);
                if (e is GameServiceException) Error.text = "Game Server Error: " + e.Message;
                else Error.text = e.Message;
                
                LoginOrSingupBtn.enabled = true;
                ChangeLoginOrSingupBtn.enabled = true;
                ForgetPwBtn.enabled = true;
            }
        }
        else
        {
            LoginOrSingupBtn.enabled = true;
            ChangeLoginOrSingupBtn.enabled = true;
            ForgetPwBtn.enabled = true;
        }
    }

    public void ChangeLoginorSingup()
    {
        if (NickName.gameObject.activeInHierarchy)
        {
            NickName.gameObject.SetActive(false);
            ChangeLoginOrSingupBtn.transform.transform.GetChild(1).gameObject.SetActive(true);
            ChangeLoginOrSingupBtn.transform.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            NickName.gameObject.SetActive(true);
            ChangeLoginOrSingupBtn.transform.transform.GetChild(1).gameObject.SetActive(false);
            ChangeLoginOrSingupBtn.transform.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void ForgetPassword()
    {
        Application.OpenURL("https://gamesservice.ir/auth/app/forgetpassword");
    }
    
    public async void LoginOrSingup()
    {
        if (Error.gameObject.activeInHierarchy)
            Error.gameObject.SetActive(false);
        try
        {
            //SingUp
            if (NickName.gameObject.activeInHierarchy)
            {
                if (string.IsNullOrEmpty(NickName.text) ||
                    string.IsNullOrEmpty(Mail.text) ||
                    string.IsNullOrEmpty(Password.text))
                {
                    Error.gameObject.SetActive(true);
                    Vibration.Vibrate(40);
                    Error.text = "Login Invalid Input!";
                }

                else
                {
                    LoginOrSingupBtn.enabled = false;
                    ChangeLoginOrSingupBtn.enabled = false;
                    ForgetPwBtn.enabled = false;
                    
                    Loading.SetActive(true);
                    PlayerPrefs.SetString("UserToken",
                        await GameService.LoginOrSignUp.SignUp(NickName.text, Mail.text, Password.text));
                    
                    //daryaft tedad id haye DataBase be komak neveshtan shart
                    var Qaggregation = TableAggregation.Of("612a213d0a314700191c2518")
                        .WithProject(new ProjectAggregation("id")).Build();
                    var LQaggregation = TableAggregation.Of("612a22a50a314700191c2519")
                        .WithProject(new ProjectAggregation("id")).Build();
                    var Qdata = await GameService.Table.GetTableItems<Data.Quiz> (Qaggregation);
                    var LQdata = await GameService.Table.GetTableItems<Data.Quiz> (LQaggregation);
                    menu.QuizCount = Qdata.Count;
                    menu.LegendryQuizCount = LQdata.Count;
                    
                    PlayerPrefs.SetString("UserNickname", NickName.text);
                    menu.GetData();
                    Loading.SetActive(false);
                    AH.Click();
                    LoginOrSingUpPanel.SetActive(false);
                    
                    LoginOrSingupBtn.enabled = true;
                    ChangeLoginOrSingupBtn.enabled = true;
                    ForgetPwBtn.enabled = true;
                }
            }
            //Login
            else
            {
                if (string.IsNullOrEmpty(Mail.text) ||
                    string.IsNullOrEmpty(Password.text))
                {
                    Error.gameObject.SetActive(true);
                    Vibration.Vibrate(40);
                    Error.text = "Login Invalid Input!";
                }

                else
                {
                    LoginOrSingupBtn.enabled = false;
                    ChangeLoginOrSingupBtn.enabled = false;
                    ForgetPwBtn.enabled = false;
                    
                    Loading.SetActive(true);
                    PlayerPrefs.SetString("UserToken", await GameService.LoginOrSignUp.Login(Mail.text, Password.text));
                    
                    //daryaft tedad id haye DataBase be komak neveshtan shart
                    var Qaggregation = TableAggregation.Of("612a213d0a314700191c2518")
                        .WithProject(new ProjectAggregation("id")).Build();
                    var LQaggregation = TableAggregation.Of("612a22a50a314700191c2519")
                        .WithProject(new ProjectAggregation("id")).Build();
                    var Qdata = await GameService.Table.GetTableItems<Data.Quiz> (Qaggregation);
                    var LQdata = await GameService.Table.GetTableItems<Data.Quiz> (LQaggregation);
                    menu.QuizCount = Qdata.Count;
                    menu.LegendryQuizCount = LQdata.Count;
                    
                    PlayerPrefs.SetString("UserNickname", "Nick Name");
                    menu.GetData();
                    Loading.SetActive(false);
                    AH.Click();
                    LoginOrSingUpPanel.SetActive(false);
                    
                    LoginOrSingupBtn.enabled = true;
                    ChangeLoginOrSingupBtn.enabled = true;
                    ForgetPwBtn.enabled = true;
                }
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
            
            LoginOrSingupBtn.enabled = true;
            ChangeLoginOrSingupBtn.enabled = true;
            ForgetPwBtn.enabled = true;
        }
    }

    public void ShowPassword(TMP_InputField label)
    {
        if (label.contentType == TMP_InputField.ContentType.Standard)
            label.contentType = TMP_InputField.ContentType.Password;
        else label.contentType = TMP_InputField.ContentType.Standard;
        label.ForceLabelUpdate();
    }
}
