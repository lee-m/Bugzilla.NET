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

using Bugzilla.Proxies.Bug.Params;
using Bugzilla.Proxies.Bug.Responses;

namespace Bugzilla.Proxies.Bug
{
  /// <summary>
  /// Bug level proxy that exposes bug level operations provided by Bugzilla's web service API.
  /// </summary>
  [XmlRpcProxyAssemblyName("IBugProxy")]
  internal interface IBugProxy : IXmlRpcProxy
  {
    /// <summary>
    /// Updates the specified bug.
    /// </summary>
    /// <param name="updateInfo">New field values for the bug to set.</param>
    [XmlRpcMethod("Bug.update")]
    void UpdateBug(UpdateBugParam updateInfo);

    /// <summary>
    /// Adds a new comment to the specified bug.
    /// </summary>
    /// <param name="commentParam"></param>
    [XmlRpcMethod("Bug.add_comment")]
    AddCommentResponse AddComment(AddCommentParam commentParam);

    /// <summary>
    /// Searches for a set of bugs which match the specified criteria.
    /// </summary>
    /// <param name="searchParams">Search criteria.</param>
    /// <returns>A list of bugs which match the specified criteria.</returns>
    [XmlRpcMethod("Bug.search")]
    BugSearchResponse Search(BugSearchParam searchParams);

    /// <summary>
    /// Adds or removes URLs for the "See Also" field on bugs. 
    /// </summary>
    /// <param name="updateParams">Parameters for the update.</param>
    /// <returns>Details of the modifications that were actually performed.</returns>
    [XmlRpcMethod("Bug.update_see_also")]
    UpdateSeeAlsoResponse UpdateSeeAlso(UpdateSeeAlsoParams updateParams);

    /// <summary>
    /// Adds an attachment to a bug.
    /// </summary>
    /// <param name="addParams"></param>
    [XmlRpcMethod("Bug.add_attachment")]
    AddAttachmentResponse AddAttachment(AddAttachmentParams addParams);

    /// <summary>
    /// Gets a set of comments based on the comment IDs or a set of bug IDs/aliases.
    /// </summary>
    /// <param name="commentParams">Parameters for retrieving the required comments.</param>
    [XmlRpcMethod("Bug.comments")]
    GetCommentsResponse GetComments(GetCommentParams commentParams);

    /// <summary>
    /// Gets a set of attachments based on attachment IDs or a set of bug IDs/aliases
    /// </summary>
    /// <param name="attachmentParams"></param>
    /// <returns></returns>
    [XmlRpcMethod("Bug.attachments")]
    GetAttachmentsResponse GetAttachments(GetAttachmentsParam attachmentParams);

    /// <summary>
    /// Gets details of a selection of bugs.
    /// </summary>
    /// <param name="getParams">Ids/aliases to get the bug details for.</param>
    /// <returns></returns>
    [XmlRpcMethod("Bug.get")]
    GetBugsResponse GetBugs(GetBugParams getParams);

    /// <summary>
    /// Gets details of all changes made to a bug.
    /// </summary>
    /// <param name="historyParams">ID/aliases of the bugs to get the history for.</param>
    /// <returns></returns>
    [XmlRpcMethod("Bug.history")]
    GetBugHistoryResponse GetHistory(GetBugHistoryParams historyParams);

    /// <summary>
    /// Creates a new bug with the specified parameters.
    /// </summary>
    /// <param name="createParams">Parameters for the new bug.</param>
    /// <returns>The ID of the newly created bug.</returns>
    /// <remarks>The parameters are typed as <c>object</c> as the actual type passed in at run-time may be
    /// dynamically generated to handle the set of custom fields being set on the new bug.</remarks>
    [XmlRpcMethod("Bug.create")]
    CreateBugResponse CreateNewBug(object createParams);
  }
}