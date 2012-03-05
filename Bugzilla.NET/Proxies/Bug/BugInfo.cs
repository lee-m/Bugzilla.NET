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
  /// Information about a bug for use within the Bug API.
  /// </summary>
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  internal class BugInfo
  {
    /// <summary>
    /// The unique alias of this bug.
    /// </summary>
    [XmlRpcMember("alias")]
    public string Alias;

    /// <summary>
    /// The login name of the user to whom the bug is assigned.
    /// </summary>
    [XmlRpcMember("assigned_to")]
    public string AssignedTo;

    /// <summary>
    /// The ids of bugs that are "blocked" by this bug.
    /// </summary>
    [XmlRpcMember("blocks")]
    public int[] Blocks;

    /// <summary>
    /// The login names of users on the CC list of this bug.
    /// </summary>
    [XmlRpcMember("cc")]
    public string[] CCList;

    /// <summary>
    /// The name of the current classification the bug is in.
    /// </summary>
    [XmlRpcMember("classification")]
    public string Classification;

    /// <summary>
    /// The name of the current component of this bug.
    /// </summary>
    [XmlRpcMember("component")]
    public string Component;

    /// <summary>
    /// When the bug was created.
    /// </summary>
    [XmlRpcMember("creation_time")]
    public DateTime CreationTime;

    /// <summary>
    /// The login name of the person who filed this bug (the reporter).
    /// </summary>
    [XmlRpcMember("creator")]
    public string ReportedBy;

    /// <summary>
    /// The day that this bug is due to be completed, in the format YYYY-MM-DD.
    /// </summary>
    [XmlRpcMember("deadline")]
    public string Deadline;

    /// <summary>
    /// The ids of bugs that this bug "depends on".
    /// </summary>
    [XmlRpcMember("depends_on")]
    public int[] DependsOn;

    /// <summary>
    /// The bug ID of the bug that this bug is a duplicate of. If this bug isn't a duplicate of any bug, this will be null.
    /// </summary>
    [XmlRpcMember("dupe_of")]
    public int DuplicateOf;

    /// <summary>
    /// The number of hours that it was estimated that this bug would take.
    /// </summary>
    [XmlRpcMember("estimated_time")]
    public double EstimatedTime;

    /// <summary>
    /// The names of all the groups that this bug is in.
    /// </summary>
    [XmlRpcMember("groups")]
    public string[] Groups;

    /// <summary>
    /// The unique numeric id of this bug.
    /// </summary>
    [XmlRpcMember("id")]
    public int Id;

    /// <summary>
    /// If true, this bug can be accessed by members of the CC list, even if they are not in the groups the bug is restricted to.
    /// </summary>
    [XmlRpcMember("is_cc_accessible")]
    public bool IsCCListAccessible;

    /// <summary>
    /// True if the bug has been confirmed. 
    /// </summary>
    [XmlRpcMember("is_confirmed")]
    public bool IsConfirmed;

    /// <summary>
    /// True if this bug is open, false if it is closed.
    /// </summary>
    [XmlRpcMember("is_open")]
    public bool IsOpen;

    /// <summary>
    /// If true, this bug can be accessed by the creator (reporter) of the bug, even if he or she is not a member of 
    /// the groups the bug is restricted to.
    /// </summary>
    [XmlRpcMember("is_creator_accessible")]
    public bool IsAccessibleByReporter;

    /// <summary>
    /// Each keyword that is on this bug.
    /// </summary>
    [XmlRpcMember("keywords")]
    public string[] Keywords;

    /// <summary>
    /// When the bug was last changed.
    /// </summary>
    [XmlRpcMember("last_change_time")]
    public DateTime LastChangeTime;

    /// <summary>
    /// The name of the operating system that the bug was filed against.
    /// </summary>
    [XmlRpcMember("op_sys")]
    public string OperatingSystem;

    /// <summary>
    /// The name of the platform (hardware) that the bug was filed against.
    /// </summary>
    [XmlRpcMember("platform")]
    public string Platform;

    /// <summary>
    /// The priority of the bug.
    /// </summary>
    [XmlRpcMember("priority")]
    public string Priority;

    /// <summary>
    /// The name of the product this bug is in.
    /// </summary>
    [XmlRpcMember("product")]
    public string Product;

    /// <summary>
    /// The login name of the current QA Contact on the bug.
    /// </summary>
    [XmlRpcMember("qa_contact")]
    public string QAContact;

    /// <summary>
    /// The number of hours of work remaining until work on this bug is complete.
    /// </summary>
    [XmlRpcMember("remaining_time")]
    public double RemainingTime;

    /// <summary>
    /// The current resolution of the bug, or an empty string if the bug is open.
    /// </summary>
    [XmlRpcMember("resolution")]
    public string Resolution;

    /// <summary>
    /// The URLs in the See Also field on the bug.
    /// </summary>
    [XmlRpcMember("see_also")]
    public string[] SeeAlso;

    /// <summary>
    /// The current severity of the bug.
    /// </summary>
    [XmlRpcMember("severity")]
    public string Severity;

    /// <summary>
    /// The current status of the bug.
    /// </summary>
    [XmlRpcMember("status")]
    public string Status;

    /// <summary>
    /// The summary of this bug.
    /// </summary>
    [XmlRpcMember("summary")]
    public string Summary;

    /// <summary>
    /// The milestone that this bug is supposed to be fixed by, or for closed bugs, the milestone that it was fixed for.
    /// </summary>
    [XmlRpcMember("target_milestone")]
    public string TargetMilestone;

    /// <summary>
    /// The token that you would have to pass to the process_bug.cgi page in order to update this bug. 
    /// This changes every time the bug is updated.
    /// </summary>
    [XmlRpcMember("update_token")]
    public string UpdateToken;

    /// <summary>
    /// A URL that demonstrates the problem described in the bug, or is somehow related to the bug report.
    /// </summary>
    [XmlRpcMember("url")]
    public string Url;

    /// <summary>
    /// The version the bug was reported against.
    /// </summary>
    [XmlRpcMember("version")]
    public string Version;

    /// <summary>
    /// The value of the "status whiteboard" field on the bug.
    /// </summary>
    [XmlRpcMember("whiteboard")]
    public string Whiteboard;
  }
}
