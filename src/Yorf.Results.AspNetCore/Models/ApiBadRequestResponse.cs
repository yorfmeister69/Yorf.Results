namespace Yorf.Results.AspNetCore;

internal sealed class ApiBadRequestResponse
{
    public string? TraceId { get; set; }
    public string? Title { get; set; }
    public Dictionary<string, IList<string>> Errors { get; set; } = new Dictionary<string, IList<string>>();
}