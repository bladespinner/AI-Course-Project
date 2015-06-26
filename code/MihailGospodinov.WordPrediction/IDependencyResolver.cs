using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MihailGospodinov.WordPrediction
{
    public interface IDependencyResolver
    {
        T Get<T>() where T : class;
    }
}
