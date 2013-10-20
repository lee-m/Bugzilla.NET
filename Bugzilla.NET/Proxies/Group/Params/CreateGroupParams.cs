//Copyright (C) 2013 by Lee Millward

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

namespace Bugzilla.Proxies.Group.Params
{
  /// <summary>
  /// Parameters used to create a new group.
  /// </summary>
  internal struct CreateGroupParams
  {
    /// <summary>
    /// Unique short name for the group.
    /// </summary>
    [XmlRpcMember("name")]
    [XmlRpcMissingMapping(MappingAction.Error)]
    public string ShortName;

    /// <summary>
    /// Description of the group.
    /// </summary>
    [XmlRpcMember("description")]
    [XmlRpcMissingMapping(MappingAction.Error)]
    public string Description;

    /// <summary>
    /// Any user whose Bugzilla username matches this regular expression will automatically be granted membership in this group.
    /// </summary>
    [XmlRpcMember("user_regexp")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string UserRegularExpression;

    /// <summary>
    /// Whether the group is active for new bugs or not.
    /// </summary>
    [XmlRpcMember("is_active")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public bool IsActive;

    /// <summary>
    /// A URL pointing to a small icon used to identify the group.
    /// </summary>
    [XmlRpcMember("icon_url")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string IconURL;
  }
}
