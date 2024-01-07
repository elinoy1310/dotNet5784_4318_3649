

namespace DO;

public record Engineer
(
    int Id,
    DO.EngineerExperience? Level,
    string? Email=null,
    double? Cost=null,
    string? Name=null
);
