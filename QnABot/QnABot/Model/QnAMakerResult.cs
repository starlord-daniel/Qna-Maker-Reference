using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QnABot.Model
{
    public class QnAMakerResult
    {
        public Answer[] answers { get; set; }

        public class Answer
        {
            public string answer { get; set; }
            public string[] questions { get; set; }
            public float score { get; set; }
        }
    }
}