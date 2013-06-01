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

namespace Bugzilla.Proxies.Bug.Responses
{
  /// <summary>
  /// Return value from Bug.update
  /// </summary>
  internal struct UpdateBugResponse
  {
    /// <summary>
    /// The actual changes made to the bug.
    /// </summary>
    [XmlRpcMember("bugs")]
    public UpdateBugResponseContents[] Changes;
  }

  /// <summary>
  /// The actual payload of the return value from Bug.update
  /// </summary>
  internal struct UpdateBugResponseContents
  {
    /// <summary>
    /// ID of the bug that was changed
    /// </summary>
    [XmlRpcMember("id")]
    public int ID;

    /// <summary>
    /// Alias of the bug that was changed
    /// </summary>
    [XmlRpcMember("alias")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Alias;

    /// <summary>
    /// The date/time the update was performed.
    /// </summary>
    [XmlRpcMember("last_change_time")]
    public DateTime LastChangeTime;

    /// <summary>
    /// Set of field modifications that were applied.
    /// </summary>
    [XmlRpcMember("changes")]
    public XmlRpcStruct Modifications;
  }
}
