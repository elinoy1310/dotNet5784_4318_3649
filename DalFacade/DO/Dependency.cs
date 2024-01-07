

namespace DO;

public record Dependency
(
    int Id,
    int Dependent,
    int DependsOnTask
);
