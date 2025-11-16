using UnityEngine;
using UnityEngine.UI;

public class CharacterUIText : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private Vector3 offset = Vector3.zero;

    public void SetTextUIToWorldObj(Vector3 worldPosition, string str)
    {
        text.text = str;
        transform.position = Camera.main.WorldToScreenPoint(worldPosition) + offset;
    }
}
