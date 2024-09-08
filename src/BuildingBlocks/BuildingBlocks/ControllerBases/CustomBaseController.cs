using BuildingBlocks.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.ControllerBases;

public class CustomBaseController : ControllerBase
{
    public IActionResult CreateActionResultInstance<T>(Response<T> response) where T : class
    {
        return new ObjectResult(response)
        {
            StatusCode = response.StatusCode
        };
    }
}