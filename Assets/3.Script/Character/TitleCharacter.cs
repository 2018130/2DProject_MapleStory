using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleCharacter : Character
{
    private SpriteRenderer characterSpriteRenderer;

    [SerializeField]
    private CharacterUIText characterNameImagePrefab;
    private CharacterUIText characterNameImage;

    [SerializeField]
    private Material outlineMaterial;
    private Material defaultMaterial;

    public Action<TitleCharacter> ChosenTitleCharacter;

    private void Awake()
    {
        characterSpriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = characterSpriteRenderer.material;
    }
    public void Initialize(PlayerCharacterData playerCharacterData)
    {
        if(characterData == null)
        {
            characterData = playerCharacterData;
        }

        if (characterNameImage == null)
        {
            characterNameImage = Instantiate(characterNameImagePrefab, GameManager.Instance.CurrentSceneContext.MainCanvas.transform);
        }

        characterNameImage.SetTextUIToWorldObj(transform.position, characterData.characterName);
    }

    private void ChooseCharacter()
    {
        SetMaterial(outlineMaterial);
        ChosenTitleCharacter?.Invoke(this);
    }

    private void SetMaterial(Material material)
    {
        characterSpriteRenderer.material = material;
    }

    private void OnMouseUp()
    {
        ChooseCharacter();
    }
}
