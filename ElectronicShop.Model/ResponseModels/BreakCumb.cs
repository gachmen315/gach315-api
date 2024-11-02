namespace ElectronicShop.Model.ResponseModels
{
    public class BreakCumb
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
    }

    public class BreakCumbWebViewModek
    {
        public IEnumerable<BreakCumb> BreakCumb { get; set; } = Enumerable.Empty<BreakCumb>();
        public IEnumerable<BreakCumb> ProductCategory { get; set; } = Enumerable.Empty<BreakCumb>();
    }
}