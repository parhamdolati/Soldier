using System;
using System.Collections.Generic;
using FiroozehGameService.Models.BasicApi.DBaaS;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;


public class Data : MonoBehaviour
{
    //Settings data
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music", 1);
        if (!PlayerPrefs.HasKey("SoundEffect"))
            PlayerPrefs.SetInt("SoundEffect", 1);
        if (!PlayerPrefs.HasKey("Vibrate"))
            PlayerPrefs.SetInt("Vibrate", 1);
    }
    
    [Serializable]
    public class TotalUserInfo
    {
        public void Initialize()
        {
            TotalWin = 0;
            TotalLoss = 0;
            TotalPoint = 0;
            TotalLegendryPoint = 0;
            CurrentQuiz = 1;
            DatabaseQuizNumber = new List<int>();
            LegendryDatabaseQuizNumber = new List<int>();
            AyinName = 0;
            Behdasht = 0;
            JangAfzar = 0;
            JangNowin = 0;
            Tajhizat = 0;
            Mamoriat = 0;
            Tim = 0;
            EstefadeAzZaimin = 0;
            Gashti = 0;
            Taktik = 0;
            Mokhaberat = 0;
            Naghshe = 0;
            Amaliat = 0;
            Moaref = 0;
        }
        
        [JsonProperty("TW")] [CanBeNull] // ba in esm dar server save mishavad
        public int ? TotalWin; // ? baraye nullable kardan ast yani mitavanad set nashavad
        
        [JsonProperty("TL")] [CanBeNull] 
        public int ? TotalLoss;
        
        [JsonProperty("TP")] [CanBeNull] //tedad kole setare ha
        public int ? TotalPoint;
        
        [JsonProperty("TLP")] [CanBeNull] //tedad kole setare haye legendry
        public int ? TotalLegendryPoint;
        
        [JsonProperty("CQ")] [CanBeNull] //andaze Soal haye javab dade shode mibashad
        public int ? CurrentQuiz;

        [JsonProperty("DQN")] [CanBeNull] //kodam soal ra az database gerefteEm
        public List<int>? DatabaseQuizNumber;
        
        [JsonProperty("LDQN")] [CanBeNull] //kodam soal legendry ra az database gerefteEm
        public List<int>? LegendryDatabaseQuizNumber;
        
        //Questions Catagorys
        [JsonProperty("AN")] [CanBeNull]
        public int ? AyinName;
        
        [JsonProperty("B")] [CanBeNull]
        public int ? Behdasht;
        
        [JsonProperty("JA")] [CanBeNull]
        public int ? JangAfzar;
        
        [JsonProperty("JN")] [CanBeNull]
        public int ? JangNowin;
        
        [JsonProperty("T")] [CanBeNull]
        public int ? Tajhizat;
        
        [JsonProperty("M")] [CanBeNull]
        public int ? Mamoriat;
        
        [JsonProperty("Ti")] [CanBeNull]
        public int ? Tim;
        
        [JsonProperty("EAZ")] [CanBeNull]
        public int ? EstefadeAzZaimin;
        
        [JsonProperty("G")] [CanBeNull]
        public int ? Gashti;
        
        [JsonProperty("Ta")] [CanBeNull]
        public int ? Taktik;
        
        [JsonProperty("Mo")] [CanBeNull]
        public int ? Mokhaberat;
        
        [JsonProperty("N")] [CanBeNull]
        public int ? Naghshe;
        
        [JsonProperty("A")] [CanBeNull]
        public int ? Amaliat;
        
        [JsonProperty("Moa")] [CanBeNull]
        public int ? Moaref;
    }
    
    [Serializable]
    public class PlayBtn
    {
        public PlayBtn()
        {
            IsWin = new List<bool?>();
            Stars = new List<int?>();
        }

        [JsonProperty("IW")] [CanBeNull] // aya play button win shode ya na
        public List<bool ?> IsWin;
        
        [JsonProperty("S")] [CanBeNull]// har play button chand setare gerefte
        public List<int ?>  Stars;
    }
    
    public class Quiz:TableItemHelper
    {
        [JsonProperty("id")]
        public int ? id;
        
        [JsonProperty("group")] [CanBeNull] 
        public string group;
        
        [JsonProperty("quiz")] [CanBeNull] 
        public string quiz;
        
        [JsonProperty("rightAnswer")] [CanBeNull] 
        public string rightAnswer;
        
        [JsonProperty("falseAnswer1")] [CanBeNull] 
        public string falseAnswer1;
        
        [JsonProperty("falseAnswer2")] [CanBeNull] 
        public string falseAnswer2;

        [JsonProperty("falseAnswer3")] [CanBeNull] 
        public string falseAnswer3;
    }
}
