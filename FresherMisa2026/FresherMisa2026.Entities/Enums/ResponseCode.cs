using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Entities.Enums
{
    public enum ResponseCode
    {
        Success = 200,
        Created = 201,
        BadRequest = 400,
        NotFound = 404,
        InternalServerError = 500
    }
}
