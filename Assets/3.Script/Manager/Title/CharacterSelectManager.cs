using System.Collections.Generic;
using UnityEngine;


public class CharacterSelectManager : MonoBehaviour, ISceneContextBuilt
{
    [Header("Character")]
    [SerializeField]
    private TitleCharacter titleCharacterPrefab;

    private List<PlayerCharacterData> playerCharacterDatas = new List<PlayerCharacterData>();
    private List<TitleCharacter> characterObjects = new List<TitleCharacter>();

    [SerializeField]
    private List<Transform> characterSpawnTransform = new List<Transform>();

    [SerializeField]
    private float characterSpawnOffsetY = 0f;

    private TitleCharacter selectedCharacter;

    // ISceneContextBuilt
    public int Priority { get; set; } = 0;

    // temp
    private void Start()
    {
        GameManager.Instance.Initialize();
    }

    public void OnSceneContextBuilt()
    {
        SetPlayerCharacterDataFromJson();
        SetPlayerCharacterDataFromSceneContext();
        SyncTitleCharacterFromCharacterData();
    }

    /// <summary>
    /// json파일로부터 남아있는 데이터를 불러 옴
    /// </summary>
    private void SetPlayerCharacterDataFromJson()
    {
        PlayerCharacterDataJson playerCharacterDataJson = PersistentDataManager.Instance.LoadFromJson();
        playerCharacterDatas = playerCharacterDataJson.data;
    }

    /// <summary>
    /// CreateCharcter씬에서 캐릭터 새로 생성 시 이 씬에 도착했을 때 SceneContext에 있는 캐릭터 데이터를 불러옴
    /// </summary>
    private void SetPlayerCharacterDataFromSceneContext()
    {
        PlayerCharacterData playerCharacterData = GameManager.Instance.CurrentSceneContext.PlayerCharacterData;

        if(playerCharacterData != null)
        {
            if (!playerCharacterDatas.Exists(data => data.Key == playerCharacterData.Key))
            {
                playerCharacterDatas.Add(playerCharacterData);

                PlayerCharacterDataJson jsonDatas = new PlayerCharacterDataJson();
                jsonDatas.data = playerCharacterDatas;
                PersistentDataManager.Instance.SaveToJson(jsonDatas);
            }
        }
    }

    private void SetSelectedCharacter(TitleCharacter selectedCharacter)
    {
        this.selectedCharacter = selectedCharacter;

        foreach(var character in characterObjects)
        {
            if(selectedCharacter.PlayerCharacterData.Key == character.PlayerCharacterData.Key)
            {
                character.SetOutlineSize(true);
            }
            else
            {
                character.SetOutlineSize(false);
            }
        }
    }

    #region using in buttons
    // 캐릭터를 선택하고 게임씬으로 화면 전환
    public void SelectCharacter()
    {
        if (selectedCharacter == null)
            return;

        SceneChangeManager.Instance.ChangeScene(SceneType.GameScene, selectedCharacter.PlayerCharacterData);
    }

    public void ChangeCreateCharacterScene()
    {
        //TODO : 최대 캐릭터 생성개수 제한
        SceneChangeManager.Instance.ChangeScene(SceneType.CharacterCreateScene);
        SyncTitleCharacterFromCharacterData();
    }

    public void DeleteCharacter(TitleCharacter titleCharacter)
    {
        playerCharacterDatas.Remove(titleCharacter.PlayerCharacterData);
        SyncTitleCharacterFromCharacterData();
    }

    #endregion

    #region visual
    private void SyncTitleCharacterFromCharacterData()
    {
        foreach (var character in characterObjects)
        {
            Destroy(character.gameObject);
        }
        characterObjects.Clear();

        for (int i = 0; i < playerCharacterDatas.Count; i++)
        {
            TitleCharacter titleCharacter = Instantiate(titleCharacterPrefab, characterSpawnTransform[i]);
            titleCharacter.transform.localPosition += new Vector3(0, characterSpawnOffsetY, 0);
            titleCharacter.ChosenTitleCharacter -= SetSelectedCharacter;
            titleCharacter.ChosenTitleCharacter += SetSelectedCharacter;
            titleCharacter.Initialize(playerCharacterDatas[i]);

            characterObjects.Add(titleCharacter);
        }
    }
    #endregion
}
