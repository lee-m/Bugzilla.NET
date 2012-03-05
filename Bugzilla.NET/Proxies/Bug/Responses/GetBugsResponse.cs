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
using Bugzilla.Proxies.Bug.Params;

namespace Bugzilla.Proxies.Bug.Responses
{
  /// <summary>
  /// Response struct from Bug.get
  /// </summary>
  internal struct GetBugsResponse
  {
    /// <summary>
    /// Details of each requested bug.
    /// </summary>
    [XmlRpcMember("bugs")]
    public BugInfo[] Bugs;

    /// <summary>
    /// Error details about any requested bugs which could not be returned.
    /// </summary>
    [XmlRpcMember("faults")]
    public GetBugsFaults[] Faults;
  }

  /// <summary>
  /// Details about invalid bug requests.
  /// </summary>
  internal struct GetBugsFaults
  {
    /// <summary>
    /// Id of the bug this fault belongs to.
    /// </summary>
    [XmlRpcMember("id")]
    public int id;

    /// <summary>
    /// Error message.
    /// </summary>
    public string faultString;

    /// <summary>
    /// Error code.
    /// </summary>
    public string faultCode;
  }
}
