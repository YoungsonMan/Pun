using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecPanel : BaseUI
{
    [SerializeField] TMP_Text playerSpec;
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerLevel;
    [SerializeField] TMP_Text playerClass;
    [SerializeField] TMP_Text playerStats;

    [SerializeField] long pLevel;
    [SerializeField] string pName;
    [SerializeField] string pClass;
    [SerializeField] string pStats;
    [SerializeField] long pStrength;
    [SerializeField] long pAgility;
    [SerializeField] long pIntelligence;
    [SerializeField] long pLuck;


    DatabaseReference userDataRef;
    DatabaseReference levelRef;
    DatabaseReference strengthRef;
    DatabaseReference agilityRef;
    DatabaseReference intelligenceRef;
    DatabaseReference luckRef;

    DatabaseTester.UserData userData;

    private void Start()
    {
        Init();
    }
    private void OnEnable()
    {
        string uID = BackendManager.Auth.CurrentUser.UserId;
        userDataRef = BackendManager.Database.RootReference.Child("UserData").Child(uID);
        levelRef = userDataRef.Child("level");
        strengthRef = userDataRef.Child("strength");
        agilityRef = userDataRef.Child("agility");
        intelligenceRef = userDataRef.Child("intelligence");
        luckRef = userDataRef.Child("luck");

        levelRef.ValueChanged += LevelRef_ValueChanged;

        Init();
    }

    private void OnDisable()
    {
        levelRef.ValueChanged -= LevelRef_ValueChanged; ;
    }
    private void Init()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        pName = user.DisplayName;
        pClass = userData.job;
        // pStrength = userData.strength;
        // pAgility = userData.agility;
        // pIntelligence = userData.intelligence;
        // pLuck = userData.luck;
        strengthRef = userDataRef.Child("strength");
        agilityRef = userDataRef.Child("agility");
        intelligenceRef = userDataRef.Child("intelligence");
        luckRef = userDataRef.Child("luck");

        playerSpec = GetUI<TMP_Text>("PlayerSpec");
        playerName = GetUI<TMP_Text>("PlayerName");
        playerLevel = GetUI<TMP_Text>("PlayerClass");
        playerClass = GetUI<TMP_Text>("PlayerClass");
        playerStats = GetUI<TMP_Text>("PlayerStats");

        playerSpec.text = "PLAYER SPEC : ";
        playerName.text = $"Name : {pName}";
        playerLevel.text = $"Level : {pLevel}";
        playerClass.text = $"Class : {pClass}";
        playerStats.text = $"Stats : {pStats}";

    }
    private void LevelRef_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        pLevel = (long)e.Snapshot.Child("level").Value;
        
        Debug.Log($"값변경 이벤트 확인 로그 Level : {e.Snapshot.Value.ToString()}");
    }
    private void Stats_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        pStrength = (long)e.Snapshot.Child("strength").Value;
        pIntelligence= (long)e.Snapshot.Child("intelligence").Value;
        pLuck = (long)e.Snapshot.Child("luck").Value;
        pAgility = (long)e.Snapshot.Child("agility").Value;
        Debug.Log($"값변경 이벤트 확인 로그 Level : {e.Snapshot.Value.ToString()}");
    }

    public void SubscribesEvents()
    {
        GetUI<Button>("LevelUpButton").onClick.AddListener(LevelUp);
    }
    public void LevelUp()
    {
        strengthRef.SetPriorityAsync(pStrength++);
        agilityRef.SetPriorityAsync(pAgility++);
        luckRef.SetPriorityAsync(pLuck++);
        intelligenceRef.SetPriorityAsync(pIntelligence++);
    }
}
