using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class InGameSpeech : InGameBehaviour
{
    public List<SpeechData> speechList = new List<SpeechData>();
    private Text m_displayText;
    private Image m_dialogImage;
    private EnemyUnit parentUnit;
    private bool m_eventToggle;

    // Use this for initialization
    void Start()
    {
        parentUnit = GetComponentInParent<EnemyUnit>();
        m_dialogImage = GetComponent<Image>();
        m_displayText = GetComponentInChildren<Text>();
        m_eventToggle = false;
        StartCoroutine(ProcessSpeechList());
    }

    IEnumerator ProcessSpeechList()
    {
        for (int i = 0; i < speechList.Count; i++)
        {
            if (speechList[i].condition == SpeechCondition.TIME)
            {
                yield return new WaitForSeconds(speechList[i].value);
                _PlaySpeech(speechList[i].text);
            }
            else if (speechList[i].condition == SpeechCondition.HP)
            {
                while ((parentUnit.currentHp / parentUnit.maxHp) > speechList[i].value)
                {
                    yield return null;
                }
                _PlaySpeech(speechList[i].text);
            }
            else if (speechList[i].condition == SpeechCondition.EVENT)
            {
                while (!m_eventToggle)
                {
                    yield return null;
                }
                m_eventToggle = true;
                _PlaySpeech(speechList[i].text);
            }
            yield return null;
        }
    }

    private void _PlaySpeech(string text)
    {
        m_displayText.text = text;
        m_dialogImage.enabled = true;
        m_displayText.enabled = true;

    }

}

[System.Serializable]
public struct SpeechData
{
    public string text;
    public SpeechCondition condition;
    /// <summary>
    /// time value is second, hp value is float (die)0-1(full)
    /// </summary>
    public float value;
}

public enum SpeechCondition
{
    TIME = 1,
    HP = 2,
    EVENT = 3,
}
