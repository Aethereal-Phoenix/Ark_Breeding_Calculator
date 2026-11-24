using Calculator_API.Data;
using System.ComponentModel.DataAnnotations;

namespace Calculator_API.Models
{
    public class DinosaurRegistrationModel
    {
        [Required]
        public int BreedingLineId { get; set; }

        [Required]
        public string DinosaurName { get; set; } = string.Empty;

        [Required]
        public SpeciesEnum Species { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [Required]
        public GenerationsEnum Generation { get; set; }

        [Required]
        public Stats Stats { get; set; } = null!;

        [Required]
        public MutatedStats MutatedStats { get; set; } = null!;

        public bool Mutated { get; set; }

        public bool AquaticDino { get; set; }

        public int? FatherId { get; set; }

        public int? MotherId { get; set; }
    }
}
