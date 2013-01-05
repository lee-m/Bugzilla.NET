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
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using CookComputing.XmlRpc;

using Bugzilla.Utils;
using Bugzilla.Proxies.Bug;
using Bugzilla.Proxies.Bug.Params;
using Bugzilla.Proxies.Bug.Responses;

namespace Bugzilla
{
  /// <summary>
  /// Represents a single bug.
  /// </summary>
  public class Bug
  {
    /// <summary>
    /// Details about this bug.
    /// </summary>
    private readonly XmlRpcStruct mBugInfo;

    /// <summary>
    /// Wrapper around the custom fields to allow accessing them by field name.
    /// </summary>
    private readonly BugCustomFields mCustomFields;

    /// <summary>
    /// Proxy for updating this bug on the server.
    /// </summary>
    private IBugProxy mProxy;

    #region BugCustomFields

    /// <summary>
    /// Wrapper around the custom fields for a bug to provide access based on the field name.
    /// </summary>
    public class BugCustomFields : IEnumerable<KeyValuePair<string, object>>
    {
      /// <summary>
      /// Field values returned by the remote server for the parent bug.
      /// </summary>
      private Dictionary<string, object> mFieldValues;

      /// <summary>
      /// Constructor to pass in the field values.
      /// </summary>
      /// <param name="fieldValues">Field values returned by the remote server.</param>
      internal BugCustomFields(Dictionary<string, object> fieldValues)
      {
        mFieldValues = fieldValues;
      }

      /// <summary>
      /// Provides an non-generic IEnumerator instance.
      /// </summary>
      /// <returns></returns>
      IEnumerator IEnumerable.GetEnumerator()
      {
        return mFieldValues.GetEnumerator();
      }

      /// <summary>
      /// Provides a generic IEnumerator instance.
      /// </summary>
      /// <returns></returns>
      IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
      {
        return mFieldValues.GetEnumerator();
      }
      /// <summary>
      /// Accessor for the value of a field based on its name.
      /// </summary>
      /// <param name="key">Name of the field to access. Must start with "cf_"</param>
      /// <returns>The value of the specified field.</returns>
      /// <exception cref="KeyNotFoundException">No custom field with the specified key was found.</exception>
      public object this[string key]
      {
        get { return mFieldValues[key]; }
        set { mFieldValues[key] = value; }
      }
    }

    #endregion

    /// <summary>
    /// Creates a instance with the specified bug details.
    /// </summary>
    /// <param name="info">Bug details</param>
    /// <param name="proxy">Proxy for updating the bug.</param>
    internal Bug(XmlRpcStruct info, IBugProxy proxy)
    {
      mBugInfo = info;
      mProxy = proxy;

      //Strip out the custom fields
      Dictionary<string, object> customFields = new Dictionary<string,object>();

      foreach (string key in info.Keys)
      {
        if (key.StartsWith("cf_", StringComparison.InvariantCultureIgnoreCase))
          customFields.Add(key, info[key]);
      }

      mCustomFields = new BugCustomFields(customFields);
    }

    /// <summary>
    /// Adds a comment to this bug.
    /// </summary>
    /// <param name="comment">The comment text. Mandatory. Max allowed length of 65556 characters.</param>
    /// <param name="isPrivate">Whether the comment is private or not.</param>
    /// <param name="workTime">Amount to add to the "Hours Worked". Ignored if logged in user is not in the 
    /// time tracking group. Cannot be larger than 99999.99.</param>
    /// <returns>The ID of the new comment.</returns>
    /// <exception cref="ArgumentNullException">Null or zero length comment text provided.</exception>
    /// <exception cref="ArgumentException">Comment text exceeded max length or work time exceeded max value.</exception>
    /// <exception cref="InvalidBugIDOrAliasException">Invalid bug ID or alias specified.</exception>
    /// <exception cref="BugEditAccessDeniedException">Currently logged in user does not have security rights to edit the bug.</exception>
    /// <exception cref="InsufficientSecurityPrivilagesException">Attempted to add a private comment but current user has insufficient security rights.</exception>
    public int AddComment(string comment, bool isPrivate, double? workTime)
    {
      if (string.IsNullOrEmpty(comment))
        throw new ArgumentNullException("comment");

      //Check parameter limits
      if (comment.Length > ushort.MaxValue)
        throw new ArgumentException("Comment text must be less than 65535 characters.");

      if (workTime.HasValue && workTime > 99999.99)
        throw new ArgumentException("Work time must be less than 99999.99.");

      AddCommentParam commentParams = new AddCommentParam();
      commentParams.CommentText = comment;
      commentParams.IdOrAlias = Id.ToString();
      commentParams.IsPrivate = isPrivate;
      commentParams.WorkTime = workTime;

      try
      {
        AddCommentResponse resp = mProxy.AddComment(commentParams);
        return resp.CommentID;
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 100:
          case 101:
            throw new InvalidBugIDOrAliasException(Id.ToString());

          case 109:
            throw new BugEditAccessDeniedException(Id.ToString());

          case 113:
            throw new InsufficientSecurityPrivilagesException();

          default:
            throw new BugzillaException(string.Format("Error adding comment to bug. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Gets details of all comments entered against this bug.
    /// </summary>
    /// <param name="startDate">If set, only comments entered after this date will be returned.</param>
    /// <returns>Details of each comment against the bug.</returns>
    /// <exception cref="CommentAccessDeniedException">Attempted to access a private comment when the current user is not in the "insiders" group.</exception>
    public List<Comment> GetComments(DateTime? startDate)
    {
      //Fill in the request params
      GetCommentParams commentParams = new GetCommentParams();
      commentParams.IdsOrAliases = new string[] { Id.ToString() };
      commentParams.NewSince = startDate;

      try
      {
        GetCommentsResponse resp = mProxy.GetComments(commentParams);

        //Parse each comments details from the response
        List<Comment> comments = new List<Comment>();

        foreach (var bugID in resp.BugComments.Keys)
        {
          XmlRpcStruct commentsHash = (XmlRpcStruct)resp.BugComments[bugID];

          foreach (var comment in (object[])commentsHash["comments"])
            comments.Add(new Comment((XmlRpcStruct)comment));
        }

        return comments;
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 110:
            throw new CommentAccessDeniedException();

          default:
            throw new BugzillaException(string.Format("Error getting comments for bug. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Adds an attachment to this bug.
    /// </summary>
    /// <param name="fileName">Name of the file to attach.</param>
    /// <param name="summary">Short summary text of the attachment. Mandatory</param>
    /// <param name="comment">Comment text to add along with the attachment.</param>
    /// <param name="isPatch">Whether the attachment is a patch file or not.</param>
    /// <param name="isPrivate">Whether the attachment should be private or not.</param>
    /// <remarks>The MIME type will be automatically determined from either the extension of the file, or it's data..</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="fileName">fileName</paramref> is null or blank.</exception>
    public void AddAttachment(string fileName, string summary, string comment, bool isPatch, bool isPrivate)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      AddAttachment(File.ReadAllBytes(fileName), Path.GetFileName(fileName), summary, 
                    MIMETypes.GetMIMEType(fileName), comment, isPatch, isPrivate);
    }

    /// <summary>
    /// Adds an attachment to this bug.
    /// </summary>
    /// <param name="attachmentData">Data for the attachment. Mandatory.</param>
    /// <param name="fileName">Name of the file to show in the UI.</param>
    /// <param name="summary">Short summary text of the attachment. Mandatory</param>
    /// <param name="mimeType">MIME type of the attachment. Mandatory</param>
    /// <param name="comment">Comment text to add along with the attachment.</param>
    /// <param name="isPatch">Whether the attachment is a patch file or not.</param>
    /// <param name="isPrivate">Whether the attachment should be private or not.</param>
    /// <returns>Details of the newly created attachment.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="attachmentData">Attachment data</paramref> not specified.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="summary">summary</paramref> is null or blank.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="mimeType">mimeType</paramref> is null or blank.</exception>
    /// <exception cref="ArgumentException"><paramref name="attachmentData"/> has zero length.</exception>
    /// <exception cref="InvalidAttachmentURLException">An attempt was made to add a URL as an attachment but the attachment data was invalid.</exception>
    /// <exception cref="URLAttachmentsDisabledException">Attaching of URLs is disabled.</exception>
    public Attachment AddAttachment(byte[] attachmentData, 
                                    string fileName, 
                                    string summary, 
                                    string mimeType, 
                                    string comment, 
                                    bool isPatch, 
                                    bool isPrivate)
    {
      //Check that the required parameters are set
      if (attachmentData == null)
        throw new ArgumentNullException("attachmentData");

      if (string.IsNullOrEmpty(summary))
        throw new ArgumentNullException("summary");

      if (string.IsNullOrEmpty(mimeType))
        throw new ArgumentNullException("mimeType");

      if (attachmentData.Length == 0)
        throw new ArgumentException("Attachment data not specified.");

      AddAttachmentParams attachmentParams = new AddAttachmentParams();
      attachmentParams.Comment = comment;
      attachmentParams.Data = attachmentData;
      attachmentParams.FileName = fileName;
      attachmentParams.IdsOrAliases = new string[] { Id.ToString() };
      attachmentParams.IsPatch = isPatch;
      attachmentParams.IsPrivate = isPrivate;
      attachmentParams.MIMEType = mimeType;
      attachmentParams.Summary = summary;

      try
      {
        AddAttachmentResponse resp = mProxy.AddAttachment(attachmentParams);
        return new Attachment(resp.Attachments.Values.Cast<XmlRpcStruct>().ToArray()[0]);
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 600:
            throw new AttachmentTooLargeException();

          case 601:
            throw new InvalidMIMETypeException(mimeType);

          case 602:
            throw new InvalidAttachmentURLException();

          case 605:
            throw new URLAttachmentsDisabledException();

          default:
            throw new BugzillaException(string.Format("Error adding attachment to bug. Details: {0}", e.Message));
        }
      }
    }
   
    /// <summary>
    /// Gets the attachments for this bug.
    /// </summary>
    /// <returns>Details of all attachments for this bug.</returns>
    /// <exception cref="InvalidBugIDOrAliasException">An invalid bug ID or alias was specified.</exception>
    /// <exception cref="BugAccessDeniedException">Access to the specified bug was denied.</exception>
    /// <exception cref="AttachmentAccessDeniedException">Attempted to get access to a private attachment when current user is not in "insiders" group..</exception>
    public List<Attachment> GetAttachments()
    {
      GetAttachmentsParam attachmentParams = new GetAttachmentsParam();
      attachmentParams.BugIDsOrAliases = new string[] { Id.ToString() };

      try
      {
        GetAttachmentsResponse resp = mProxy.GetAttachments(attachmentParams);

        //Parse each attachment from the response
        List<Attachment> attachments = new List<Attachment>();

        foreach (var bugID in resp.BugAttachments.Keys)
        {
          foreach (var attachment in (object[])resp.BugAttachments[bugID])
            attachments.Add(new Attachment((XmlRpcStruct)attachment));
        }

        return attachments;
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 100:
          case 101:
            throw new InvalidBugIDOrAliasException(Id.ToString());

          case 102:
            throw new BugAccessDeniedException();

          case 304:
            throw new AttachmentAccessDeniedException();

          default:
            throw new BugzillaException(string.Format("Error getting attachments for bug. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Gets details of all changes made to this bug.
    /// </summary>
    /// <exception cref="InvalidBugIDOrAliasException">No bug exists with the specified ID.</exception>
    /// <exception cref="BugAccessDeniedException">Requested bug is inaccessible to the current user.</exception>
    /// <returns>Details of all changes made to fields or attachments for this bug.</returns>
    public BugHistory GetHistory()
    {
      GetBugHistoryParams getParams = new GetBugHistoryParams();
      getParams.IdsOrAliases = new string[] { Id.ToString() };

      try
      {
        GetBugHistoryResponse resp = mProxy.GetHistory(getParams);
        return new BugHistory(resp.History.First());
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 100:
          case 101:
            throw new InvalidBugIDOrAliasException(Id.ToString());

          case 102:
            throw new BugAccessDeniedException();

          default:
            throw new BugzillaException(string.Format("Error getting history for bug. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Adds or removes URLs from the "see also" field on the bug.
    /// </summary>
    /// <param name="urlsToAdd">URLs to add. If a URL does not start with "http://" or "https://" then "http://" will be automatically added.</param>
    /// <param name="urlsToRemove">URLs to remove.</param>
    /// <exception cref="InvalidBugIDOrAliasException">No bug exists with the specified ID.</exception>
    /// <exception cref="BugAccessDeniedException">Requested bug is inaccessible to the current user.</exception>
    /// <exception cref="InvalidSeeAlsoURLException">One or more of the specified URLs are invalid.</exception>
    /// <exception cref="SeeAlsoEditAccessDenied">Currently logged in user does not have security access to edit this bug's see also field.</exception>
    /// <returns>Details of the actual changes which were made to the bug.</returns>
    /// <remarks>
    /// <para>
    /// Attempting to add an already added URL or attempts to remove an invalid URL will be silently ignored.
    /// </para>
    /// If the same URL is specified in both <paramref name="urlsToAdd"/> and <paramref name="urlsToRemove"/> it will be <b>added</b>.
    /// <para>
    /// </para>
    /// </remarks>
    public SeeAlsoModifications UpdateSeeAlsoURLs(IEnumerable<string> urlsToAdd, IEnumerable<string> urlsToRemove)
    {
      UpdateSeeAlsoParams updateParams = new UpdateSeeAlsoParams();
      updateParams.IdsOrAliases = new string[] { Id.ToString() };

      if (urlsToAdd != null)
        updateParams.URLsToAdd = urlsToAdd.ToArray();

      if (urlsToRemove != null)
        updateParams.URLsToRemove = urlsToRemove.ToArray();

      try
      {
        UpdateSeeAlsoResponse resp = mProxy.UpdateSeeAlso(updateParams);

        //Parse the response
        object key = resp.Changes.Keys.Cast<object>().First();
        XmlRpcStruct mods = (XmlRpcStruct)((XmlRpcStruct)resp.Changes[key])["see_also"];

        int bugID = int.Parse(key.ToString());
        object[] added = (object[])mods["added"];
        object[] removed = (object[])mods["removed"];
        return new SeeAlsoModifications(bugID, added.Cast<string>().ToArray(), removed.Cast<string>().ToArray());
      }
      catch(XmlRpcFaultException e)
      {
        switch(e.FaultCode)
        {
          case 100:
          case 101:
            throw new InvalidBugIDOrAliasException(Id.ToString());

          case 102:
            throw new BugAccessDeniedException();

          case 119:
            throw new BugEditAccessDeniedException(Id.ToString());

          case 112:
            throw new InvalidSeeAlsoURLException();

          case 115:
            throw new SeeAlsoEditAccessDenied();

          default:
            throw new BugzillaException(string.Format("Error updating see also field for bug. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Resets the assignee for this bug back to the default for its component.
    /// </summary>
    /// <param name="changeComment">The text of any comment to add whilst making the changes. May be <code>null</code> or a blank string
    /// to indicate no comment should be created.</param>
    /// <param name="changeCommentPrivate">>If adding a comment along with the changes, whether the comment should be private or public.  
    /// Defaults to a public comment if not set.</param>
    public void ResetAssignedTo(string changeComment, bool? changeCommentPrivate)
    {
      UpdateBugParam updateParams = new UpdateBugParam();
      updateParams.Ids = new int[] { Id };
      updateParams.ResetAssignedTo = true;

      if (!string.IsNullOrEmpty(changeComment))
      {
        updateParams.Comment = new CommentParam();
        updateParams.Comment.CommentText = changeComment;
        updateParams.Comment.IsPrivate = changeCommentPrivate.GetValueOrDefault();
      }

      try
      {
        mProxy.UpdateBug(updateParams);
      }
      catch(XmlRpcFaultException e)
      {
        switch(e.FaultCode)
        {
          case 115:
            throw new BugEditAccessDeniedException(Id.ToString());

          default:
            throw new BugzillaException(string.Format("Error resetting the assigned to field. Details: {0}", e.Message));
        }
      }
    }
 
    /// <summary>
    /// Resets the QA contact for this bug back to the default for its component.
    /// </summary>
    /// <param name="changeComment">The text of any comment to add whilst making the changes. May be <code>null</code> or a blank string
    /// to indicate no comment should be created.</param>
    /// <param name="changeCommentPrivate">>If adding a comment along with the changes, whether the comment should be private or public.  
    /// Defaults to a public comment if not set.</param>
    public void ResetQAContact(string changeComment, bool? changeCommentPrivate)
    {
      UpdateBugParam updateParams = new UpdateBugParam();
      updateParams.Ids = new int[] { Id };
      updateParams.ResetQAContact = true;

      if (!string.IsNullOrEmpty(changeComment))
      {
        updateParams.Comment = new CommentParam();
        updateParams.Comment.CommentText = changeComment;
        updateParams.Comment.IsPrivate = changeCommentPrivate.GetValueOrDefault();
      }

      try
      {
        mProxy.UpdateBug(updateParams);
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 115:
            throw new BugEditAccessDeniedException(Id.ToString());

          default:
            throw new BugzillaException(string.Format("Error resetting the QA contact. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Sets the number of hours work remaining on the bug.
    /// </summary>
    /// <param name="remainingWorkTime">The amount of work time remaining in hours.</param>
    /// <param name="changeComment">If set, the text of a comment to add at the same time as the remaining work time is updated.</param>
    /// <param name="changeCommentPrivate">If adding a comment, indicates whether the comment is private or not. Defaults to false if not set.</param>
    public void SetNumberOfHoursWorkRemaining(double remainingWorkTime, 
                                              string changeComment,
                                              bool? changeCommentPrivate)
    {
      UpdateBugParam updateParams = new UpdateBugParam();
      updateParams.Ids = new int[] { Id };
      updateParams.WorkTimeRemaining = remainingWorkTime;

      if (!string.IsNullOrEmpty(changeComment))
      {
        updateParams.Comment = new CommentParam();
        updateParams.Comment.CommentText = changeComment;
        updateParams.Comment.IsPrivate = changeCommentPrivate.GetValueOrDefault();
      }

      try
      {
        mProxy.UpdateBug(updateParams);
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 115:
            throw new BugEditAccessDeniedException(Id.ToString());

          default:
            throw new BugzillaException(string.Format("Error setting the number of hours work remaining. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Updates the number of hours worked on this bug.
    /// </summary>
    /// <param name="hoursWorked">Number of hours worked on the bug.</param>
    /// <param name="remainingTime">The number of hours left to work on this bug.</param>
    /// <param name="changeComment">If set, the text of a comment to add whilst updating the remaining work time.</param>
    /// <param name="changeCommentPrivate">If adding a comment, whether the comment is private or not. If not set, defaults to public.</param>
    /// <remarks>If <paramref name="remainingTime"/> is not set, the value of <paramref name="hoursWorked"/> will be deducted
    /// from the bugs remaining time.</remarks>
    public void UpdateNumberOfHoursWorked(double hoursWorked, double? remainingTime,
                                          string changeComment,
                                          bool? changeCommentPrivate)
    {
      UpdateBugParam updateParams = new UpdateBugParam();
      updateParams.Ids = new int[] { Id };
      updateParams.TimeWorked = hoursWorked;
      updateParams.WorkTimeRemaining = remainingTime;

      if (!string.IsNullOrEmpty(changeComment))
      {
        updateParams.Comment = new CommentParam();
        updateParams.Comment.CommentText = changeComment;
        updateParams.Comment.IsPrivate = changeCommentPrivate.GetValueOrDefault();
      }

      try
      {
        mProxy.UpdateBug(updateParams);
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 115:
            throw new BugEditAccessDeniedException(Id.ToString());

          default:
            throw new BugzillaException(string.Format("Error updating number of hours worked. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Toggles the public/private status of a set of existing comments on this bug.
    /// </summary>
    /// <param name="commentStatusChanges">A set of comment ID/private status pairs.</param>
    /// <param name="changeComment">If non-null, the text of a comment to add alongside the changes.</param>
    /// <param name="changeCommentPrivate">If non-null, indicates whether the change comment should be private. If not set, defaults to a public comment.</param>
    /// <exception cref="ArgumentNullException"><paramref name="commentStatusChanges"/> is <code>null</code>.</exception>
    public void ToggleCommentsPublicPrivateStatus(IEnumerable<Tuple<int, bool>> commentStatusChanges,
                                                  string changeComment, 
                                                  bool? changeCommentPrivate)
    {
      if (commentStatusChanges == null)
        throw new ArgumentNullException("commentStatusChanges");

      UpdateBugParam updateParams = new UpdateBugParam();
      updateParams.Ids = new int[] { Id };
      updateParams.CommentVisibilityChanges = new XmlRpcStruct();

      if (!string.IsNullOrEmpty(changeComment))
      {
        updateParams.Comment = new CommentParam();
        updateParams.Comment.CommentText = changeComment;
        updateParams.Comment.IsPrivate = changeCommentPrivate.GetValueOrDefault();
      }

      foreach (Tuple<int, bool> commentStatus in commentStatusChanges)
        updateParams.CommentVisibilityChanges.Add(commentStatus.Item1.ToString(), commentStatus.Item2);

      try
      {
        mProxy.UpdateBug(updateParams);
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 115:
            throw new BugEditAccessDeniedException(Id.ToString());

          default:
            throw new BugzillaException(string.Format("Error toggling comments privacy statuses. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Updates the set of keywords on this bug to add new keywords, remove deleted keywords or set a new set of keywords.
    /// </summary>
    /// <param name="newKeywords">If non-null, the new keywords to add.</param>
    /// <param name="deletedKeywords">If non-null, the keywords to remove from the bug.</param>
    /// <param name="resetKeywords">If non-null, the new set of keywords to set on the bug.</param>
    /// <param name="changeComment">If non-null, the text of a comment to add at the same time as resetting the keywords.</param>
    /// <param name="changeCommentPrivate">If adding a change comment, whether the comment should be private or not.</param>
    /// <remarks>
    /// Specifying <paramref name="resetKeywords"/> will override any values passed in via <paramref name="newKeywords"/>
    /// or <paramref name="deletedKeywords"/>.
    /// </remarks>
    /// <exception cref="InvalidKeywordException">One or more invalid keywords were specified.</exception>
    /// <exception cref="BugEditAccessDeniedException">Currently logged in user does not have the required security rights to modify this bug.</exception>
    public void UpdateKeywords(IEnumerable<string> newKeywords, 
                               IEnumerable<string> deletedKeywords,
                               IEnumerable<string> resetKeywords,
                               string changeComment, 
                               bool? changeCommentPrivate)
    {
      //At least one of the new, deleted or reset parameters need to be specified otherwise there's nothing to do.
      if (newKeywords == null
          && deletedKeywords == null
          && resetKeywords == null)
        throw new ArgumentException("At least one of new keywords, deleted keywords or keywords to set needs to be provided.");

      UpdateBugParam updateParams = new UpdateBugParam();
      updateParams.Ids = new int[] { Id };
      updateParams.KeywordModifications = new XmlRpcStruct();

      if(newKeywords != null)
        updateParams.KeywordModifications.Add("add", newKeywords.ToArray());

      if(deletedKeywords != null)
        updateParams.KeywordModifications.Add("remove", newKeywords.ToArray());

      if(resetKeywords != null)
        updateParams.KeywordModifications.Add("set", newKeywords.ToArray());

      if (!string.IsNullOrEmpty(changeComment))
      {
        updateParams.Comment = new CommentParam();
        updateParams.Comment.CommentText = changeComment;
        updateParams.Comment.IsPrivate = changeCommentPrivate.GetValueOrDefault();
      }

      try
      {
        mProxy.UpdateBug(updateParams);
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 51:
            throw new InvalidKeywordException(e.FaultString);

          case 115:
            throw new BugEditAccessDeniedException(Id.ToString());

          default:
            throw new BugzillaException(string.Format("Error updating bug keywords. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Updates the CC list for this bug.
    /// </summary>
    /// <param name="usersToAdd">Set of <b>full</b> usernames to add to the CC list.</param>
    /// <param name="usersToRemove">Set of <b>full</b> usernames to remove from the CC list.</param>
    /// <param name="changeComment">If set, the text of a comment to add at the same time as updating the CC list.</param>
    /// <param name="changeCommentPrivate">If adding a change comment, indicates whether the comment is private or not.</param>
    /// <exception cref="ArgumentException">Both <paramref name="usersToAdd"/> and <paramref name="usersToRemove"/> are null/Nothing.</exception>
    public void UpdateCCList(IEnumerable<string> usersToAdd, 
                             IEnumerable<string> usersToRemove,
                             string changeComment,
                             bool? changeCommentPrivate)
    {
      if (usersToAdd == null
         && usersToRemove == null)
        throw new ArgumentException("At least one set of users to add and/or remove must be provided.");

      UpdateBugParam updateParams = new UpdateBugParam();
      updateParams.Ids = new int[] { Id };
      updateParams.CCListModifications = new XmlRpcStruct();

      if (usersToAdd != null)
        updateParams.CCListModifications.Add("add", usersToAdd.ToArray());

      if (usersToRemove != null)
        updateParams.CCListModifications.Add("remove", usersToRemove.ToArray());

      if (!string.IsNullOrEmpty(changeComment))
      {
        updateParams.Comment = new CommentParam();
        updateParams.Comment.CommentText = changeComment;
        updateParams.Comment.IsPrivate = changeCommentPrivate.GetValueOrDefault();
      }

      try
      {
        mProxy.UpdateBug(updateParams);
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 51:
            throw new InvalidUserException(e.FaultString);

          case 115:
            throw new BugEditAccessDeniedException(Id.ToString());

          default:
            throw new BugzillaException(string.Format("Error updating bug CC list. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Updates this bug's fields to the values set on the various properties.
    /// </summary>
    /// <param name="changeComment">If set, the text of a comment to add at the same time as updating the CC list.</param>
    /// <param name="changeCommentPrivate">If adding a change comment, indicates whether the comment is private or not.</param>
    public void Update(string changeComment, bool? changeCommentPrivate)
    {
      UpdateBugParam updateParams = new UpdateBugParam();
      updateParams.Ids = new int[] { Id };
      updateParams.Product = Product;
      updateParams.AssignedTo = AssignedTo;
      updateParams.AccessibleToCCList = AccessibleToCCListMembers;
      updateParams.Component = Component;
      updateParams.Deadline = Deadline;
      updateParams.DuplicateOf = DuplicateOf;
      updateParams.EstimatedResolutionTime = EstimatedResolutionTimeHours;
      updateParams.OperatingSystem = OperatingSystem;
      updateParams.Platform = Platform;
      updateParams.Priority = Priority;
      updateParams.QAContact = QAContact;
      updateParams.IsAccessibleByReporter = IsAccessibleByReporter;
      updateParams.Resolution = Resolution;
      updateParams.Severity = Severity;
      updateParams.Status = Status;
      updateParams.Summary = Summary;
      updateParams.TargetMilestone = TargetMilestone;
      updateParams.URL = URL;
      updateParams.Version = Version;
      updateParams.Whiteboard = StatusWhiteboard;

      //Set the depends on field to the current list of dependencies
      updateParams.DependsOnModifications = new XmlRpcStruct();
      updateParams.DependsOnModifications.Add("set", DependsOn);

      //Set the blocks list to
      updateParams.BlocksModifications = new XmlRpcStruct();
      updateParams.BlocksModifications.Add("set", Blocks);

      //Set the list of keywords
      updateParams.KeywordModifications = new XmlRpcStruct();
      updateParams.KeywordModifications.Add("set", Keywords);
        
      //Add the change comment if specified
      if (!string.IsNullOrEmpty(changeComment))
      {
        updateParams.Comment = new CommentParam();
        updateParams.Comment.CommentText = changeComment;
        updateParams.Comment.IsPrivate = changeCommentPrivate.GetValueOrDefault();
      }

      try
      {
        mProxy.UpdateBug(updateParams);
      }
      catch (XmlRpcFaultException e)
      {
        switch(e.FaultCode)
        {
          case 50:
          case 52:
          case 54:
          case 55:
          case 56:
          case 112:
            throw new InvalidBugFieldValueException(e.FaultString);

          case 115:
            throw new BugEditAccessDeniedException(Id.ToString());

          case 116:
            throw new CyclicBugDependenciesException(e.FaultString);

          case 118:
            throw new CyclicBugDuplicateException(e.FaultString);

          case 121:
          case 122:
          case 119:
            throw new InvalidBugResolutionChangeException(e.FaultString);

          case 120:
            throw new GroupEditAccessDeniedException(e.FaultString);

          case 123:
            throw new InvalidBugStatusTransitionException(e.FaultString);

          default:
            throw new BugzillaException(string.Format("Error updating bug. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Marks this bug as a duplicate of another bug.
    /// </summary>
    /// <param name="duplicateBugID">ID of the bug to mark this bug as a duplicate of.</param>
    /// <param name="changeComment">If set, the text of a comment to add at the same time as updating the CC list.</param>
    /// <param name="changeCommentPrivate">If adding a change comment, indicates whether the comment is private or not.</param>
    public void MarkAsDuplicate(int duplicateBugID, string changeComment, bool? changeCommentPrivate)
    {
      UpdateBugParam updateParams = new UpdateBugParam();
      updateParams.Ids = new int[] { Id };
      updateParams.DuplicateOf = duplicateBugID;

      //Add the change comment if specified
      if (!string.IsNullOrEmpty(changeComment))
      {
        updateParams.Comment = new CommentParam();
        updateParams.Comment.CommentText = changeComment;
        updateParams.Comment.IsPrivate = changeCommentPrivate.GetValueOrDefault();
      }

      try
      {
        mProxy.UpdateBug(updateParams);
      }
      catch (XmlRpcFaultException e)
      {
        switch(e.FaultCode)
        {
          case 115:
            throw new BugEditAccessDeniedException(Id.ToString());

          case 118:
            throw new CyclicBugDuplicateException(e.FaultString);

          default:
            throw new BugzillaException(string.Format("Error marking bug as duplicate. Details: {0}", e.Message));
        }
      }
    }

    #region Private Methods

    /// <summary>
    /// Gets the value for a field as it's nullable type equivalent.
    /// </summary>
    /// <typeparam name="T">The data type of the field.</typeparam>
    /// <param name="fieldKey">Key for the field.</param>
    /// <returns><code>null</code> if the field does not have a value, otherwise the field value wrapped in a nullable type.</returns>
    private T? GetValueTypeFieldValue<T>(string fieldKey) where T : struct
    {
      object val = mBugInfo[fieldKey];

      if (val == null)
        return null;

      return new T?((T)mBugInfo[fieldKey]);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Accessor for the alias.
    /// </summary>
    public string Alias { get { return (string)mBugInfo["alias"]; } }

    /// <summary>
    /// Accessor for the classification.
    /// </summary>
    public string Classification { get { return (string)mBugInfo["classification"]; } }

    /// <summary>
    /// Accessor for the id.
    /// </summary>
    public int Id { get { return (int)mBugInfo["id"]; } }

    /// <summary>
    /// Accessor for the bug's creation date.
    /// </summary>
    public DateTime CreationDate { get { return (DateTime)mBugInfo["creation_time"]; } }

    /// <summary>
    /// Accessor for the update token.
    /// </summary>
    public string UpdateToken { get { return (string)mBugInfo["update_token"]; } }

    /// <summary>
    /// Accesor for the product name.
    /// </summary>
    public string Product 
    {
      get { return (string)mBugInfo["product"]; }
      set { mBugInfo["product"] = value; }
    }

    /// <summary>
    /// Accessor for the user name of the person who reported the bug.
    /// </summary>
    public string ReportedBy { get { return (string)mBugInfo["creator"]; } }

    /// <summary>
    /// Whether the bug is confirmed or not.
    /// </summary>
    public bool IsConfirmed { get { return (bool)mBugInfo["is_confirmed"]; } }

    /// <summary>
    /// Whether the bug is open or not.
    /// </summary>
    public bool IsOpen { get { return (bool)mBugInfo["is_open"]; } }

    /// <summary>
    /// Date/time of the last change to this bug.
    /// </summary>
    public DateTime LastChangeTime { get { return (DateTime)mBugInfo["last_change_time"]; } }

    /// <summary>
    /// Gets/sets the full login name of the person this bug is assigned to.
    /// </summary>
    public string AssignedTo
    {
      get { return (string)mBugInfo["assigned_to"]; }
      set { mBugInfo["assigned_to"] = value; }
    }

    /// <summary>
    /// Accessor for the bug ids this bug blocks.
    /// </summary>
    public int[] Blocks
    {
      get 
      {
        Array arr = (Array)mBugInfo["blocks"];
        return arr.OfType<int>().ToArray();
      }
      set { mBugInfo["blocks"] = value; }
    }

    /// <summary>
    /// Accessor for the bug ids this bug depends on.
    /// </summary>
    public int[] DependsOn
    {
      get 
      {
        Array arr = (Array)mBugInfo["depends_on"];
        return arr.OfType<int>().ToArray();
      }
      set { mBugInfo["depends_on"] = value; }
    }

    /// <summary>
    /// Accessor for the CC list.
    /// </summary>
    public string[] CCList 
    { 
      get 
      {
        Array arr = (Array)mBugInfo["cc"];
        return arr.OfType<string>().ToArray();
      } 
    }

    /// <summary>
    /// Whether members of the CC list can access this bug, even if the groups they belong to don't have access.
    /// </summary>
    public bool AccessibleToCCListMembers
    {
      get 
      {
        object val = mBugInfo["is_cc_accessible"];
        return val == null ? false : (bool)val;
      }
      set { mBugInfo["is_cc_accessible"] = value; }
    }

    /// <summary>
    /// Accessor for the component the bug was logged against.
    /// </summary>
    public string Component
    {
      get { return (string)mBugInfo["component"]; }
      set { mBugInfo["component"] = value; }
    }

    /// <summary>
    /// Day that the bug is due to be completed. Format is 'YYYY-MM-DD'.
    /// </summary>
    public string Deadline
    {
      get { return (string)mBugInfo["deadline"]; }
      set { mBugInfo["deadline"] = value; }
    }

    /// <summary>
    /// ID of the bug this bug is a duplicate of.
    /// </summary>
    public int? DuplicateOf
    {
      get { return GetValueTypeFieldValue<int>("dupe_of"); }
      set { mBugInfo["dupe_of"] = value; }
    }

    /// <summary>
    /// Number of hours estimated this bug will take to fix.
    /// </summary>
    public double? EstimatedResolutionTimeHours
    {
      get { return GetValueTypeFieldValue<double>("estimated_time"); }
      set { mBugInfo["estimated_time"] = value; }
    }

    /// <summary>
    /// The names of all groups this bug is in.
    /// </summary>
    public string[] Groups 
    { 
      get 
      {
        Array arr = (Array)mBugInfo["groups"];
        return arr.OfType<string>().ToArray();
      } 
    }

    /// <summary>
    /// Keywords set on the bug.
    /// </summary>
    public string[] Keywords
    {
      get 
      {
        Array arr = (Array)mBugInfo["keywords"];
        return arr.OfType<string>().ToArray();
      }
      set { mBugInfo["keywords"] = value; }
    }

    /// <summary>
    /// Name of the operating system the bug was filed against.
    /// </summary>
    public string OperatingSystem
    {
      get { return (string)mBugInfo["op_sys"]; }
      set { mBugInfo["op_sys"] = value; }
    }

    /// <summary>
    /// The platform the bug was filed against.
    /// </summary>
    public string Platform
    {
      get { return (string)mBugInfo["platform"]; }
      set { mBugInfo["platform"] = value; }
    }

    /// <summary>
    /// Priority of the bug.
    /// </summary>
    public string Priority
    {
      get { return (string)mBugInfo["priority"]; }
      set { mBugInfo["priority"] = value; }
    }

    /// <summary>
    /// The login name of the current QA Contact on the bug
    /// </summary>
    public string QAContact
    {
      get { return (string)mBugInfo["qa_contact"]; }
      set { mBugInfo["qa_contact"] = value; }
    }

    /// <summary>
    /// If true, this bug can be accessed by the creator (reporter) of the bug, even if he or 
    /// she is not a member of the groups the bug is restricted to.
    /// </summary>
    public bool IsAccessibleByReporter
    {
      get 
      {
        object val = mBugInfo["is_creator_accessible"];
        return val == null ? false : (bool)val;
      }
      set { mBugInfo["is_creator_accessible"] = value; }
    }

    /// <summary>
    /// The current resolution of the bug, or an empty string if the bug is open.
    /// </summary>
    public string Resolution
    {
      get { return (string)mBugInfo["resolution"]; }
      set { mBugInfo["resolution"] = value; }
    }

    /// <summary>
    /// The URLs in the See Also field on the bug.
    /// </summary>
    public string[] SeeAlso 
    { 
      get 
      {
        Array arr = (Array)mBugInfo["see_also"];
        return arr.OfType<string>().ToArray();
      } 
    }

    /// <summary>
    /// The current severity of the bug.
    /// </summary>
    public string Severity
    {
      get { return (string)mBugInfo["severity"]; }
      set { mBugInfo["severity"] = value; }
    }

    /// <summary>
    /// he current status of the bug.
    /// </summary>
    public string Status
    {
      get { return (string)mBugInfo["status"]; }
      set { mBugInfo["status"] = value; }
    }

    /// <summary>
    /// The summary of this bug.
    /// </summary>
    public string Summary
    {
      get { return (string)mBugInfo["summary"]; }
      set { mBugInfo["summary"] = value; }
    }

    /// <summary>
    /// The milestone that this bug is supposed to be fixed by, or for closed bugs, the milestone that it was fixed for.
    /// </summary>
    public string TargetMilestone
    {
      get { return (string)mBugInfo["target_milestone"]; }
      set { mBugInfo["target_milestone"] = value; }
    }

    /// <summary>
    /// A URL that demonstrates the problem described in the bug, or is somehow related to the bug report.
    /// </summary>
    public string URL
    {
      get { return (string)mBugInfo["url"]; }
      set { mBugInfo["url"] = value; }
    }

    /// <summary>
    /// The version the bug was reported against.
    /// </summary>
    public string Version
    {
      get { return (string)mBugInfo["version"]; }
      set { mBugInfo["version"] = value; }
    }

    /// <summary>
    /// The value of the "status whiteboard" field on the bug.
    /// </summary>
    public string StatusWhiteboard
    {
      get { return (string)mBugInfo["whiteboard"]; }
      set { mBugInfo["whiteboard"] = value; }
    }

    /// <summary>
    /// Accessor for the custom fields for this bug.
    /// </summary>
    public BugCustomFields CustomFields
    {
      get { return mCustomFields; }
    }

    #endregion
  }
}
