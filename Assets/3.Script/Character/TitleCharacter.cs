using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleCharacter : PlayerCharacter
{
    [SerializeField]
    private CharacterUIText characterNameImagePrefab;
    private CharacterUIText characterNameImage;

    public Action<TitleCharacter> ChosenTitleCharacter;

    private SpriteOutline spriteOutline;

    protected override void Awake()
    {
        spriteOutline = GetComponent<SpriteOutline>();
        SetOutlineSize(false);
    }

    public void Initialize(PlayerCharacterData playerCharacterData)
    {
        if(base.playerCharacterData == null)
        {
            base.playerCharacterData = playerCharacterData;
        }

        if (characterNameImage == null)
        {
            characterNameImage = Instantiate(characterNameImagePrefab, GameManager.Instance.CurrentSceneContext.MainCanvas.transform);
        }

        characterNameImage.SetTextUIToWorldObj(transform.position, base.playerCharacterData.characterName);
    }

    private void ChooseCharacter()
    {
        SetOutlineSize(true);
        ChosenTitleCharacter?.Invoke(this);
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
