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
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  internal class UpdateBugParam
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
    public double? RemainingTime;

    /// <summary>
    /// The amount of time worked (in hours) on the bug.
    /// </summary>
    [XmlRpcMember("work_time")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public double? WorkTIme;

    /// <summary>
    /// If set, the public/private status of a set of comments on the bug to update.
    /// </summary>
    [XmlRpcMember("comment_is_private")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public XmlRpcStruct CommentVisibilityChanges;

    /// <summary>
    /// The set of groups to add or remove.
    /// </summary>
    [XmlRpcMember("groups")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public XmlRpcStruct Groups;

    /// <summary>
    /// The set of keywords to add, remove or set on the bug.
    /// </summary>
    [XmlRpcMember("keywords")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public XmlRpcStruct Keywords;

    /// <summary>
    /// The set of usernames to add or remove on the bug.
    /// </summary>
    [XmlRpcMember("cc")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public XmlRpcStruct CCList;

    /// <summary>
    /// The alias of the bug to update.
    /// </summary>
    [XmlRpcMember("alias")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Alias;

    /// <summary>
    /// The name of the product the bug is in.
    /// </summary>
    [XmlRpcMember("product")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Product;

    /// <summary>
    /// The full login name of the user this bug is assigned to.
    /// </summary>
    [XmlRpcMember("assigned_to")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string AssignedTo;

    /// <summary>
    /// Set of bug IDs this bug depends on.
    /// </summary>
    [XmlRpcMember("depends_on")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public XmlRpcStruct DependsOn;

    /// <summary>
    /// Set of bug IDs this bug blocks.
    /// </summary>
    [XmlRpcMember("blocks")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public XmlRpcStruct Blocks;

    /// <summary>
    /// Whether or not users in the CC list are allowed to access the bug, even if 
    /// they aren't in a group that can normally access the bug.
    /// </summary>
    [XmlRpcMember("is_cc_accessible")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public bool? IsCCAccessible;

    /// <summary>
    /// The component the bug is in.
    /// </summary>
    [XmlRpcMember("component")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Component;

    /// <summary>
    /// A date specifying when the bug must be completed by in the format YYYY-MM-DD
    /// </summary>
    [XmlRpcMember("deadline")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Deadline;

    /// <summary>
    /// The bug that this bug is a duplicate of.
    /// </summary>
    [XmlRpcMember("dupe_of")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public int? DuplicateOf;

    /// <summary>
    /// The total estimate of time required to fix the bug, in hours
    /// </summary>
    [XmlRpcMember("estimated_time")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public double? EstimatedTime;

    /// <summary>
    /// The operating system field on the bug.
    /// </summary>
    [XmlRpcMember("op_sys")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string OperatingSystem;

    /// <summary>
    /// The platform field on the bug.
    /// </summary>
    [XmlRpcMember("platform")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Platform;

    /// <summary>
    /// The priority field on the bug.
    /// </summary>
    [XmlRpcMember("priority")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Priority;

    /// <summary>
    /// The full login name of the bug's QA Contact.
    /// </summary>
    [XmlRpcMember("qa_contact")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string QAContact;

    /// <summary>
    /// Whether or not the bug's reporter is allowed to access the bug, even if he or she isn't 
    /// in a group that can normally access the bug.
    /// </summary>
    [XmlRpcMember("is_creator_accessible")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public bool? IsCreatorAccessible;

    /// <summary>
    /// The severity field on the bug.
    /// </summary>
    [XmlRpcMember("severity")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Severity;

    /// <summary>
    /// The summary field on the bug.
    /// </summary>
    [XmlRpcMember("summary")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Summary;

    /// <summary>
    /// The target milestone for the bug.
    /// </summary>
    [XmlRpcMember("target_milestone")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string TargetMilestone;

    /// <summary>
    /// The URL field on the bug.
    /// </summary>
    [XmlRpcMember("url")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string URL;

    /// <summary>
    /// The version field on the bug.
    /// </summary>
    [XmlRpcMember("version")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Version;

    /// <summary>
    /// Status whiteboard field on the bug.
    /// </summary>
    [XmlRpcMember("whiteboard")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Whiteboard;
  }
}
