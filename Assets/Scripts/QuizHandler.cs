using System;
using System.Collections;
using System.Linq;
using FiroozehGameService.Core;
using FiroozehGameService.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = System.Random;

public class QuizHandler : MonoBehaviour
{
    [SerializeField] private AudioHandler audioHandler;
    [SerializeField] private Menu menu;
    [SerializeField] private GameHandler gameHandler;
    public Data.Quiz data = new Data.Quiz();
    public GameObject LoadingPanel, Gameplay;
    public TMP_Text Answer1Txt, Answer2Txt, Answer3Txt, Answer4Txt;
    public TMP_Text QuizTxt, QuizGroupTxt;
    Random rnd = new Random();
    string DatabaseId;
    public int QuizId; // quiz shomare chand ra mikhahim

    public void Play(Button btn)
    {
        int btnNumber = int.Parse(btn.transform.GetChild(0).GetComponent<TMP_Text>().text);
        SetQuiz(btnNumber);
    }

    public async void SetQuiz(int CurrentPlayBtnNumber)
    {
        LoadingPanel.SetActive(true);
        try
        {
            //az kodam database data begirim
            if (CurrentPlayBtnNumber % 7 == 0)
            {
                DatabaseId = "612a22a50a314700191c2519";
                while (menu.totalUserInfo.LegendryDatabaseQuizNumber.Count != menu.LegendryQuizCount)
                {
                    QuizId = rnd.Next(1, menu.LegendryQuizCount + 1);
                    var b = menu.totalUserInfo.LegendryDatabaseQuizNumber.Where(n => n == QuizId);
                    if (b.Count() == 0)
                    {
                        break;
                    }
                }
            }
            else
            {
                DatabaseId = "612a213d0a314700191c2518";
                while (menu.totalUserInfo.DatabaseQuizNumber.Count != menu.QuizCount)
                {
                    QuizId = rnd.Next(1, menu.QuizCount + 1);
                    var b = menu.totalUserInfo.DatabaseQuizNumber.Where(n => n == QuizId);
                    if (b.Count() == 0)
                    {
                        break;
                    }
                }
            }

            //data hara set mikonim
            data = await GameService.Table.GetTableItem <Data.Quiz> (DatabaseId, QuizId.ToString());
            
            QuizGroupTxt.text = data.group;
            QuizTxt.text = data.quiz;
            int RightAnswerIndex = rnd.Next(1, 5); // javab dorost dar Btn shomare chand ast?
            switch (RightAnswerIndex)
            {
                case 1:
                    Answer1Txt.text = data.rightAnswer;
                    Answer2Txt.text = data.falseAnswer1;
                    Answer3Txt.text = data.falseAnswer2;
                    Answer4Txt.text = data.falseAnswer3;
                    break;
                case 2:
                    Answer1Txt.text = data.falseAnswer1;
                    Answer2Txt.text = data.rightAnswer;
                    Answer3Txt.text = data.falseAnswer2;
                    Answer4Txt.text = data.falseAnswer3;
                    break;
                case 3:
                    Answer1Txt.text = data.falseAnswer1;
                    Answer2Txt.text = data.falseAnswer2;
                    Answer3Txt.text = data.rightAnswer;
                    Answer4Txt.text = data.falseAnswer3;
                    break;
                case 4:
                    Answer1Txt.text = data.falseAnswer1;
                    Answer2Txt.text = data.falseAnswer2;
                    Answer3Txt.text = data.falseAnswer3;
                    Answer4Txt.text = data.rightAnswer;
                    break;
            }
            LoadingPanel.SetActive(false);
            
            //zamani ke az menu varede soalat mishavim animation ejra mishavad pas bayad sabr konim bad bazi shro shavad
            if (!Gameplay.activeInHierarchy)
            {
                Gameplay.SetActive(true);
                StartCoroutine(GameplayAnimation(RightAnswerIndex));
            }
            else
            {
                GamePlayReset();
                gameHandler.StartGame(RightAnswerIndex);
            }
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
                Gameplay.SetActive(false);
                LoadingPanel.SetActive(false);
            });
        }
    }

    IEnumerator GameplayAnimation(int rightAnswerIndex)
    {
        gameHandler.Answer1Btn.enabled = false;
        gameHandler.Answer2Btn.enabled = false;
        gameHandler.Answer3Btn.enabled = false;
        gameHandler.Answer4Btn.enabled = false;
        GamePlayReset();
        yield return new WaitForSeconds(.45f);
        audioHandler.GameStart();
        yield return new WaitForSeconds(.73f);
        gameHandler.StartGame(rightAnswerIndex);
    }

    void GamePlayReset()
    {
        gameHandler.TimerImg.fillAmount = 1;
        gameHandler.TimerTxt.text = "00:30";
        gameHandler.Answer1Btn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        gameHandler.Answer2Btn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        gameHandler.Answer3Btn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        gameHandler.Answer4Btn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
}
