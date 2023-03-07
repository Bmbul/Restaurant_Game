using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    [Multiline]public string question;
    public string[] answers;
    public int rightAnswerIndex;

    public Question(string _question, string[] _answers, int _rightAnswerIndex)
    {
        question = _question;
        //change it here
        answers = _answers;
        rightAnswerIndex = _rightAnswerIndex;
    }
}
