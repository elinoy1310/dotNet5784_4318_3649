

namespace DO;

public record Engineer
(
    int Id,
    EngineerExperience Level,
    string? Email=null,
    double? Cost=null,
    string? Name=null
);
