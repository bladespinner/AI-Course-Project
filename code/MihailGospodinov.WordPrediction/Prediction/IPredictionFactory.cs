using MihailGospodinov.WordPrediction.Prediction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Prediction
{
    public interface IPredictionFactory
    {
        IWordPredictor GetPredictor();
    }
}
