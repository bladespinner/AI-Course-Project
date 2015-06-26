using MihailGospodinov.WordPrediction.Learning;
using MihailGospodinov.WordPrediction.Prediction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction
{
    public static class WordPrediction
    {
        private static IPredictionFactory _predictionFactory;
        private static ILearnerFactory _learnerFactory;
        private static IDependencyResolver _dependencyResolver;

        private static bool _initialized = false;   

        public static IDependencyResolver DependencyResolver
        {
            get
            {
                if(_dependencyResolver == null)
                {
                    _dependencyResolver = new DefaultDependencyResolver();
                }
                return _dependencyResolver;
            }
            set
            {
                _dependencyResolver = value;
            }
        }

        public static void Initialize()
        {
            if(!_initialized)
            {
                _initialized = true;
                _predictionFactory = DependencyResolver.Get<IPredictionFactory>();
                _learnerFactory = DependencyResolver.Get<ILearnerFactory>();
            }
        }

        public static void Teach(Stream contentStream)
        {
            Initialize();
            using(var learner = _learnerFactory.GetLearner())
            {
                learner.Learn(contentStream);
            }
        }

        public static List<Tuple<string,double>> Predict(string text)
        {
            Initialize();
            var predictor = _predictionFactory.GetPredictor();
            return predictor.Predict(text);
        }
    }
}
