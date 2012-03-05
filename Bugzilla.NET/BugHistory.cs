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
using System.Collections.Generic;
using System.Linq;

using Bugzilla.Proxies.Bug.Responses;

namespace Bugzilla
{
  /// <summary>
  /// Contains the history of a bug and all the modifications made to it.
  /// </summary>
  public class BugHistory
  {
    /// <summary>
    /// Modification info returned from the web services API.
    /// </summary>
    private BugHistoryInfo mInfo;

    /// <summary>
    /// Internal constructor to pass in the response value to wrap.
    /// </summary>
    /// <param name="info">Bug history WS response to wrap.</param>
    internal BugHistory(BugHistoryInfo info)
    {
      mInfo = info;
    }

    /// <summary>
    /// Accessor for the bug ID.
    /// </summary>
    public int Id { get { return mInfo.Id; } }

    /// <summary>
    /// Accessor for the bug alias (if specified).
    /// </summary>
    public string Alias { get { return mInfo.Alias; } }

    /// <summary>
    /// Accessor for the set of modifications made to this bug.
    /// </summary>
    public List<BugModification> Modifications { get { return mInfo.Changes.Select(m => new BugModification(m)).ToList(); } }
  }

  /// <summary>
  /// Contains the modifications made to a bug in a single operation.
  /// </summary>
  public class BugModification
  {
    /// <summary>
    /// Modification info returned from the web services API.
    /// </summary>
    private BugModificationInfo mInfo;

    /// <summary>
    /// Internal constructor to pass in the response value to wrap.
    /// </summary>
    /// <param name="info">Bug history WS response to wrap.</param>
    internal BugModification(BugModificationInfo info)
    {
      mInfo = info;
    }

    /// <summary>
    /// Accessor for the date/time the modification was made.
    /// </summary>
    public DateTime ModificationDateTime { get { return mInfo.ModificationDateTime; } }

    /// <summary>
    /// Accessor for the login name of the person who made the change.
    /// </summary>
    public string Who { get { return mInfo.Who; } }

    /// <summary>
    /// Accessor for the set of field/attachment modifications 
    /// </summary>
    public List<BugFieldOrAttachmentModification> FieldAttachmentModifications { get { return mInfo.FieldModifications.Select(m => new BugFieldOrAttachmentModification(m)).ToList(); ; } }
  }

  /// <summary>
  /// Details of a single field/attachment modification made to a bug.
  /// </summary>
  public class BugFieldOrAttachmentModification
  {
    /// <summary>
    /// Modification info returned from the web services API.
    /// </summary>
    private BugFieldModificationInfo mInfo;

    /// <summary>
    /// Internal constructor to pass in the response value to wrap.
    /// </summary>
    /// <param name="info">Bug history WS response to wrap.</param>
    internal BugFieldOrAttachmentModification(BugFieldModificationInfo info)
    {
      mInfo = info;
    }

    /// <summary>
    /// Accessor for the name of the field which was changed.
    /// </summary>
    public string FieldName { get { return mInfo.FieldName; } }

    /// <summary>
    /// Accessor for the old value of the field.
    /// </summary>
    public string OldValue { get { return mInfo.OldValue; } }

    /// <summary>
    /// Accessor for the new value of the field.
    /// </summary>
    public string NewValue { get { return mInfo.NewValue; } }

    /// <summary>
    /// If this modification was to an attachment, the ID of the attachment, otherwise null.
    /// </summary>
    public int? AttachmentId { get { return mInfo.AttachmentId; } }
  }
}

