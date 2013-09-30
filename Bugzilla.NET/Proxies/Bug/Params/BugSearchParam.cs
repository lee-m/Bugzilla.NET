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
  /// Parameters available when searching for a set of bugs.
  /// </summary>
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  internal struct BugSearchParam
  {
    /// <summary>
    /// Set of unique aliases to search on.
    /// </summary>
    [XmlRpcMember("alias")]
    public string[] Alias;

    /// <summary>
    /// List of user assigned to the bugs.
    /// </summary>
    [XmlRpcMember("assigned_to")]
    public string[] AssignedTo;

    /// <summary>
    /// Set of components to search for.
    /// </summary>
    [XmlRpcMember("component")]
    public string[] Component;

    /// <summary>
    /// Set of creation date/time to search on.
    /// </summary>
    [XmlRpcMember("creation_time")]
    public DateTime CreationTime;

    /// <summary>
    /// List of users who logged the bugs.
    /// </summary>
    [XmlRpcMember("creator")]
    public string[] Creator;

    /// <summary>
    /// List of bug IDs.
    /// </summary>
    [XmlRpcMember("id")]
    public int[] Id;

    /// <summary>
    /// Last modification date/time to search on.
    /// </summary>
    [XmlRpcMember("last_change_time")]
    public DateTime LastChangeTime;

    /// <summary>
    /// Maximum number of bugs to return.
    /// </summary>
    [XmlRpcMember("limit")]
    public int Limit;

    /// <summary>
    /// Used in conjunction with limit to define the starting point for the search.
    /// </summary>
    [XmlRpcMember("offset")]
    public int Offset;

    /// <summary>
    /// Set of operating systems to search on.
    /// </summary>
    [XmlRpcMember("op_sys")]
    public string[] OperatingSystem;

    /// <summary>
    /// Set of platforms to match on.
    /// </summary>
    [XmlRpcMember("platform")]
    public string[] Platform;

    /// <summary>
    /// Set of priorities to match on.
    /// </summary>
    [XmlRpcMember("priority")]
    public string[] Priority;

    /// <summary>
    /// Set of products to match.
    /// </summary>
    [XmlRpcMember("product")]
    public string[] Product;

    /// <summary>
    /// Set of resolutions to match.
    /// </summary>
    [XmlRpcMember("resolution")]
    public string[] Resolution;

    /// <summary>
    /// Set of severities to match on.
    /// </summary>
    [XmlRpcMember("severity")]
    public string[] Severity;

    /// <summary>
    /// Set of bug statuses to match on.
    /// </summary>
    [XmlRpcMember("status")]
    public string[] Status;

    /// <summary>
    /// Bug summaries to match on.
    /// </summary>
    [XmlRpcMember("summary")]
    public string[] Summary;

    /// <summary>
    /// Target milestones to match on.
    /// </summary>
    [XmlRpcMember("target_milestone")]
    public string[] TargetMilestone;

    /// <summary>
    /// QA contacts to search for.
    /// </summary>
    [XmlRpcMember("qa_contact")]
    public string[] QAContact;

    /// <summary>
    /// URLs to match on.
    /// </summary>
    [XmlRpcMember("url")]
    public string[] Url;

    /// <summary>
    /// Versions to match on.
    /// </summary>
    [XmlRpcMember("version")]
    public string[] Version;

    /// <summary>
    /// Status whiteboard values to match on.
    /// </summary>
    [XmlRpcMember("whiteboard")]
    public string[] Whiteboard;
  }
}
