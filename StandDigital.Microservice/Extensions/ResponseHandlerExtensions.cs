using ErrorOr;

namespace StandDigital.Microservice.Extensions;

public static class ResponseHandlerExtensions
{
    public static IResult ResponseHandler<T>(ErrorOr<T> result)
     => result.IsError ?
     ResponseHandler(result.FirstError) :
     Results.Ok(result.Value);

    public static IResult ResponseHandler<T>(T result) where T : class?
        => Results.Ok(result);
  
    public static IResult ResponseHandler(Error error)
        => error.Type switch
        {
            ErrorType.Failure => Results.Problem(error.Description),
            ErrorType.Validation => Results.BadRequest(error.Description),
            ErrorType.NotFound => Results.NotFound(error.Description),
            ErrorType.Conflict => Results.Conflict(error.Description),
            _ => Results.Problem(error.Description)
        };

    public static IResult ResponseHandler<T>(IEnumerable<T> obj)
        => obj.Any() ? Results.Ok(obj) : Results.NoContent();

}
