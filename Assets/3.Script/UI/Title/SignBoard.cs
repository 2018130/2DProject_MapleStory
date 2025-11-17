using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SignBoard : MonoBehaviour
{
    [SerializeField]
    private StatusData statusData;

    [SerializeField]
    private InputField nameField;
    [SerializeField]
    private Text strText;
    [SerializeField]
    private Text dexText;
    [SerializeField]
    private Text intText;
    [SerializeField]
    private Text lukText;

    [SerializeField]
    private RectTransform diceTransform; 


    private bool isRolling = false;

    public string GetNameFieldText()
    {
        return nameField.text;
    }

    public void SetCurrentStatusData(StatusData statusData)
    {
        this.statusData = statusData;
    }

    public void RollingDice()
    {
        if(isRolling)
            return;

        StartCoroutine(RollingDice_co());
    }

    private IEnumerator RollingDice_co()
    {
        isRolling = true;
        float timer = 0f;
        float rollDuration = 1f;
        float flipInterval = 0.1f;

        while(timer < rollDuration)
        {
            timer += Time.deltaTime + flipInterval;

            int sign = diceTransform.localScale.x > 0 ? -1 : 1;
            diceTransform.localScale = new Vector3(sign, 1, 1);
            yield return new WaitForSeconds(flipInterval);
        }

        statusData.SetRandomStatus();
        SetStatusText();

        isRolling = false;
    }

    public void SetStatusText()
    {
        strText.text = statusData.STR.ToString();
        dexText.text = statusData.DEX.ToString();
        intText.text = statusData.INT.ToString();
        lukText.text = statusData.LUK.ToString();
    }
}
