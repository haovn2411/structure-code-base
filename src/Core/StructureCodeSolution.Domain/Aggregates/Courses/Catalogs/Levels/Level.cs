using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Catalogs.Levels
{
    public class Level : EntityAuditBase<int>
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public int Order { get; private set; }
        public bool IsActive { get; private set; }

        private Level() { }

        private Level(string name, string? description, int order)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Level name cannot be empty", nameof(name));

            if (order < 0)
                throw new ArgumentException("Level order cannot be negative", nameof(order));

            Name = name;
            Description = description;
            Order = order;
            IsActive = true;
        }

        public static Level Create(string name, string? description, int order = 0)
        {
            return new Level(name, description, order);
        }

        public void Update(string name, string? description, int order)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Level name cannot be empty", nameof(name));

            if (order < 0)
                throw new ArgumentException("Level order cannot be negative", nameof(order));

            Name = name;
            Description = description;
            Order = order;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}