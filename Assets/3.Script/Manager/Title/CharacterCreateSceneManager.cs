using UnityEngine;

public class CharacterCreateSceneManager : MonoBehaviour
{
    [SerializeField]
    private SignBoard signBoard;

    private StatusData statusData;

    private void Start()
    {
        statusData = new StatusData();
        signBoard.SetCurrentStatusData(statusData);
        statusData.SetRandomStatus();

        signBoard.SetStatusText();
    }

    public void BackToSelectCharacterScene(bool isCreatedCharacter)
    {
        string name = signBoard.GetNameFieldText();
        if(name.Length == 0 || PersistentDataManager.Instance.LoadFromJson().data.Count >= Constants.MaxPlayerCreateCount)
            return;

        

        if (!isCreatedCharacter)
        {
            SceneChangeManager.Instance.ChangeScene(SceneType.CharacterSelectScene);
        }
        else
        {
            PlayerCharacterData playerCharacterData = new PlayerCharacterData(name, statusData);
            SceneChangeManager.Instance.ChangeScene(SceneType.CharacterSelectScene, playerCharacterData);
        }

    }
}
