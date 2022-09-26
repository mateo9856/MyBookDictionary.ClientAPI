using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Model.Enums
{
    public enum StatusDescriptions : uint
    {
        [Description("Data downloaded")]
        OK = 200,
        [Description("Incorrect server proxy")]
        BadGateway = 502,
        [Description("Bad requested data")]
        BadRequest = 400,
        [Description("Server error")]
        InternalServerError = 500,
        [Description("Values without content")]
        NoContent = 204,
        [Description("Zbyt duża ilośc wywołań do serwera!")]
        TooManyRequests = 429

    }
}
