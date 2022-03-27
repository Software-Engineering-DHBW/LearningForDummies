using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveData
{

    [System.Serializable]
    public class QuestionCatalogue
    {
        public string fileName;
        public List<Question> questions = new List<Question>();
    }

    [System.Serializable]
    public class Question
    {
        public string questionName;
        public List<string> answers;
        public int rightAnswerPosition;

        public Question(string _questionName, List<string> _answers, int _rightAnswerPosition)
        {
            questionName = _questionName;
            answers = _answers;
            rightAnswerPosition = _rightAnswerPosition;
        }
    }

    [System.Serializable]
    public class PlayerProfile
    {
        public string fileName = "Player";
        public string userName;
        public int profilePicture_ID = 0;
        public List<Statistic> statistics = new List<Statistic>();
    }

    [System.Serializable]
    public class Statistic
    {
        public string label;
        public int score;

        public Statistic(string _label, int _score)
        {
            label = _label;
            score = _score;
        }
    }


}