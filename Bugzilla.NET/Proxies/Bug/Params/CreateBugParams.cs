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
  /// Parameters required when creating a new bug.
  /// </summary>
  /// <remarks>Declared as a class rather than struct as this forms the base class for dynamically generated classes
  /// at run-time to handle custom fields.</remarks>
  internal class CreateBugParams
  {
    /// <summary>
    /// The name of the product the bug is being filed against.
    /// </summary>
    [XmlRpcMember("product")]
    public string Product;

    /// <summary>
    /// The name of a component in the specified product.
    /// </summary>
    [XmlRpcMember("component")]
    public string Component;

    /// <summary>
    /// A brief description of the bug being filed.
    /// </summary>
    [XmlRpcMember("summary")]
    public string Summary;

    /// <summary>
    /// The version of the specified product the bug was found in
    /// </summary>
    [XmlRpcMember("version")]
    public string Version;

    /// <summary>
    /// The initial description for this bug.
    /// </summary>
    [XmlRpcMember("description")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Description;

    /// <summary>
    /// The operating system the bug was discovered on.
    /// </summary>
    [XmlRpcMember("op_sys")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string OperatingSystem;

    /// <summary>
    /// What type of hardware the bug was experienced on.
    /// </summary>
    [XmlRpcMember("platform")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Platform;

    /// <summary>
    /// What order the bug should be fixed in by the assignee.
    /// </summary>
    [XmlRpcMember("priority")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Priority;

    /// <summary>
    /// How severe the bug is.
    /// </summary>
    [XmlRpcMember("severity")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Severity;

    /// <summary>
    /// Unique alias for the bug.
    /// </summary>
    [XmlRpcMember("alias")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Alias;

    /// <summary>
    /// Who the bug is assigned to.
    /// </summary>
    [XmlRpcMember("assigned_to")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string AssignedTo;

    /// <summary>
    /// Set of usernames to CC on the bug.
    /// </summary>
    [XmlRpcMember("cc")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string[] CCList;

    /// <summary>
    /// Whether the initial comment added to the bug is private or not.
    /// </summary>
    [XmlRpcMember("comment_is_private")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public bool IsCommentPrivate;

    /// <summary>
    /// Set of group names the bug belongs to.
    /// </summary>
    [XmlRpcMember("groups")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string[] Groups;

    /// <summary>
    /// Username of the QC contact for the bug.
    /// </summary>
    [XmlRpcMember("qa_contact")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string QAContact;

    /// <summary>
    /// The status that this bug should start out as.
    /// </summary>
    [XmlRpcMember("status")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Status;

    /// <summary>
    /// A valid target milestone for the specified product.
    /// </summary>
    [XmlRpcMember("target_milestone")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string TargetMilestone;

    /// <summary>
    /// IDs of any bug the new bug depends on.
    /// </summary>
    [XmlRpcMember("depends_on")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public int[] DependsOn;

    /// <summary>
    /// IDs of any bugs blocked from being fixed by this bug.
    /// </summary>
    [XmlRpcMember("blocks")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public int[] Blocks;

    /// <summary>
    /// Estimation for how long this bug will take to fix.
    /// </summary>
    [XmlRpcMember("estimated_time")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public double? EstimatedTime;

    /// <summary>
    /// Deadline for fixing the bug.
    /// </summary>
    [XmlRpcMember("deadline")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Deadline;

    /// <summary>
    /// URL for the bug.
    /// </summary>
    [XmlRpcMember("bug_file_loc")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string URL;
  }
}
