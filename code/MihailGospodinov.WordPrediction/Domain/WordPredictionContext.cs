using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Domain
{
    public class WordPredictionContext : DbContext
    {
        public WordPredictionContext() : base("WordPredictionConnection") { }

        public DbSet<Word> Words { get; set; }
        public DbSet<FourGram> FourGrams { get; set; }
        public DbSet<ThreeGram> ThreeGrams { get; set; }
        public DbSet<DiGram> DiGrams { get; set; }
    }
}
