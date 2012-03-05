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

namespace Bugzilla.Proxies.Bug.Params
{
  /// <summary>
  /// Information required when querying for comments.
  /// </summary>
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  internal struct GetCommentParams
  {
    /// <summary>
    /// List of bug ids or aliases to get the visible comments for.
    /// </summary>
    [XmlRpcMember("ids")]
    public string[] IdsOrAliases;

    /// <summary>
    /// Set of comment ids to get in addition to any comments against the specified bugs.
    /// </summary>
    [XmlRpcMember("comment_ids")]
    public int[] CommentIDs;

    /// <summary>
    /// Only get comments which are newer than this time.
    /// </summary>
    [XmlRpcMember("new_since")]
    public DateTime? NewSince;
  }
}
