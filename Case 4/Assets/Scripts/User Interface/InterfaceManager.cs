﻿using LitJson;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    [Header("Only populate if current scene requires the diary!")]
    [Space(10)]
    #region Diary and Quests interface references
    [Space(10)]
    public GameObject QuestButtonPrefab;
    public GameObject CurrentQuestsDisplay;
    public GameObject AvailableQuestsDisplay;
    public GameObject CompletedQuestsDisplay;
    [Space(10)]
    public GameObject QuestObjectivePrefab;
    public TextMeshProUGUI SelectedQuestTitle;
    public TextMeshProUGUI SelectedQuestDescription;
    public TextMeshProUGUI SelectedQuestStatus;
    public GameObject SelectedQuestObjectivesDisplay;
    private Quest _currentlySelectedQuest;
    #endregion

    [Header("Only populate if current scene requires the diary!")]
    [Space(10)]
    #region Item details for bottom UI inventory
    [Space(10)]
    public ItemsLoader ItemsLoader;
    public GameObject BottomUIInventory;
    public GameObject ItemActionsWindow;
    public Item ItemSelected;
    public GameObject ItemDetailsWindow;
    public Image ItemDetailsPortrait;
    public TextMeshProUGUI ItemDetailsName;
    public TextMeshProUGUI ItemDetailsDescription;
    public TextMeshProUGUI ItemDetailsActives;
    #endregion

    [Header("Only populate if current scene is the main map!")]
    #region Area references
    [Space(10)]
    public GameObject AreaLockedErrorPopup;
    #endregion

    [Header("Only populate if current scene requires the diary!")]
    [Space(10)]
    #region Blueprint pieces references
    [Space(10)]
    public Image Blueprint1;
    public Image Blueprint2;
    public Image Blueprint3;
    public Image Blueprint4;
    public Image Blueprint5;
    #endregion
    
    [Header("Only populate if current scene has an interface of icons!")]
    [Space(10)]
    #region Icons display variables
    [Space(10)]
    public List<GameObject> IconDisplays = new List<GameObject>();

    public Image _bodyIcon;
    public Image _faceIcon;
    public Image _hairIcon;
    public Image _topIcon;
    #endregion

    [Header("Only populate if current scene requires NPCs for dialogue!")]
    [Space(10)]
    #region Dialogue body parts display variables
    public Image _bodyPart;
    public Image _facePart;
    public Image _hairPart;
    public Image _topPart;
    public Image _botPart;
    public Image _shoesPart;
    #endregion

    public Canvas InventoryUICanvas;

    public CameraBehavior CameraBehavior;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Escape Game")
        {
            SetupQuestsInDiary();
            LoadCharacterAppearance();
        }

        //if(Character.Instance.TutorialCompleted == false)
        //{
        //    foreach(GameObject mapArea in MapAreas)
        //    {
        //        if(mapArea.name != "Map Area 1")
        //        {
        //            mapArea.SetActive(false);
        //        }
        //    }
        //}
    }

    /// <summary>
    /// This function must run whenever a scene containing the inventory backpack
    /// is initiated OR the player's inventory has to be updated.
    /// </summary>
    public void RefreshInvetory()
    {
        Character.Instance.ReloadInventory();
    }

    /// <summary>
    /// This function displays the blueprint parts the player has collected whenever
    /// he enters the blueprints page.
    /// </summary>
    public void LoadBlueprints()
    {
        Character.Instance.RefreshJsonData();
        string dataToJson = File.ReadAllText(Character.Instance.PlayerStatsFilePath);
        JsonData characterData = JsonMapper.ToObject(dataToJson);
        
        for (int i = 1; i < 6; i++)
        {
            if (characterData["Blueprint" + i].ToString() == "True")
            {
                Sprite blueprintSprite = Resources.Load<Sprite>("Blueprints/Blueprint" + i + "Unlocked");
                if (i == 1)
                {
                    Blueprint1.sprite = blueprintSprite;
                    Blueprint1.color = new Color(255, 255, 255, 1);
                }
                else if (i == 2)
                {
                    Blueprint2.sprite = blueprintSprite;
                    Blueprint2.color = new Color(255, 255, 255, 1);
                }
                else if (i == 3)
                {
                    Blueprint3.sprite = blueprintSprite;
                    Blueprint3.color = new Color(255, 255, 255, 1);
                }
                else if (i == 4)
                {
                    Blueprint4.sprite = blueprintSprite;
                    Blueprint4.color = new Color(255, 255, 255, 1);
                }
                else if (i == 5)
                {
                    Blueprint5.sprite = blueprintSprite;
                    Blueprint5.color = new Color(255, 255, 255, 1);
                }
            }
            else if (characterData["Blueprint" + i].ToString() == "False")
            {
                Sprite blueprintSprite = Resources.Load<Sprite>("Blueprints/Blueprint" + i + "Locked");
                if (i == 1)
                {
                    Blueprint1.sprite = blueprintSprite;
                    Blueprint1.color = new Color(255, 255, 255, 0);
                } else if (i == 2)
                {
                    Blueprint2.sprite = blueprintSprite;
                    Blueprint2.color = new Color(255, 255, 255, 0);
                } else if (i == 3)
                {
                    Blueprint3.sprite = blueprintSprite;
                    Blueprint3.color = new Color(255, 255, 255, 0);
                } else if (i == 4)
                {
                    Blueprint4.sprite = blueprintSprite;
                    Blueprint4.color = new Color(255, 255, 255, 0);
                } else if (i == 5)
                {
                    Blueprint5.sprite = blueprintSprite;
                    Blueprint5.color = new Color(255, 255, 255, 0);
                }
            }
        }
    }

    /// <summary>
    /// This function loads the quest/s in the diary of the player.
    /// </summary>
    public void SetupQuestsInDiary()
    {
        if (SceneManager.GetActiveScene().name != "Escape Game")
        {
            if (CurrentQuestsDisplay != null)
            {
                for (int i = 0; i < CurrentQuestsDisplay.transform.GetChild(0).transform.childCount; i++)
                {
                    Destroy(CurrentQuestsDisplay.transform.GetChild(0).transform.GetChild(i).gameObject);
                }
                for (int i = 0; i < AvailableQuestsDisplay.transform.GetChild(0).transform.childCount; i++)
                {
                    Destroy(AvailableQuestsDisplay.transform.GetChild(0).transform.GetChild(i).gameObject);
                }
                for (int i = 0; i < CompletedQuestsDisplay.transform.GetChild(0).transform.childCount; i++)
                {
                    Destroy(CompletedQuestsDisplay.transform.GetChild(0).transform.GetChild(i).gameObject);
                }
            }

            List<Quest> questsToLoad = new List<Quest>();

            if (SettingsManager.Instance != null)
            {
                switch (SettingsManager.Instance.Language)
                {
                    case "English":
                        questsToLoad = Character.Instance.AllQuests;
                        break;
                    case "Dutch":
                        questsToLoad = Character.Instance.AllQuestsDutch;
                        break;
                }
            }

            //Character.Instance.AllQuests.Clear();
            if (CurrentQuestsDisplay != null)
            {
                foreach (Quest loadedQuest in questsToLoad)
                {
                    if (loadedQuest.ProgressStatus == "Ongoing" || loadedQuest.ProgressStatus == "Nog niet gedaan")
                    {
                        GameObject newQuestButton = Instantiate(QuestButtonPrefab, CurrentQuestsDisplay.transform.GetChild(0).transform);
                        newQuestButton.GetComponentInChildren<TextMeshProUGUI>().text = loadedQuest.Name;
                    }
                }

                foreach (Quest loadedQuest in questsToLoad)
                {
                    GameObject newQuestButton = Instantiate(QuestButtonPrefab, AvailableQuestsDisplay.transform.GetChild(0).transform);
                    newQuestButton.GetComponentInChildren<TextMeshProUGUI>().text = loadedQuest.Name;
                }

                foreach (Quest loadedQuest in questsToLoad)
                {
                    if (loadedQuest.ProgressStatus == "Completed" || loadedQuest.ProgressStatus == "Gedaan")
                    {
                        GameObject newQuestButton = Instantiate(QuestButtonPrefab, CompletedQuestsDisplay.transform.GetChild(0).transform);
                        newQuestButton.GetComponentInChildren<TextMeshProUGUI>().text = loadedQuest.Name;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// This function hides and shows interfaces in the main menu whenever the
    /// player selects a new interface he wants to display (settings, diary).
    /// </summary>
    /// <param name="ui"></param>
    public void ToggleUI(Object ui)
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.ButtonPress);

        GameObject uiObj = (GameObject)ui;

        foreach (GameObject obj in IconDisplays)
        {
            if (obj == uiObj && obj.activeSelf == false)
            {
                if (CameraBehavior != null)
                {
                    CameraBehavior.IsUIOpen = true;
                }

                obj.SetActive(true);
                
                if (uiObj.transform.tag == "Settings UI")
                {
                    SettingsManager.Instance.UpdateHiglightedLanguageIcons();
                }
            }
            else if (obj == uiObj && obj.activeSelf == true)
            {
                if (CameraBehavior != null)
                {
                    CameraBehavior.IsUIOpen = false;
                    CameraBehavior.IsInterfaceElementSelected = false;
                    CameraBehavior.TapPosition = Camera.main.transform.position;
                }

                    obj.SetActive(false);

            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Opens a window or pop-up (game object) by activating it.
    /// </summary>
    /// <param name="obj"></param>
    public void ClosePopup(Object obj)
    {
        GameObject popupObject = obj as GameObject;
        popupObject.SetActive(false);

        if (IconDisplays.Contains(popupObject))
        {
            StartCoroutine(Character.Instance.EnableEntityTapping());
            StartCoroutine(Character.Instance.EnableCameraInteraction());
        }
    }

    /// <summary>
    /// Closes a window or pop-up (game object) by disabling it.
    /// </summary>
    /// <param name="obj"></param>
    public void OpenPopup(Object obj)
    {
        GameObject popupObject = obj as GameObject;
        popupObject.SetActive(true);
    }
    
    #region Quests, diary and character functions
    public void DisplayQuestDetails(Object obj)
    {
        if (obj == null)
        {
            SelectedQuestTitle.text = string.Empty;
            SelectedQuestDescription.text = string.Empty;
            SelectedQuestStatus.text = string.Empty;

            for (int i = 0; i < SelectedQuestObjectivesDisplay.transform.childCount; i++)
            {
                Destroy(SelectedQuestObjectivesDisplay.transform.GetChild(i).gameObject);
            }
        } else
        {
            GameObject button = obj as GameObject;

            _currentlySelectedQuest = null;

            switch (SettingsManager.Instance.Language)
            {
                case "English":
                    foreach (Quest loadedQuest in Character.Instance.AllQuests)
                    {
                        if (loadedQuest.Name == button.GetComponentInChildren<TextMeshProUGUI>().text)
                        {
                            _currentlySelectedQuest = loadedQuest;

                            SelectedQuestTitle.text = "Title: " + loadedQuest.Name;
                            SelectedQuestDescription.text = "Description: " + loadedQuest.Description;
                            SelectedQuestStatus.text = "Status: " + loadedQuest.ProgressStatus;

                            for (int i = 0; i < SelectedQuestObjectivesDisplay.transform.childCount; i++)
                            {
                                Destroy(SelectedQuestObjectivesDisplay.transform.GetChild(i).gameObject);
                            }

                            foreach (Objective objective in loadedQuest.Objectives)
                            {
                                if(objective.CompletedStatus == false)
                                {
                                    GameObject newObjective = Instantiate(QuestObjectivePrefab,
                                    SelectedQuestObjectivesDisplay.transform);
                                    newObjective.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = objective.Name;
                                    newObjective.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Done: No";
                                    break;
                                } else
                                {
                                    GameObject newObjective = Instantiate(QuestObjectivePrefab,
                                    SelectedQuestObjectivesDisplay.transform);
                                    newObjective.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = objective.Name;
                                    newObjective.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Done: Yes";
                                }
                            }
                        }
                    }
                    break;
                case "Dutch":
                    foreach (Quest loadedQuest in Character.Instance.AllQuestsDutch)
                    {
                        if (loadedQuest.Name == button.GetComponentInChildren<TextMeshProUGUI>().text)
                        {
                            _currentlySelectedQuest = loadedQuest;

                            SelectedQuestTitle.text = "Naam: " + loadedQuest.Name;
                            SelectedQuestDescription.text = "Omschrijving: " + loadedQuest.Description;
                            SelectedQuestStatus.text = "Status: " + loadedQuest.ProgressStatus;

                            for (int i = 0; i < SelectedQuestObjectivesDisplay.transform.childCount; i++)
                            {
                                Destroy(SelectedQuestObjectivesDisplay.transform.GetChild(i).gameObject);
                            }
                            foreach (Objective objective in loadedQuest.Objectives)
                            {
                                if (objective.CompletedStatus == false)
                                {
                                    GameObject newObjective = Instantiate(QuestObjectivePrefab,
                                    SelectedQuestObjectivesDisplay.transform);
                                    newObjective.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = objective.Name;
                                    newObjective.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Gedaan: Nee";
                                    break;
                                }
                                else
                                {
                                    GameObject newObjective = Instantiate(QuestObjectivePrefab,
                                    SelectedQuestObjectivesDisplay.transform);
                                    newObjective.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = objective.Name;
                                    newObjective.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Gedaan: Ja";
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }

    public void TakeOnQuest()
    {
        if (_currentlySelectedQuest.ProgressStatus == "Available")
        {
            foreach (Quest completedQuest in Character.Instance.CompletedQuests)
            {
                if (completedQuest.Name == _currentlySelectedQuest.Name)
                {
                    Character.Instance.MoveQuestStageStatus(_currentlySelectedQuest, "Completed Quests", "CurrentQuests");
                    completedQuest.ProgressStatus = "Ongoing";
                    _currentlySelectedQuest.CompletionStatus = false;
                    break;
                }
            }
            foreach (Quest availableQuest in Character.Instance.AvailableQuests)
            {
                if (availableQuest.Name == _currentlySelectedQuest.Name)
                {
                    Character.Instance.MoveQuestStageStatus(_currentlySelectedQuest, "Available Quests", "Current Quests");
                    availableQuest.ProgressStatus = "Ongoing";
                    _currentlySelectedQuest.CompletionStatus = false;
                    break;
                }
            }

            _currentlySelectedQuest = null;

            Character.Instance.RefreshAllQuests();
            Character.Instance.SetupQuests();
            SetupQuestsInDiary();

            ResetFields();
        }
    }

    public void AbandonQuest()
    {
        if (_currentlySelectedQuest.ProgressStatus == "Ongoing")
        {
            Character.Instance.MoveQuestStageStatus(_currentlySelectedQuest, "Current Quests", "Available Quests");
            _currentlySelectedQuest.ProgressStatus = "Available";
            _currentlySelectedQuest.CompletionStatus = false;

            _currentlySelectedQuest = null;

            Character.Instance.RefreshAllQuests();
            Character.Instance.SetupQuests();
            SetupQuestsInDiary();

            ResetFields();
        }
    }
    
    public void ResetFields()
    {
        SelectedQuestTitle.text = string.Empty;
        SelectedQuestDescription.text = string.Empty;
        SelectedQuestStatus.text = string.Empty;

        for (int i = 0; i < SelectedQuestObjectivesDisplay.transform.childCount; i++)
        {
            Destroy(SelectedQuestObjectivesDisplay.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < SelectedQuestObjectivesDisplay.transform.childCount; i++)
        {
            Destroy(SelectedQuestObjectivesDisplay.transform.GetChild(i));
        }
    }

    public void LoadCharacterAppearance()
    {
        Sprite[] _spritesFromStorage = Resources.LoadAll<Sprite>("Clothing/New Clothing");
        if (Character.Instance != null)
        {
            foreach (Clothing clothing in Character.Instance.Wearables)
            {
                if (clothing.BodyPart == "Body" && clothing.Selected == true)
                {
                    foreach (Sprite sprite in _spritesFromStorage)
                    {
                        if (sprite.name == clothing.Name)
                        {
                            if (_bodyIcon != null)
                            {
                                _bodyIcon.sprite = sprite;
                            }
                        }
                    }
                }
                if (clothing.BodyPart == "Face" && clothing.Selected == true)
                {
                    foreach (Sprite sprite in _spritesFromStorage)
                    {
                        if (sprite.name == clothing.Name)
                        {
                            if (_faceIcon != null)
                            {
                                _faceIcon.sprite = sprite;
                            }
                        }
                    }
                }
                if (clothing.BodyPart == "Hair" && clothing.Selected == true)
                {
                    foreach (Sprite sprite in _spritesFromStorage)
                    {
                        if (sprite.name == clothing.Name)
                        {
                            if (_hairIcon != null)
                            {
                                _hairIcon.sprite = sprite;
                            }
                        }
                    }
                }
                if (clothing.BodyPart == "Top" && clothing.Selected == true)
                {
                    foreach (Sprite sprite in _spritesFromStorage)
                    {
                        if (sprite.name == clothing.Name)
                        {
                            if (_topIcon != null)
                            {
                                _topIcon.sprite = sprite;
                            }
                        }
                    }
                }
            }
        }
    }

    public void LoadCharacterDialogueAppearance()
    {
        Sprite[] _spritesFromStorage = Resources.LoadAll<Sprite>("Clothing/New Clothing");
        foreach (Clothing clothing in Character.Instance.Wearables)
        {
            if (clothing.BodyPart == "Body" && clothing.Selected == true)
            {
                foreach (Sprite sprite in _spritesFromStorage)
                {
                    if (sprite.name == clothing.Name)
                    {
                        _bodyPart.sprite = sprite;
                    }
                }
            }
            if (clothing.BodyPart == "Face" && clothing.Selected == true)
            {
                foreach (Sprite sprite in _spritesFromStorage)
                {
                    if (sprite.name == clothing.Name)
                    {
                        _facePart.sprite = sprite;
                    }
                }
            }
            if (clothing.BodyPart == "Hair" && clothing.Selected == true)
            {
                foreach (Sprite sprite in _spritesFromStorage)
                {
                    if (sprite.name == clothing.Name)
                    {
                        _hairPart.sprite = sprite;
                    }
                }
            }
            if (clothing.BodyPart == "Top" && clothing.Selected == true)
            {
                foreach (Sprite sprite in _spritesFromStorage)
                {
                    if (sprite.name == clothing.Name)
                    {
                        _topPart.sprite = sprite;
                    }
                }
            }
            if (clothing.BodyPart == "Bot" && clothing.Selected == true)
            {
                foreach (Sprite sprite in _spritesFromStorage)
                {
                    if (sprite.name == clothing.Name)
                    {
                        _botPart.sprite = sprite;
                    }
                }
            }
            if (clothing.BodyPart == "Shoes" && clothing.Selected == true)
            {
                foreach (Sprite sprite in _spritesFromStorage)
                {
                    if (sprite.name == clothing.Name)
                    {
                        _shoesPart.sprite = sprite;
                    }
                }
            }
        }
    }
    #endregion

    #region Item drops functions
    // Display the menu that gives the player the options of different actions to
    // do on that item he tapped.
    public void DisplayActionsMenu()
    {
        if (ItemActionsWindow.activeSelf == true)
        {
            ClosePopup(ItemActionsWindow);
        } else
        {
            OpenPopup(ItemActionsWindow);
        }
    }

    public void CollectItem()
    {
        Character.Instance.AddItem(ItemSelected);
        Destroy(ItemSelected.gameObject);
    }

    // Displays the selected item on the item details window if the action "view"
    // is pressed by the player.
    public void ViewItem()
    {
        Sprite sprite = Resources.Load<Sprite>("Items/Inventory/" + ItemSelected.AssetsImageName);
        ItemDetailsPortrait.sprite = sprite;
        ItemDetailsName.text = ItemSelected.Name;
        ItemDetailsDescription.text = ItemSelected.Description;
        ItemDetailsActives.text = ItemSelected.Active;

        ItemSelected.DisplayItemDetails();
    }

    public void UseItem()
    {
        // TODO: Create item actives and test them from here.
        //Debug.Log("Used " + ItemSelected.Name);
    }

    /// <summary>
    /// Map area =/= from main map. This function will load the last scene that is
    /// also considered a map area type.
    /// </summary>
    public void LoadLastMapAreaScene()
    {
        Character.Instance.LastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(Character.Instance.LastMapArea);
    }

    public void LoadScene(string scene)
    {
        if (SceneManager.GetActiveScene().name == "Tutorial Map Area" || SceneManager.GetActiveScene().name == "Castle Area" || SceneManager.GetActiveScene().name == "Jacob's House")
        {
            Character.Instance.LastMapArea = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(scene);
        } else
        {
            if (Character.Instance.LastMapArea == "Test Area")
            {
                SceneManager.LoadScene("Test Area");
            } else
            {
                Character.Instance.LastScene = scene;
                SceneManager.LoadScene(scene);
            }
        }
        Character.Instance.RefreshJsonData();
    }
    #endregion
    
    public void ResetTheGame()
    {
        if (SceneManager.GetActiveScene().name == "Beginning Character Creation")
        {
            SettingsManager.Instance.RemoveSingletonInstance();
            Character.Instance.RemoveSingletonInstance();
            SceneManagement.Instance.RemoveSingletonInstance();
            DeleteJsonDataFromStorage();
            SceneManager.LoadScene("Logo Introduction");
        }
        else
        {
            SettingsManager.Instance.RemoveSingletonInstance();
            Character.Instance.RemoveSingletonInstance();
            SceneManagement.Instance.RemoveSingletonInstance();
            DeleteJsonDataFromStorage();
            SceneManager.LoadScene("Logo Introduction");
        }
    }

    public void DeleteJsonDataFromStorage()
    {
        string path = Application.persistentDataPath;
        DirectoryInfo gameDataDirectory = new DirectoryInfo(path);
        FileInfo[] gameData = gameDataDirectory.GetFiles();

        List<FileInfo> filesToRemove = new List<FileInfo>();

        // Here we retrieve the relevant files from the game's data directory
        // and store a list of them for removal.
        foreach (FileInfo file in gameData)
        {
            string fileTypeString = string.Empty;
            for (int i = 0; i < file.Name.Length; i++)
            {
                if (file.Name[i] == '.')
                {
                    fileTypeString = file.Name.Substring(i, file.Name.Length - i);

                    if (fileTypeString == ".json")
                    {
                        filesToRemove.Add(file);
                    }
                }
            }
        }

        // Then we use the list of files to remove to delete the ones that match
        // in the game data directory.
        foreach (FileInfo file in gameData)
        {
            foreach (FileInfo fileToRemove in filesToRemove)
            {
                if (file.Name == fileToRemove.Name)
                {
                    //Debug.Log(file.Name);
                    File.Delete(path + "/" + fileToRemove.Name);
                }
            }
        }
    }
}
