using UnityEngine;

public class CharacterCreateSceneManager : MonoBehaviour
{
    [SerializeField]
    private SignBoard signBoard;

    private StatusData statusData;

    private void Start()
    {
        statusData = new StatusData(true);
        signBoard.SetCurrentStatusData(statusData);
        statusData.SetRandomStatus();

        signBoard.SetStatusText();
    }

    public void BackToSelectCharacterScene(bool isCreatedCharacter)
    {
        string name = signBoard.GetNameFieldText();
        if (name.Length == 0)
            return;

        if (!isCreatedCharacter)
        {
            SceneChangeManager.Instance.ChangeScene(SceneType.CharacterSelectScene);
        }
        else if(1 < Constants.MaxPlayerCreateCount)
        {
            PlayerCharacterData playerCharacterData = new PlayerCharacterData(name, statusData);
            SceneChangeManager.Instance.ChangeScene(SceneType.CharacterSelectScene, playerCharacterData);
        }
    }
}
