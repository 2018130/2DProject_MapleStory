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

    public int Priority { get; set; } = 0;
    public void OnSceneContextBuilt()
    {
        SetPlayerCharacterDataFromSceneContext();
        SyncTitleCharacterFromCharacterData();
    }

    /// <summary>
    /// CreateCharcter씬에서 캐릭터 새로 생성 시 이 씬에 도착했을 때 SceneContext에 있는 캐릭터 데이터를 불러옴
    /// </summary>
    private void SetPlayerCharacterDataFromSceneContext()
    {
        PlayerCharacterData playerCharacterData = GameManager.Instance.CurrentSceneContext.PlayerCharacterData;
        Debug.Log("1111");
        if(playerCharacterData != null)
        {
            Debug.Log("2222");
            if (!playerCharacterDatas.Exists(data => data.Key == playerCharacterData.Key))
            {
                Debug.Log("3333");
                playerCharacterDatas.Add(playerCharacterData);
            }
        }
    }

    #region using in buttons
    public void SelectCharacter(TitleCharacter titleCharacter)
    {
        SceneChangeManager.Instance.ChangeScene(SceneType.GameScene, titleCharacter.CharacterData);
    }

    public void ChangeCreateCharacterScene()
    {
        //TODO : 최대 캐릭터 생성개수 제한
        SceneChangeManager.Instance.ChangeScene(SceneType.CharacterCreateScene);
        SyncTitleCharacterFromCharacterData();
    }

    public void DeleteCharacter(TitleCharacter titleCharacter)
    {
        playerCharacterDatas.Remove(titleCharacter.CharacterData);
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
            titleCharacter.Initialize(playerCharacterDatas[i]);

            characterObjects.Add(titleCharacter);
        }
    }

    #endregion
}
