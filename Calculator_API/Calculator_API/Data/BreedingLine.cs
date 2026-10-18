namespace Calculator_API.Data
{
    public class BreedingLine
    {
        public int BreedingLineId { get; set; } // PK
        public SpeciesEnum Species { get; set; }
        public ICollection<Dinosaur> Dinosaurs { get; set; } = new List<Dinosaur>();
        public string BreedingLineName { get; set; } = string.Empty;
    }
}
