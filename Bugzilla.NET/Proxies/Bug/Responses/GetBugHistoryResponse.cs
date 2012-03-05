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
  /// Return value from querying for a bug's history.
  /// </summary>
  internal struct GetBugHistoryResponse
  {
    /// <summary>
    /// Details of all changes made to each requested bug.
    /// </summary>
    [XmlRpcMember("bugs")]
    public BugHistoryInfo[] History;
  }

  /// <summary>
  /// Holds all of the history for a single bug.
  /// </summary>
  internal class BugHistoryInfo
  {
    /// <summary>
    /// ID of the bug.
    /// </summary>
    [XmlRpcMember("id")]
    public int Id;

    /// <summary>
    /// Alias of the bug.
    /// </summary>
    [XmlRpcMember("alias")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Alias;

    /// <summary>
    /// Details of all the changes made to it.
    /// </summary>
    [XmlRpcMember("history")]
    public BugModificationInfo[] Changes;
  }

  /// <summary>
  /// Details of a single set of changes made to a bug.
  /// </summary>
  internal struct BugModificationInfo
  {
    /// <summary>
    /// Date/time when the change was made.
    /// </summary>
    [XmlRpcMember("when")]
    public DateTime ModificationDateTime;

    /// <summary>
    /// Login name of the person who made the change.
    /// </summary>
    [XmlRpcMember("who")]
    public string Who;

    /// <summary>
    /// The modifications made to the bug.
    /// </summary>
    [XmlRpcMember("changes")]
    public BugFieldModificationInfo[] FieldModifications;
  }

  /// <summary>
  /// Details of a single field/attachment modification for a bug.
  /// </summary>
  internal struct BugFieldModificationInfo
  {
    /// <summary>
    /// Name of the field which was changed.
    /// </summary>
    [XmlRpcMember("field_name")]
    public string FieldName;
    
    /// <summary>
    /// Old value of the field.
    /// </summary>
    [XmlRpcMember("removed")]
    public string OldValue;
    
    /// <summary>
    /// New value of the field.
    /// </summary>
    [XmlRpcMember("added")]
    public string NewValue;

    /// <summary>
    /// If the change was to an attachment, this is the ID of the attachment.
    /// </summary>
    [XmlRpcMember("attachment_id")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public int? AttachmentId;
  }
}
