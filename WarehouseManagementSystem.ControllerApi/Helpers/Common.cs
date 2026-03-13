using Microsoft.AspNetCore.Mvc;

namespace PickAPile.Helpers
{
    public class Common
    {
        public static IActionResult PostMan(object? data, int status, string message)
        {
            if (status >= 400)
            {
                return Error(status, message);
            }
            else
            {
                return Success(data, status, message);
            }
        }

        public static IActionResult Success(object? data, int status, string message)
        {
            return new JsonResult(new
            {
                success = true,
                message = message,
                data = data
            })
            {
                StatusCode = status
            };
        }

        public static IActionResult Error(int status, string message)
        {
            return new JsonResult(new
            {
                success = false,
                message = message,
                data = (object?)null
            })
            {
                StatusCode = status
            };
        }

        public static string? Validate(object request, Dictionary<string, string> rules)
        {
            return null;
        }
    }
}
