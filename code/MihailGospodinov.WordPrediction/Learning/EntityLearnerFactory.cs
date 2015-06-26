using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Learning
{
    public class EntityLearnerFactory : ILearnerFactory
    {
        public IWordPredictionLearner GetLearner()
        {
            return new EntityLearner();
        }
    }
}
