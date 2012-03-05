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

namespace Bugzilla.Proxies.User.Params
{
  /// <summary>
  /// Parameters required to query for a set of user's details.
  /// </summary>
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  internal struct GetUserParams
  {
    /// <summary>
    /// User IDs to search for.
    /// </summary>
    [XmlRpcMember("ids")]
    public int[] Ids;

    /// <summary>
    /// Login names to search for.
    /// </summary>
    [XmlRpcMember("names")]
    public string[] Names;

    /// <summary>
    /// User matching strings - any user whose real or login contains any of the strings which the currently logged in
    /// user can see will be returned.
    /// </summary>
    [XmlRpcMember("match")]
    public string[] UserMatches;

    /// <summary>
    /// IDs of any group that a user can be in.
    /// </summary>
    [XmlRpcMember("group_ids")]
    public int[] GroupIds;

    /// <summary>
    /// Names of any group that a user can be in.
    /// </summary>
    [XmlRpcMember("group")]
    public string[] Groups;

    /// <summary>
    /// Whether to return disabled users or not.
    /// </summary>
    /// <remarks>
    /// By default, when using the match parameter, disabled users are excluded from the returned results unless 
    /// their full username is identical to the match string. Setting include_disabled to true will include 
    /// disabled users in the returned results even if their username doesn't fully match the input string.
    /// </remarks>
    [XmlRpcMember("include_disabled")]
    public bool IncludeDisabled;
  }
}
