using Newtonsoft.Json;
using System.Net;
using Yorf.Results.Core;

namespace Yorf.Results.AspNetCore;

public static class HttpResponseExtensions
{
    public static async Task<Result> GetResult(this HttpResponseMessage httpResponse, CancellationToken cancellationToken)
    {
        var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        if (httpResponse.IsSuccessStatusCode)
        {
            return Result.Success();
        }

        return ProcessErrorResult(responseContent, httpResponse.StatusCode);
    }

    public static async Task<Result<T>> GetResult<T>(this HttpResponseMessage httpResponse, CancellationToken cancellationToken) where T : class
    {
        var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        if (httpResponse.IsSuccessStatusCode)
        {
            var payment = JsonConvert.DeserializeObject<T>(responseContent);
            return Result.Success(payment!);
        }

        return ProcessErrorResult(responseContent, httpResponse.StatusCode);

    }

    private static Result ProcessErrorResult(string responseContent, HttpStatusCode httpStatusCode)
    {
        switch (httpStatusCode)
        {
            case HttpStatusCode.NotFound:
                return Result.NotFound();
            case HttpStatusCode.BadRequest:
                {
                    //parse model state error
                    if (!string.IsNullOrWhiteSpace(responseContent))
                    {
                        ValidationError[]? validationErrors = Array.Empty<ValidationError>();

                        var badRequest = JsonConvert.DeserializeObject<ApiBadRequestResponse>(responseContent);

                        if (badRequest is null || badRequest.Errors.Count == 0)
                        {
                            var errors = JsonConvert.DeserializeObject<Dictionary<string, IList<string>>>(responseContent);

                            validationErrors = errors?.Select(x => new ValidationError(x.Key, x.Value.FirstOrDefault() ?? "This field was invalid")).ToArray();
                        }
                        else
                        {
                            validationErrors = badRequest?.Errors.Select(x => new ValidationError(x.Key, x.Value.FirstOrDefault() ?? "This field was invalid")).ToArray();
                        }

                        return Result.Invalid(validationErrors!);
                    }
                    else
                    {
                        return Result.Error($"An error occured in the Gateway API. Status ({httpStatusCode})");
                    }

                }

            default:
                return Result.Error($"Gateway API Error ({httpStatusCode}): {responseContent}");
        }
    }
}