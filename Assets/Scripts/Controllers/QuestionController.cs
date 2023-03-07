using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OperationType {Addition, Substration, Multiplication, Division}
public class QuestionController : MonoBehaviour
{
    public static QuestionController Instance;
    internal OperationType[] operations;
    internal OperationType currentOperation;
    int operationsLenght;
    List<int> answersList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);

        operationsLenght = 0;
        for(int  i = 0; i < 4; i ++)
        {
            if (StartingSceneController.Instance.operationsSelected[i] == true)
                operationsLenght++;
        }
        operations = new OperationType[operationsLenght];
        int operationIndex = 0;

        for (int i = 0; i < 4; i++)
        {
            if (StartingSceneController.Instance.operationsSelected[i] == true)
            {
                operations[operationIndex] = (OperationType)i;
                operationIndex++;
            }
        }
    }

    public Question GetRandomQuestion(int _minAdd, int _maxAdd, int _minMult, int _maxMult, int _minDiv, int _maxDiv,int _minAns, int _maxAns)
    {
        int randIndex = Random.Range(0, operationsLenght);
        currentOperation = operations[randIndex];
        int first, second, rightAnswer = 0;
        string questionString = string.Empty;
        string[] answers = new string[4];
        int rightAnswerIndex = Random.Range(0, 4);
        answersList = new List<int>();

        switch (currentOperation)
        {
            case OperationType.Addition:
                first = Random.Range(_minAdd,_maxAdd);
                second = Random.Range(_minAdd, _maxAdd);
                rightAnswer = first + second;
                questionString = $"{first} + {second}";
                break;

            case OperationType.Substration:
                first = Random.Range(_minAdd, _maxAdd);
                second = Random.Range(0, _maxAdd);
                if (first > second)
                {
                    rightAnswer = first - second;
                    questionString = $"{first} - {second}";
                }
                else
                {
                    rightAnswer = second - first;
                    questionString = $"{second} - {first}";
                }
                break;

            case OperationType.Multiplication:
                first = Random.Range(_minMult, _maxMult);
                second = Random.Range(_minMult, _maxMult);
                rightAnswer = first * second;
                questionString = $"{first} x {second}";
                break;

            case OperationType.Division:
                first = Random.Range(_minDiv, _maxDiv);
                rightAnswer = Random.Range(_minAns, _maxAns);
                second = first * rightAnswer;
                questionString = $"{second} ÷ {first}";
                break;
        }

        answers[rightAnswerIndex] = $"{rightAnswer}";
        answersList.Add(rightAnswer);
        for(int i = 0; i < answers.Length; i++)
        {
            if (i == rightAnswerIndex)
                continue;
            int randomNumber = 0;
            while(answersList.Contains(rightAnswer + randomNumber))
                randomNumber = Random.Range(-6, 6);
            answers[i] = $"{rightAnswer + randomNumber}";
            answersList.Add(rightAnswer + randomNumber);
        }

        Question question = new Question(questionString, answers, rightAnswerIndex);
        return question;
    }

    internal Question CreateRandomQuestion()
    {
        if (StartingSceneController.Instance.GameDifficulty == Difficulties.easy)
            return GetRandomQuestion(1, 20, 1, 10, 2, 6, 1, 20);
        else
            return GetRandomQuestion(-500,500,-30,30,-10,10,-50, 50);
    }
}
