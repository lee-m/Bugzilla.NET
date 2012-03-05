//Copyright (C) 2012 by Lee Millward

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;
using CookComputing.XmlRpc;

namespace Bugzilla.Proxies.Bugzilla.Responses
{
  /// <summary>
  /// Represents the return value from Bugzilla.time web service method.
  /// </summary>
  internal struct GetTimeResponse
  {
    /// <summary>
    /// The current time in UTC, according to the Bugzilla database server.
    /// </summary>
    [XmlRpcMember("db_time")]
    public DateTime DatabaseServerTime;

    /// <summary>
    /// This is the current time in UTC, according to Bugzilla's web server.
    /// </summary>
    [XmlRpcMember("web_time")]
    public DateTime WebServerTime;

    /// <summary>
    /// Exists only for backwards compatability. Identical to <see cref="WebServerTime"/>
    /// </summary>
    [XmlRpcMember("web_time_utc")]
    public DateTime WebServerTimeUTC;
    
    /// <summary>
    /// Exists only for backwards compatability, always returns "UTC".
    /// </summary>
    [XmlRpcMember("tz_name")]
    public string TimeZoneName;

    /// <summary>
    /// Exists only for backwards compatability, always returns "UTC".
    /// </summary>
    [XmlRpcMember("tz_short_name")]
    public string TimeZoneShortName;

    /// <summary>
    /// Exists only for backwards compatability, always returns "+0000".
    /// </summary>
    [XmlRpcMember("tz_offset")]
    public string TimeZoneOffset;
  }
}
