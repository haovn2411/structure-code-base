using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Catalogs.Categories
{
    public class Category : EntityAuditBase<int>
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string? IconCode { get; private set; }
        public bool IsActive { get; private set; }

        private Category() { }

        private Category(string name, string? description, string? iconCode = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty", nameof(name));

            Name = name;
            Description = description;
            IconCode = iconCode;
            IsActive = true;
        }

        public static Category Create(string name, string? description, string? iconCode = null)
        {
            return new Category(name, description, iconCode);
        }

        public void Update(string name, string? description, string? iconCode = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty", nameof(name));

            Name = name;
            Description = description;
            IconCode = iconCode;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}