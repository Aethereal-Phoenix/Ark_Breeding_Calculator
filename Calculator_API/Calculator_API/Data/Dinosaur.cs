namespace Calculator_API.Data
{
    public class Dinosaur
    {
        public Guid DinosaurId { get; set; } // PK
        public Guid BreedingLineId { get; set; } // FK
        public string DinosaurName { get; set; } = string.Empty;
        public SpeciesEnum Species { get; set; }
        public GenderEnum Gender { get; set; }
        public GenerationsEnum GenerationType { get; set; }
        public Stats Stats { get; set; } = new Stats();
        public MutatedStats MutatedStats { get; set; } = new MutatedStats();
        public bool Mutated { get; set; }
        public bool AquaticDino { get; set; }
        public Guid FatherId { get; set; }
        public Guid MotherId { get; set; }
    }
}
