using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Learning
{
    public interface IWordPredictionLearner : IDisposable
    {
        void Learn(Stream words);
    }
}
