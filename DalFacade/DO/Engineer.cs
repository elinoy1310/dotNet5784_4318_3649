

namespace DO;

public record Engineer
(
    int Id,
    string? Email=null,
    double? Cost=null,
    string? Name=null,
    DO.EngineerExperience? Level=null
);
