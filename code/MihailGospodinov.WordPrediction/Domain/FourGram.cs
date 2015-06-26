using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Domain
{
    public class FourGram
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Index("FirstWord")]
        [MaxLength(50)]
        public string FirstWord { get; set; }
        [Index("SecondWord")]
        [MaxLength(50)]
        public string SecondWord { get; set; }
        [Index("ThirdWord")]
        [MaxLength(50)]
        public string ThirdWord { get; set; }
        [Index("FourthWord")]
        [MaxLength(50)]
        public string FourthWord { get; set; }
        public int Count { get; set; }
        public double Probability { get; set; }
    }
}
