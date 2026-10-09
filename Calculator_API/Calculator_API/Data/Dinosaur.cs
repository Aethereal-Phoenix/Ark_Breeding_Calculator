namespace Calculator_API.Data
{
    public class Dinosaur
    {
        public Guid DinosaurId { get; set; } // PK
        public Guid BreedingLineId { get; set; } // FK
        public string DinosaurName { get; set; }
        public SpeciesEnum Species { get; set; }
        public GenderEnum Gender { get; set; }
        public bool Mutated { get; set; }
        public bool AquaticDino { get; set; }
        public Guid FatherId { get; set; }
        public Guid MotherId { get; set; }
        public MutatedStats MutatedStates { get; set; }
        public Stats Stats { get; set; }
        public GenerationsEnum GenerationType { get; set; }
    }
}
