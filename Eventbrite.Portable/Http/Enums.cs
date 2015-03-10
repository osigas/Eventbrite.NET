using System;

namespace EventbriteNET.Http
{
    ///<summary>
    /// Types of parameters that can be added to requests
    ///</summary>
    public enum ParameterType
    {
        GetOrPost,
        UrlSegment,
        QueryString
    }
}
