namespace Calculator_API.Data
{
    public class Dinosaur
    {
        public Guid DinosaurId { get; set; } // PK
        public Guid BreedingLineId { get; set; } // FK
        public BreedingLine BreedingLine { get; set; } = null!;
        public string DinosaurName { get; set; }
        public SpeciesEnum Species { get; set; }
        public GenderEnum Gender { get; set; }
        public bool Mutated { get; set; }
        public Stats MutatedStates { get; set; }
        public GenerationsEnum GenerationType { get; set; }
    }
}
