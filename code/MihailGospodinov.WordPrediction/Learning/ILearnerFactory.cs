using MihailGospodinov.WordPrediction.Prediction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Learning
{
    public interface ILearnerFactory
    {
        IWordPredictionLearner GetLearner();
    }
}
