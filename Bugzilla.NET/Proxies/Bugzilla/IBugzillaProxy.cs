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

using CookComputing.XmlRpc;
using Bugzilla.Proxies.Bugzilla.Responses;

namespace Bugzilla.Proxies.Bugzilla
{
  /// <summary>
  /// Proxy interface for interacting with the global Bugzilla web service API.
  /// </summary>
  [XmlRpcProxyAssemblyName("IBugzillaProxy")]
  internal interface IBugzillaProxy : IXmlRpcProxy
  {
    /// <summary>
    /// Gets the version number of the Bugzilla instance.
    /// </summary>
    /// <returns>The version number of the Bugzilla instance.</returns>
    [XmlRpcMethod("Bugzilla.version")]
    GetVersionResponse GetVersion();

    /// <summary>
    /// Gets time information from the Bugzilla instance.
    /// </summary>
    /// <returns></returns>
    [XmlRpcMethod("Bugzilla.time")]
    GetTimeResponse GetTime();

    /// <summary>
    /// Gets details of enabled extensions.
    /// </summary>
    /// <returns></returns>
    [XmlRpcMethod("Bugzilla.extensions")]
    GetExtensionsResponse GetExtensions();
  }
}
