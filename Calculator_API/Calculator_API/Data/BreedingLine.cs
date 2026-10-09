namespace Calculator_API.Data
{
    public class BreedingLine
    {
        public Guid BreedingLineId { get; set; } = Guid.NewGuid(); // PK
        public SpeciesEnum Species { get; set; }
        public ICollection<Dinosaur> Dinosaurs { get; set; } = new List<Dinosaur>();
        public string BreedingLineName { get; set; } = string.Empty;
    }
}
