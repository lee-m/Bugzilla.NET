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

namespace Bugzilla.Proxies.Bug.Params
{
  /// <summary>
  /// Parameters passed when updating details of a bug.
  /// </summary>
  internal struct UpdateBugParam
  {
    /// <summary>
    /// IDs of the bugs to update.
    /// </summary>
    [XmlRpcMember("ids")]
    public int[] Ids;

    /// <summary>
    /// Resolution of the bug.
    /// </summary>
    [XmlRpcMember("resolution")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Resolution;

    /// <summary>
    /// Status of the bug.
    /// </summary>
    [XmlRpcMember("status")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Status;

    /// <summary>
    /// A comment to apply when updating the bug.
    /// </summary>
    [XmlRpcMember("comment")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public CommentParam Comment;

    /// <summary>
    /// Whether to reset to assigned to field back to the default value.
    /// </summary>
    [XmlRpcMember("reset_assigned_to")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public bool? ResetAssignedTo;

    /// <summary>
    /// Whether to reset to QA contact field back to the default value.
    /// </summary>
    [XmlRpcMember("reset_qa_contact")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public bool? ResetQAContact;

    /// <summary>
    /// The amount of time left (in hours) to work on this bug.
    /// </summary>
    [XmlRpcMember("remaining_time")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public double? WorkTimeRemaining;

    /// <summary>
    /// The amount of time worked (in hours) on the bug.
    /// </summary>
    [XmlRpcMember("work_time")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public double? TimeWorked;

    /// <summary>
    /// If set, the public/private status of a set of comments on the bug to update.
    /// </summary>
    [XmlRpcMember("comment_is_private")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public XmlRpcStruct CommentVisibilityChanges;

    /// <summary>
    /// The set of keywords to add, remove or set on the bug.
    /// </summary>
    [XmlRpcMember("keywords")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public XmlRpcStruct KeywordModifications;

    /// <summary>
    /// The set of usernames to add or remove on the bug.
    /// </summary>
    [XmlRpcMember("cc")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public XmlRpcStruct CCListModifications;
  }
}
