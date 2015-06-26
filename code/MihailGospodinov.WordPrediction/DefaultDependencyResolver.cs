using MihailGospodinov.WordPrediction.Learning;
using MihailGospodinov.WordPrediction.Prediction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MihailGospodinov.WordPrediction
{
    internal class DefaultDependencyResolver : IDependencyResolver
    {
        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(IDependencyResolver))
            {
                return new DefaultDependencyResolver() as T;
            }
            if (typeof(T) == typeof(ILearnerFactory))
            {
                return new EntityLearnerFactory() as T;
            }
            if (typeof(T) == typeof(IPredictionFactory))
            {
                return new FourGramPredictionFactory() as T;
            }
            return null;
        }
    }
}
