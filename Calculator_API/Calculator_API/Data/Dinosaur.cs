namespace Calculator_API.Data
{
    public class Dinosaur
    {
        public int DinosaurId { get; set; } // PK
        public int BreedingLineId { get; set; } // FK
        public BreedingLine BreedingLine { get; set; }
        public string DinosaurName { get; set; } = string.Empty;
        public SpeciesEnum Species { get; set; }
        public GenderEnum Gender { get; set; }
        public GenerationsEnum Generation { get; set; }
        public int StatsId { get; set; } // FK
        public Stats Stats { get; set; } = null!;
        public int MutatedStatsId { get; set; } // FK
        public MutatedStats MutatedStats { get; set; } = null!;
        public bool Mutated { get; set; }
        public bool AquaticDino { get; set; }
        public int? FatherId { get; set; } = null; // FK
        public Dinosaur? Father { get; set; }
        public int? MotherId { get; set; } = null; // FK
        public Dinosaur? Mother { get; set; }
    }
}
