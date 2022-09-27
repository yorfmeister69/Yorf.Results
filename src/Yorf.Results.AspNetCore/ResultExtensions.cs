using Microsoft.AspNetCore.Mvc;
using System.Text;
using Yorf.Results.Core;

namespace Yorf.Results.AspNetCore;
public static class ResultExtensions
{
    public static IActionResult Handle(this IResult result, Controller controller)
        => ConvertToActionResult(controller, result);

    public static IActionResult Handle(this IResult result, Controller controller, Func<object?, IActionResult> handler)
        => ConvertToActionResult(controller, result, handler);

    public static IActionResult Handle<TModel>(this IResult result, Controller controller, Func<TModel?, IActionResult> handler)
       => ConvertToActionResult(controller, result, handler);

    private static IActionResult ConvertToActionResult(Controller controller, IResult result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => typeof(Result).IsInstanceOfType(result)
                ? controller.Ok() : new OkObjectResult(result.GetValue()),

            ResultStatus.NotFound => controller.NotFound(),
            ResultStatus.Unauthorized => controller.Unauthorized(),
            ResultStatus.Forbidden => controller.Forbid(),
            ResultStatus.Invalid => BadRequest(controller, result),
            ResultStatus.Error => UnprocessableEntity(controller, result),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };
    }

    private static IActionResult ConvertToActionResult(Controller controller, IResult result, Func<object?, IActionResult> handler)
    {
        return result.Status switch
        {
            ResultStatus.Ok => typeof(Result).IsInstanceOfType(result)
                  ? controller.Ok() : handler(result.GetValue()),

            ResultStatus.NotFound => controller.NotFound(),
            ResultStatus.Unauthorized => controller.Unauthorized(),
            ResultStatus.Forbidden => controller.Forbid(),
            ResultStatus.Invalid => BadRequest(controller, result),
            ResultStatus.Error => UnprocessableEntity(controller, result),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };
    }

    private static IActionResult ConvertToActionResult<TModel>(Controller controller, IResult result, Func<TModel?, IActionResult> handler)
    {
        return result.Status switch
        {
            ResultStatus.Ok => typeof(Result).IsInstanceOfType(result)
                  ? controller.Ok() : handler((TModel?)result.GetValue()),

            ResultStatus.NotFound => controller.NotFound(),
            ResultStatus.Unauthorized => controller.Unauthorized(),
            ResultStatus.Forbidden => controller.Forbid(),
            ResultStatus.Invalid => BadRequest(controller, result),
            ResultStatus.Error => UnprocessableEntity(controller, result),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };
    }

    private static ActionResult BadRequest(Controller controller, IResult result)
    {
        foreach (var error in result.ValidationErrors)
        {
            controller.ModelState.AddModelError(error.Identifier, error.ErrorMessage);
        }

        return controller.BadRequest(controller.ModelState);
    }

    private static ActionResult UnprocessableEntity(Controller controller, IResult result)
    {
        var details = new StringBuilder("Next error(s) occured:");

        foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

        var problemModel = new ProblemDetails
        {
            Title = "Something went wrong.",
            Detail = details.ToString()
        };

        bool isAjax = controller.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        return isAjax ? controller.UnprocessableEntity(problemModel) : controller.View("Problem", problemModel);
    }

    public static string GetErrorMessage(this IResult result)
    {
        var faultMessage = new StringBuilder("Event Faults:: ");

        switch (result.Status)
        {
            case ResultStatus.Error:
                {
                    faultMessage.Append("(Errors): ");
                    foreach (var error in result.Errors)
                    {
                        faultMessage.Append("* ").AppendLine(error);
                    }
                }
                break;
            case ResultStatus.Forbidden:
                {
                    faultMessage.Append("(Forbidden)");
                }
                break;
            case ResultStatus.Unauthorized:
                {
                    faultMessage.Append("(Unauthorized)");
                }
                break;
            case ResultStatus.Invalid:
                {
                    faultMessage.Append("(Validation Failure): ");
                    foreach (var validationError in result.ValidationErrors)
                    {
                        faultMessage.Append("* ").AppendLine($"[{validationError.Identifier}] => {validationError.ErrorMessage}");
                    }
                }
                break;
            case ResultStatus.NotFound:
                {
                    faultMessage.Append("(NotFound)");
                }
                break;
            default:
                break;
        }

        return faultMessage.ToString();
    }
}