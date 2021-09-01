using System;
using UnityEngine;
using UnityEngine.UI;
using FiroozehGameService.Core;
using FiroozehGameService.Models;
using FiroozehGameService.Models.GSLive;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private AudioHandler audioHandler;
    public Data.PlayBtn playBtn = new Data.PlayBtn();
    public Data.TotalUserInfo totalUserInfo = new Data.TotalUserInfo();
    public TMP_Text Nickname;
    public TMP_Text TotalWin, TotalLoss, TotalLegendryPoint, TotalPoint;
    public GameObject Roadmap;
    public int QuizCount, LegendryQuizCount, LastBtn;
    public Sprite[] WinLossBtnSprite;//0 loss - 1 win - 2 noramlQuiz - 3 LQuiz
    private bool LastBtnIsPicked = false;
    
    public async void GetData()
    {
        GetComponent<Animator>().SetBool("StartMenu",true);
        audioHandler.MenuZoom();
        try
        {
            Member member = new Member();
            member = await GameService.Player.GetCurrentPlayer();
            PlayerPrefs.SetString("UserNickname",member.Name);
            playBtn = await GameService.Save.GetSaveGame<Data.PlayBtn>("PlayBtn");
            totalUserInfo = await GameService.Save.GetSaveGame<Data.TotalUserInfo>("TotalUserInfo");
            
        }
        catch (Exception e)
        { 
            totalUserInfo.Initialize();
            if(e is GameServiceException) Debug.Log("Game Server Error: " + e.Message);
            else Debug.Log(e.Message);
        }
        
        Nickname.text = PlayerPrefs.GetString("UserNickname");
        TotalWin.text = totalUserInfo.TotalWin.ToString();
        TotalLoss.text = totalUserInfo.TotalLoss.ToString();
        TotalLegendryPoint.text = totalUserInfo.TotalLegendryPoint.ToString();
        TotalPoint.text = totalUserInfo.TotalPoint.ToString();

        CreateRoadmap();

        Roadmap.SetActive(true);
    }

    public void CreateRoadmap()
    {
        if (totalUserInfo.CurrentQuiz % 7 == 1)
        {
            playBtn.Stars.Clear();
            playBtn.IsWin.Clear();
            for (int i = 0; i < 7; i++)
            {
                var btn = Roadmap.transform.GetChild(i);
                if (int.Parse(Roadmap.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text) % 7 != 0)
                    btn.GetComponent<Image>().sprite = WinLossBtnSprite[2];
                else
                {
                    btn.GetComponent<Image>().sprite = WinLossBtnSprite[3];
                }

                for (int j = 0; j < 3; j++)
                {
                    btn.GetChild(1).GetChild(j).gameObject.SetActive(false);
                }
            }
        }

        int CurrentQuiz = totalUserInfo.CurrentQuiz.Value;
        int level = (totalUserInfo.CurrentQuiz.Value - 1) / 7; // az 0 shro mishavad
        
        for (int i = 0; i < 7; i++)
        {
            int btnNumber = ((level * 7) + i + 1); // mohasebe shomare har button
            var btn = Roadmap.transform.GetChild(i); // gereftane Button i'om

            //be andaze soalat QuizButton misazim
            if ((i != 6 && btnNumber - 1 - level < QuizCount) || (i == 6 && level < LegendryQuizCount))
            {
                btn.GetChild(0).GetComponent<TMP_Text>().text =
                    btnNumber.ToString(); //set kardane shomare be button i'om

                //agar shomare button i'om barabare avalin soali bashad ke javab nadadim, target An ra faAl kon
                if (CurrentQuiz == btnNumber)
                {
                    btn.GetChild(2).gameObject.SetActive(true);
                    btn.GetComponent<Button>().enabled = true;
                }
                else
                {
                    btn.GetChild(2).gameObject.SetActive(false);
                    btn.GetComponent<Button>().enabled = false;
                }

                //handle kardan WinLoss budan va tedade setare ha
                if (playBtn.IsWin != null && i < playBtn.IsWin.Count)
                {
                    if (playBtn.IsWin[i].Value)
                    {
                        //taghir dadn sprit baraye win budan
                        btn.GetComponent<Image>().sprite = WinLossBtnSprite[1];

                        //faAl kardane star ha
                        var stars = btn.GetChild(1);
                        for (int j = 0; j < playBtn.Stars[i]; j++)
                        {
                            stars.GetChild(j).gameObject.SetActive(true);
                        }
                    }
                    else
                        btn.GetComponent<Image>().sprite = WinLossBtnSprite[0];
                }
            }
            else
            {
                btn.gameObject.SetActive(false);
                //akharin btn ra moshakhas mikonim
                if (!LastBtnIsPicked)
                {
                    LastBtnIsPicked = true;
                    LastBtn = btnNumber - 1;
                }
            }
        }
    }
}
