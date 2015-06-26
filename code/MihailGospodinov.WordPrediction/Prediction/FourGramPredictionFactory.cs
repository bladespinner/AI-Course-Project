using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Prediction
{
    public class FourGramPredictionFactory : IPredictionFactory
    {
        public IWordPredictor GetPredictor()
        {
            return new FourGramPredictor();
        }
    }
}
