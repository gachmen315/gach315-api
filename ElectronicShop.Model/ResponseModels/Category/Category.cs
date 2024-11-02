namespace ElectronicShop.Model.ResponseModels.Category
{
    public class Category
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }

        public CategoryWebViewModel ConvertToViewModel() => new()
        {
            Id = Id,
            Slug = Code,
            Title = Name,
        };
    }

    public class Categorys : Category
    {
        public int TotalRows { get; set; }

        public Category ConvertToViewModel() => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Description = Description,
            Status = Status
        };
    }

    public class CategoryWebViewModel
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
    }
}