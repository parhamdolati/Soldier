using System;
using System.Collections;
using FiroozehGameService.Core;
using FiroozehGameService.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameHandler : MonoBehaviour
{
    [SerializeField] private Menu menu;
    [SerializeField] private AudioHandler AH;
    [SerializeField] private QuizHandler quizHandler;
    Color WinColor = new Color32(114, 255, 64, 255);
    Color LossColor = new Color32(255, 93, 86, 255);
    public Image TimerImg;
    public TMP_Text TimerTxt;
    public GameObject ResultPanel, LoadingPanel, GameplayPanel;
    public Button Answer1Btn, Answer2Btn, Answer3Btn, Answer4Btn, NextQuizBtn;
    private int time;
    private Coroutine Timer;
    private int rightAnswer;

    public void StartGame(int RightAnswer)
    {
        Answer1Btn.enabled = true;
        Answer2Btn.enabled = true;
        Answer3Btn.enabled = true;
        Answer4Btn.enabled = true;
        Timer = StartCoroutine(timer());
        rightAnswer = RightAnswer;
    }
    
    public void WonOrLoss(Button btn)
    {
        //button dorost ra click karde ya na
        switch (int.Parse(btn.name))
        {
            case 1:
                if (rightAnswer == 1)
                {
                    Answer1Btn.GetComponent<Image>().color = WinColor;
                    AH.True();
                    DoWin();
                }
                else
                {
                    Answer1Btn.GetComponent<Image>().color = LossColor;
                    AH.False();
                    DoLoss();
                }
                break;
            case 2:
                if (rightAnswer == 2)
                {
                    Answer2Btn.GetComponent<Image>().color = WinColor;
                    AH.True();
                    DoWin();
                }
                else
                {
                    Answer2Btn.GetComponent<Image>().color = LossColor;
                    AH.False();
                    DoLoss();
                }
                break;
            case 3:
                if (rightAnswer == 3)
                {
                    Answer3Btn.GetComponent<Image>().color = WinColor;
                    AH.True();
                    DoWin();
                }
                else
                {
                    Answer3Btn.GetComponent<Image>().color = LossColor;
                    AH.False();
                    DoLoss();
                }
                break;
            case 4:
                if (rightAnswer == 4)
                {
                    Answer4Btn.GetComponent<Image>().color = WinColor;
                    AH.True();
                    DoWin();
                }
                else
                {
                    Answer4Btn.GetComponent<Image>().color = LossColor;
                    AH.False();
                    DoLoss();
                }
                break;
        }

        Answer1Btn.enabled = false;
        Answer2Btn.enabled = false;
        Answer3Btn.enabled = false;
        Answer4Btn.enabled = false;
        Result();
    }
    
    IEnumerator timer()
    {
        time = 30;
        while (time>0)
        {
            yield return new WaitForSeconds(1);
            if(time >= 11) TimerTxt.text = "00:"+(--time);
            else TimerTxt.text = "00:0"+(--time);

            TimerImg.fillAmount = (time / 30f);
        }
        DoLoss();
        Result();
    }
    
    async void Result()
    {
        try
        {
            StopCoroutine(Timer);
            
            if (menu.totalUserInfo.CurrentQuiz.Value % 7 == 0)
                menu.totalUserInfo.LegendryDatabaseQuizNumber.Add(quizHandler.QuizId);
            else menu.totalUserInfo.DatabaseQuizNumber.Add(quizHandler.QuizId);
            
            menu.totalUserInfo.CurrentQuiz++;
            await GameService.Save.SaveGame("TotalUserInfo", menu.totalUserInfo);
            await GameService.Save.SaveGame("PlayBtn", menu.playBtn);
            
            //agar soal ra dorost javab dade bashim cup mosbat migirm agar eshtebah cup manfi
            int score = menu.totalUserInfo.TotalPoint.Value +
                        (2 * menu.totalUserInfo.TotalLegendryPoint.Value) + menu.totalUserInfo.TotalWin.Value;
            if (menu.playBtn.IsWin[menu.playBtn.IsWin.Count - 1].Value)
            {
                await GameService.Leaderboard.SubmitScore("611ba83b64a10700194932ae", score);
            }
            else
            {
                score = score - ((((menu.totalUserInfo.CurrentQuiz.Value - 1) / 7) + 1) * menu.totalUserInfo.TotalLoss.Value);
                if (score < 0) score = 1;
                await GameService.Leaderboard.SubmitScore("611ba83b64a10700194932ae", score);
            }

            //agar soal legendry javab dadim ya akharin soal bazi ra javab dadim NextBtn ro namayesh nade
            if (menu.totalUserInfo.CurrentQuiz % 7 == 1 || menu.LastBtn == menu.totalUserInfo.CurrentQuiz.Value - 1)
                NextQuizBtn.gameObject.SetActive(false);
            else NextQuizBtn.gameObject.SetActive(true);
            
            ResultPanel.SetActive(true);
            ResultPanel.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Button>().enabled = false;
            ResultPanel.transform.GetChild(0).GetChild(5).GetChild(1).GetComponent<Button>().enabled = false;
            StartCoroutine(ResultAnimations());
        }
        catch (Exception e)
        {
            var Error = LoadingPanel.transform.GetChild(2).GetComponent<Text>();
            Button ExitBtn = LoadingPanel.transform.GetChild(3).GetComponent<Button>();
            if(e is GameServiceException) Error.text = "Game Server Error: " + e.Message;
            else Error.text = e.Message;
            
            ExitBtn.gameObject.SetActive(true);
            ExitBtn.onClick.AddListener(() =>
            {
                GameplayPanel.SetActive(false);
                LoadingPanel.SetActive(false);
            });
        }
    }

    IEnumerator ResultAnimations()
    {
        int stars = menu.playBtn.Stars[menu.playBtn.Stars.Count - 1].Value;
        yield return new WaitForSeconds(.7f);
        for (int i = 0; i < stars; i++)
        {
            yield return new WaitForSeconds(.3f);
            AH.GetStar();
            ResultPanel.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        }

        //taEn color Total Point va Legendry Total Point
        if (menu.playBtn.IsWin[menu.playBtn.IsWin.Count - 1].Value)
        {
            ResultPanel.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().color =
                new Color32(125, 255, 112, 255);
            ResultPanel.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().color =
                new Color32(125, 255, 112, 255);
        }
        else
        {
            ResultPanel.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().color =
                new Color32(255,112,120,255);
            ResultPanel.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().color =
                new Color32(255,112,120,255);
        }
        
        ResultPanel.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().text =
            menu.totalUserInfo.TotalPoint.Value.ToString(); //meghdar dehi total point
        yield return new WaitForSeconds(.3f);
        ResultPanel.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().text =
            menu.totalUserInfo.TotalLegendryPoint.Value.ToString(); //meghdar dehi total legendry star
        ResultPanel.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Button>().enabled = true;
        ResultPanel.transform.GetChild(0).GetChild(5).GetChild(1).GetComponent<Button>().enabled = true;
    }

    void DoWin()
    {
        menu.totalUserInfo.TotalWin++;
        menu.playBtn.IsWin.Add(true);
        if (time > 20)
        {
            menu.playBtn.Stars.Add(3);
            if (menu.totalUserInfo.CurrentQuiz % 7 != 0)
                menu.totalUserInfo.TotalPoint += 3;
            else menu.totalUserInfo.TotalLegendryPoint += 3;
        }
        else if (time > 10)
        {
            menu.playBtn.Stars.Add(2);
            if (menu.totalUserInfo.CurrentQuiz % 7 != 0)
                menu.totalUserInfo.TotalPoint += 2;
            else menu.totalUserInfo.TotalLegendryPoint += 2;
        }
        else
        {
            menu.playBtn.Stars.Add(1);
            if (menu.totalUserInfo.CurrentQuiz % 7 != 0)
                menu.totalUserInfo.TotalPoint += 1;
            else menu.totalUserInfo.TotalLegendryPoint += 1;
        }
        
        switch (quizHandler.data.group)
        {
            case "آیین نامه ها و اطلاعات رزمی":
                menu.totalUserInfo.AyinName++;
                break;
            case "بهداشت و کمک های اولیه":
                menu.totalUserInfo.Behdasht++;
                break;
            case "جنگ افزار شناسی و تیراندازی":
                menu.totalUserInfo.JangAfzar++;
                break;
            case "جنگ نوین":
                menu.totalUserInfo.JangNowin++;
                break;
            case "تجهیزات انفرادی":
                menu.totalUserInfo.Tajhizat++;
                break;
            case "ماموریت های انفرادی":
                menu.totalUserInfo.Mamoriat++;
                break;
            case "تیم و گروه تفنگدار":
                menu.totalUserInfo.Tim++;
                break;
            case "استفاده از زمین در رزم":
                menu.totalUserInfo.EstefadeAzZaimin++;
                break;
            case "گشتی و کمین":
                menu.totalUserInfo.Gashti++;
                break;
            case "تاکتیک":
                menu.totalUserInfo.Taktik++;
                break;
            case "مخابرات":
                menu.totalUserInfo.Mokhaberat++;
                break;
            case "نقشه خوانی":
                menu.totalUserInfo.Naghshe++;
                break;
            case "عملیات روانی":
                menu.totalUserInfo.Amaliat++;
                break;
            case "معارف جنگ":
                menu.totalUserInfo.Moaref++;
                break;
        }
    }

    void DoLoss()
    {
        menu.totalUserInfo.TotalLoss++;
        menu.playBtn.IsWin.Add(false);
        menu.playBtn.Stars.Add(0);
    }

    public void GoHome()
    {
        menu.TotalWin.text = menu.totalUserInfo.TotalWin.Value.ToString();
        menu.TotalLoss.text = menu.totalUserInfo.TotalLoss.Value.ToString();
        menu.TotalPoint.text = menu.totalUserInfo.TotalPoint.Value.ToString();
        menu.TotalLegendryPoint.text = menu.totalUserInfo.TotalLegendryPoint.Value.ToString();
        for (int i = 0; i < 3; i++)
        {
            ResultPanel.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        ResultPanel.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().text = null;
        ResultPanel.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().text = null;
        menu.CreateRoadmap();
        GameplayPanel.SetActive(false);
        ResultPanel.SetActive(false);
    }

    public void NextQuiz()
    {
        for (int i = 0; i < 3; i++)
        {
            ResultPanel.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        ResultPanel.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().text = null;
        ResultPanel.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().text = null;
        ResultPanel.SetActive(false);
        quizHandler.SetQuiz(menu.totalUserInfo.CurrentQuiz.Value);
    }
}
