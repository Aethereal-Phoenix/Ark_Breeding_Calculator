using Calculator_API.Data;

namespace Calculator_API.Models
{
    public class DinosaurData
    {
        public Guid DinosaurId { get; set; } // PK
        public Guid BreedingLineId { get; set; } // FKf
        public string DinosaurName { get; set; }
        public Guid FatherId { get; set; }
        public Guid MotherId { get; set; }
        public SpeciesEnum Species { get; set; }
        public GenderEnum Gender { get; set; }
        public bool Mutated { get; set; }
        public Stats Stats { get; set; }
        public Stats MutatedStates { get; set; }
        public GenerationsEnum GenerationType { get; set; }
    }
}
