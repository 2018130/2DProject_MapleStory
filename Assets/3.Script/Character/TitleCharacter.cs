using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleCharacter : PlayerCharacter
{
    public Action<string> ChosenTitleCharacter;

    private SpriteOutline spriteOutline;

    protected override void Awake()
    {
        spriteOutline = GetComponent<SpriteOutline>();
        SetOutlineSize(false);
    }
    protected override void Update()
    {

    }

    public void Initialize(PlayerCharacterData playerCharacterData)
    {
        if(base.playerCharacterData.Key.Length == 0)
        {
            base.playerCharacterData = playerCharacterData;
        }

        if (characterNameImage == null)
        {
            characterNameImage = Instantiate(characterNameImagePrefab, GameManager.Instance.CurrentSceneContext.Canvas.transform);
        }

        characterNameImage.SetTextUIToWorldObj(transform.position, playerCharacterData.CharacterName);
    }

    private void ChooseCharacter()
    {
        SetOutlineSize(true);
        ChosenTitleCharacter?.Invoke(playerCharacterData.Key);
    }

    public void SetOutlineSize(bool choosed)
    {
        if(choosed)
        {
            spriteOutline.outlineSize = 2;
        }
        else
        {
            spriteOutline.outlineSize = 0;
        }
    }

    private void OnMouseUp()
    {
        ChooseCharacter();
    }
}
