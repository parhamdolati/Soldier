using System.Collections.Generic;
using FiroozehGameService.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileHandler : MonoBehaviour
{
    [SerializeField] private Menu menu;
    [SerializeField] private AudioHandler audioHandler;
    public Button RecordsBtn, LeadersBtn;
    public GameObject RecordsView, LeadersView;
    public GameObject ProfilePanel, Profile;
    public Sprite[] OnOffBtnsSprite;//0 off - 1 on
    public GameObject Leaderboardtemp;
    public List<GameObject> LeadersTemp;

    public async void ShowProfile()
    {
        if (ProfilePanel.activeInHierarchy)
        {
            ProfilePanel.SetActive(false);
            Profile.SetActive(false);
            foreach (var l in LeadersTemp)
            {
                Destroy(l);
            }
            LeadersTemp.Clear();
            Vibration.Vibrate(40);
        }
        else
        {
            audioHandler.Click();
            //dasturat avalie baraye namayesh
            ProfilePanel.SetActive(true);
            Profile.SetActive(true);
            RecordsView.SetActive(true);
            RecordsView.transform.GetChild(0).localPosition = new Vector2(0, -258);
            LeadersView.SetActive(false);
            RecordsBtn.GetComponent<Image>().sprite = OnOffBtnsSprite[1];
            LeadersBtn.GetComponent<Image>().sprite = OnOffBtnsSprite[0];
            
            //meghdar dehi records
            for (int i = 0; i < 14; i++)
            {
                var list = RecordsView.transform.GetChild(0);
                var totalInfo = menu.totalUserInfo;
                int totalPoint = totalInfo.TotalLoss.Value + totalInfo.TotalWin.Value;
                switch (i)
                {
                    case 0:
                        list.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = 
                            (100*totalInfo.AyinName)/totalPoint + "درصد";
                        list.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.AyinName+"درست";
                        break;
                    case 1:
                        list.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.Behdasht)/totalPoint + "درصد";
                        list.GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.Behdasht+"درست";
                        break;
                    case 2:
                        list.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.JangAfzar)/totalPoint + "درصد";
                        list.GetChild(2).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.JangAfzar+"درست";
                        break;
                    case 3:
                        list.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.JangNowin)/totalPoint + "درصد";
                        list.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.JangNowin+"درست";
                        break;
                    case 4:
                        list.GetChild(4).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.Tajhizat)/totalPoint + "درصد";
                        list.GetChild(4).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.Tajhizat+"درست";
                        break;
                    case 5:
                        list.GetChild(5).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.Mamoriat)/totalPoint + "درصد";
                        list.GetChild(5).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.Mamoriat+"درست";
                        break;
                    case 6:
                        list.GetChild(6).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.Tim)/totalPoint + "درصد";
                        list.GetChild(6).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.Tim+"درست";
                        break;
                    case 7:
                        list.GetChild(7).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.EstefadeAzZaimin)/totalPoint + "درصد";
                        list.GetChild(7).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.EstefadeAzZaimin+"درست";
                        break;
                    case 8:
                        list.GetChild(8).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.Gashti)/totalPoint + "درصد";
                        list.GetChild(8).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.Gashti+"درست";
                        break;
                    case 9:
                        list.GetChild(9).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.Taktik)/totalPoint + "درصد";
                        list.GetChild(9).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.Taktik+"درست";
                        break;
                    case 10:
                        list.GetChild(10).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.Mokhaberat)/totalPoint + "درصد";
                        list.GetChild(10).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.Mokhaberat+"درست";
                        break;
                    case 11:
                        list.GetChild(11).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.Naghshe)/totalPoint + "درصد";
                        list.GetChild(11).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.Naghshe+"درست";
                        break;
                    case 12:
                        list.GetChild(12).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.Amaliat)/totalPoint + "درصد";
                        list.GetChild(12).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.Amaliat+"درست";
                        break;
                    case 13:
                        list.GetChild(13).GetChild(1).GetComponent<TMP_Text>().text =
                            (100*totalInfo.Moaref)/totalPoint + "درصد";
                        list.GetChild(13).GetChild(2).GetComponent<TMP_Text>().text = totalInfo.Moaref+"درست";
                        break;
                }
            }

            //meghdar dehi leadr boeard
            var Leaders = await GameService.Leaderboard.GetLeaderBoardDetails("611ba83b64a10700194932ae", 20);
            var score = Leaders.Scores;
            for (int i = 0; i < score.Count; i++)
            {
                var temp = Instantiate(Leaderboardtemp,transform.parent);
                temp.transform.GetChild(0).GetComponent<TMP_Text>().text = score[i].Rank.ToString();
                temp.transform.GetChild(1).GetComponent<TMP_Text>().text = score[i].Submitter.Name;
                temp.transform.GetChild(2).GetComponent<TMP_Text>().text = score[i].Value.ToString();
                temp.transform.parent = Leaderboardtemp.transform.parent;
                if (score[i].Submitter.User.IsMe)
                    temp.GetComponent<Image>().color = new Color32(255, 182, 255, 255);
                temp.SetActive(true);
                LeadersTemp.Add(temp);
            }
            
            RecordsBtn.onClick.AddListener(() =>
            {
                RecordsView.SetActive(true);
                LeadersView.SetActive(false);

                RecordsBtn.GetComponent<Image>().sprite = OnOffBtnsSprite[1];
                LeadersBtn.GetComponent<Image>().sprite = OnOffBtnsSprite[0];
            });

            LeadersBtn.onClick.AddListener(() =>
            {
                RecordsView.SetActive(false);
                LeadersView.SetActive(true);
                RecordsBtn.GetComponent<Image>().sprite = OnOffBtnsSprite[0];
                LeadersBtn.GetComponent<Image>().sprite = OnOffBtnsSprite[1];
            });
        }
    }
}
