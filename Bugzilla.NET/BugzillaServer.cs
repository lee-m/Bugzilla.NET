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
using System.Collections.Generic;
using System.Text;
using System.Net;

using CookComputing.XmlRpc;

using Bugzilla.Utils;
using Bugzilla.Proxies.Bug;
using Bugzilla.Proxies.Bug.Params;
using Bugzilla.Proxies.Bug.Responses;
using Bugzilla.Proxies.Bugzilla;
using Bugzilla.Proxies.Bugzilla.Responses;
using Bugzilla.Proxies.User;
using Bugzilla.Proxies.User.Params;
using Bugzilla.Proxies.User.Responses;
using Bugzilla.Proxies.Product;
using Bugzilla.Proxies.Product.Responses;
using Bugzilla.Proxies.Product.Params;

namespace Bugzilla
{
  /// <summary>
  /// Represents a connection to a remove Bugzilla server.
  /// </summary>
  public class BugzillaServer
  {
    /// <summary>
    /// Host name of the server on which Bugzilla sits without any protocol info - i.e. "localhost" rather than
    /// "http://localhost"
    /// </summary>
    private readonly string mHostName;

    /// <summary>
    /// Complete URL to the XMLRPC.cgi script on the bugzilla server.
    /// </summary>
    private readonly string mURL;

    /// <summary>
    /// Proxy for interacting with bugs.
    /// </summary>
    private readonly IBugProxy mBugProxy;

    /// <summary>
    /// Proxy for interacting with users.
    /// </summary>
    private readonly IUserProxy mUserProxy;

    /// <summary>
    /// Proxy for interacting with the Bugzilla web service API.
    /// </summary>
    private readonly IBugzillaProxy mBugzillaProxy;

    /// <summary>
    /// Proxy for interacting with the product web service API.
    /// </summary>
    private readonly IProductProxy mProductProxy;

    /// <summary>
    /// Whether a user is logged in or not.
    /// </summary>
    private bool mLoggedIn;

    /// <summary>
    /// Options to be passed to <see cref="GetBug">GetBug</see>/>.
    /// </summary>
    public enum BugFetchOptions
    {
      /// <summary>
      /// Fetch the bug's data from the remote server.
      /// </summary>
      FetchData = 0,

      /// <summary>
      /// Don't fetch the bug's data. Simply initialises the bug instance with the given ID.
      /// </summary>
      NoFetch = 1
    }

    /// <summary>
    /// Creates a new instance and logs the specified user into the remote server.
    /// </summary>
    /// <param name="hostName">Host containing the bugzilla server.</param>
    /// <param name="path">The path on the host to the Bugzilla installation.</param>
    /// <param name="userName">Username to log in with.</param>
    /// <param name="password">Password for the specified user.</param>
    /// <param name="remember">Whether the login cookies should expire with the session or not.</param>
    public BugzillaServer(string hostName, string path, string userName, string password, bool remember) : this(hostName, path)
    {
      if (string.IsNullOrEmpty(hostName))
        throw new ArgumentNullException("hostName");

      Login(userName, password, remember);
    }

    /// <summary>
    /// Overloaded constructor to be used when a login isn't required.
    /// </summary>
    /// <param name="hostName">Host containing the bugzilla server.</param>
    /// <param name="path">The path on the host to the Bugzilla installation.</param>
    public BugzillaServer(string hostName, string path)
    {
      if (string.IsNullOrEmpty(hostName))
        throw new ArgumentNullException("hostName");

      mHostName = hostName;

      if (!string.IsNullOrEmpty(path))
        mURL = string.Format(@"http://{0}/{1}/xmlrpc.cgi", hostName, path);
      else
        mURL = string.Format(@"http://{0}/xmlrpc.cgi", hostName);

      mBugProxy = XmlRpcProxyGen.Create<IBugProxy>();
      mUserProxy = XmlRpcProxyGen.Create<IUserProxy>();
      mBugzillaProxy = XmlRpcProxyGen.Create<IBugzillaProxy>();
      mProductProxy = XmlRpcProxyGen.Create<IProductProxy>();

#if DEBUG

      RequestResponseTracer tracer = new RequestResponseTracer();
      tracer.Attach(mBugProxy);
      tracer.Attach(mUserProxy);
      tracer.Attach(mBugzillaProxy);
      tracer.Attach(mProductProxy);

#endif

      mBugProxy.Url = mURL;
      mUserProxy.Url = mURL;
      mBugzillaProxy.Url = mURL;
      mProductProxy.Url = mURL;
    }

    /// <summary>
    /// Logs a user into the Bugzilla server.
    /// </summary>
    /// <param name="userName">Username to log in with.</param>
    /// <param name="password">Password for the specified user.</param>
    /// <param name="remember">Whether the login cookies should expire with the session or not.</param>
    /// <returns>The ID of the user logged in.</returns>
    /// <exception cref="InvalidOperationException">A user is already logged in.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="userName"/> or <paramref name="password"/> was null or blank.</exception>
    /// <exception cref="InvalidLoginDetailsException">Invalid username and/or password specified.</exception>
    /// <exception cref="DisabledAccountException">Specified account has been disabled.</exception>
    /// <exception cref="ExpiredPasswordException">User for the specified account has requested to change their password.</exception>
    public int Login(string userName, string password, bool remember)
    {
      if (string.IsNullOrEmpty(userName))
        throw new ArgumentNullException("userName");

      if (string.IsNullOrEmpty(password))
        throw new ArgumentNullException("password");

      if (mLoggedIn)
        throw new InvalidOperationException("Already logged in.");

      try
      {
        //Log the user in and pass the session cookie over to the other proxies
        UserIDResponse resp = mUserProxy.Login(new LoginParam { LoginName = userName, Password = password, Remember = remember });
        mLoggedIn = true;

        foreach (Cookie Cookie in mUserProxy.CookieContainer.GetCookies(new Uri("http://" + mHostName)))
        {
          mBugProxy.CookieContainer.Add(Cookie);
          mBugzillaProxy.CookieContainer.Add(Cookie);
          mProductProxy.CookieContainer.Add(Cookie);
        }

        return resp.Id;
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case (int)UserProxyErrorCodes.InvalidUserNamePassword:
            throw new InvalidLoginDetailsException();

          case (int)UserProxyErrorCodes.AccountDisabled:
            throw new DisabledAccountException();

          case (int)UserProxyErrorCodes.PasswordExpired:
            throw new ExpiredPasswordException();

          default:
            throw;
        }
      }
    }

    /// <summary>
    /// Logs out the currently logged in user.
    /// </summary>
    /// <remarks>No-op if no-one is currently logged in.</remarks>
    public void Logout()
    {
      if (mLoggedIn)
      {
        mUserProxy.Logout();
        mLoggedIn = false;
      }
    }

    /// <summary>
    /// Gets details of a particular product by its ID.
    /// </summary>
    /// <param name="productId">ID of the product to query for.</param>
    /// <returns>Details for the requested product or null if no product exists with the specified ID.</returns>
    public Product GetProduct(int productId)
    {
      return GetProducts(new int[] { productId }).FirstOrDefault();
    }

    /// <summary>
    /// Queries for a set of products based on their IDs.
    /// </summary>
    /// <param name="productIds">roduct IDs to query for.</param>
    /// <returns>A list of product details for each requested product.</returns>
    public List<Product> GetProducts(IEnumerable<int> productIds)
    {
      GetProductsParams prodParams = new GetProductsParams();
      prodParams.Ids = productIds.ToArray();

      GetProductsResponse response = mProductProxy.GetProducts(prodParams);
      return response.Products.Select((dets) => new Product(dets)).ToList();
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="email">Email address of the new user (required).</param>
    /// <param name="password">Password for the new user (optional).</param>
    /// <param name="fullName">Full name of the new user (optional).</param>
    /// <returns>The ID of the newly created user.</returns>
    /// <exception cref="DuplicateAccountException">An account already exists with the specified email address.</exception>
    /// <exception cref="IllegalEmailAddressException">An illegal email address was specified or account creation is disabled.</exception>
    /// <exception cref="PasswordTooShortException">The length of the password is too short.</exception>
    public int CreateNewUser(string email, string password, string fullName)
    {
      if (string.IsNullOrEmpty(email))
        throw new ArgumentNullException("email");

      CreateUserParams newUserParams = new CreateUserParams();
      newUserParams.Email = email;
      newUserParams.Password = password;
      newUserParams.FullName = fullName;

      try
      {
        UserIDResponse resp = mUserProxy.CreateNewUser(newUserParams);
        return resp.Id;
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case (int)UserProxyErrorCodes.AccountAlreadyExists:
            throw new DuplicateAccountException();

          case (int)UserProxyErrorCodes.IllegalEmailAddress:
            throw new IllegalEmailAddressException();

          case (int)UserProxyErrorCodes.PasswordTooShort:
            throw new PasswordTooShortException();

          default:
            throw;
        }
      }
    }

    /// <summary>
    /// Searches for users which matches any of the specified search parameters.
    /// </summary>
    /// <param name="groupIds">IDs of any group the users belongs to.</param>
    /// <param name="groupNames">Names of any group the users belongs to.</param>
    /// <param name="ids">IDs of the users.</param>
    /// <param name="loginNames">Login names of the users to find.</param>
    /// <param name="userMatches">User match strings - login or real names to match on.</param>
    /// <param name="includeDisabled">Whether to include disabled users in the search results.</param>
    /// <returns>A list of user details which matches any of the specified search parameters.</returns>
    /// <remarks>At least one of <paramref name="ids"/></remarks>, <paramref name="loginNames"/> or <paramref name="userMatches"/>
    /// must be non-null and have at least one value.
    /// <exception cref="ArgumentException"><paramref name="ids"/></remarks>, <paramref name="loginNames"/> 
    /// or <paramref name="userMatches"/> are all null or have no values.</exception>
    /// <exception cref="InvalidLoginOrGroupNameException">One or more user IDs or group names specified is invalid.</exception>
    /// <exception cref="UserAccessDeniedException">One or more of the requested users are not accessible to the currently logged in user.</exception>
    /// <exception cref="InvalidOperationException">Logged out users cannot use the <paramref name="userMatches"/> functionality.</exception>
    public List<User> SearchUsers(int[] groupIds, 
                                  string[] groupNames, 
                                  int[] ids, 
                                  string[] loginNames, 
                                  string[] userMatches, 
                                  bool includeDisabled)
    {
      //Bugzilla requires that at least one of these be set.
      if ((ids == null 
           || ids.Length == 0)
          && (loginNames == null 
             || loginNames.Length == 0)
          && (userMatches == null 
             || userMatches.Length == 0))
        throw new ArgumentException("At least one user ID, login name or user match must be specified.");

      GetUserParams searchParams = new GetUserParams();
      searchParams.GroupIds = groupIds;
      searchParams.Groups = groupNames;
      searchParams.Ids = ids;
      searchParams.IncludeDisabled = includeDisabled;
      searchParams.Names = loginNames;
      searchParams.UserMatches = userMatches;

      try
      {
        return mUserProxy.GetUserDetails(searchParams).Users.Select((dets) => new User(dets)).ToList();
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case (int)UserProxyErrorCodes.BadLoginOrGroupName:
            throw new InvalidLoginOrGroupNameException();

          case (int)UserProxyErrorCodes.AuthorisationRequired:
            throw new UserAccessDeniedException();

          case (int)UserProxyErrorCodes.UserMatchingDenied:
            throw new InvalidOperationException("Logged-out users cannot use the user matching functionality.");

          default:
            throw;
        }
      }
    }

    /// <summary>
    /// Sends an invitation email to the specified email address with a link to create a new account.
    /// </summary>
    /// <param name="email">Email address to send the invitation to.</param>
    /// /// <exception cref="DuplicateAccountException">An account already exists with the specified email address.</exception>
    /// <exception cref="IllegalEmailAddressException">An illegal email address was specified or account creation is disabled.</exception>
    public void OfferNewUserAccount(string email)
    {
      if(string.IsNullOrEmpty(email))
        throw new ArgumentNullException("email");

      try
      {
        mUserProxy.OfferAccountByEmail(new OfferAccountParam() { Email = email });
      }
      catch(XmlRpcFaultException e)
      {
        switch(e.FaultCode)
        {
          case (int)UserProxyErrorCodes.AccountAlreadyExists:
            throw new DuplicateAccountException();

          case (int)UserProxyErrorCodes.IllegalEmailAddress:
            throw new IllegalEmailAddressException();

          default:
            throw;
        }
      }
    }

    /// <summary>
    /// Creates a bug instance with the specified id without fetching any details from the remote server/
    /// </summary>
    /// <param name="id">ID of the bug instance.</param>
    /// <param name="fetchOptions">Whether the fetch the bug's data from the remote server or not.</param>
    /// <returns>A bug instance for the specified ID.</returns>
    public Bug GetBug(int id, BugFetchOptions fetchOptions)
    {
      if (fetchOptions == BugFetchOptions.NoFetch)
      {
        BugInfo info = new BugInfo();
        info.Id = id;

        return new Bug(info, mBugProxy);
      }
      else
      {
        GetBugParams getParams = new GetBugParams();
        getParams.IDsOrAliases = new string[] { id.ToString() };
        getParams.Permissive = false;

        try
        {
          return new Bug(mBugProxy.GetBugs(getParams).Bugs.First(), mBugProxy);
        }
        catch (XmlRpcFaultException e)
        {
          switch (e.FaultCode)
          {
            case 100:
            case 101:
              throw new InvalidBugIDOrAliasException(id.ToString());

            case 102:
              throw new BugAccessDeniedException();

            default:
              throw;
          }
        }
      }
    }

    /// <summary>
    /// Adds an attachment to multiple bugs in a single operation.
    /// </summary>
    /// <param name="fileName">Name of the file to attach.</param>
    /// <param name="summary">Short summary text of the attachment. Mandatory</param>
    /// <param name="comment">Comment text to add along with the attachment.</param>
    /// <param name="isPatch">Whether the attachment is a patch file or not.</param>
    /// <param name="isPrivate">Whether the attachment should be private or not.</param>
    /// <returns>Details of the newly created attachments.</returns>
    /// <remarks>The MIME type will be automatically determined from either the extension of the file, or it's data..</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="fileName">fileName</paramref> is null or blank.</exception>
    public List<Attachment> AddAttachmentToBugs(IEnumerable<string> idsOrAliases,
                                                string fileName, 
                                                string summary, 
                                                string comment, 
                                                bool isPatch, 
                                                bool isPrivate)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      return AddAttachmentToBugs(idsOrAliases, 
                                 File.ReadAllBytes(fileName), 
                                 Path.GetFileName(fileName), 
                                 summary,
                                 MIMETypes.GetMIMEType(fileName), 
                                 comment, isPatch, isPrivate);
    }

    /// <summary>
    /// Adds an attachment to multiple bugs at once.
    /// </summary>
    /// <param name="idsOrAliases">Numeric IDs or aliases of the bugs to add the attachment to</param>
    /// <param name="attachmentData">Data for the attachment. Mandatory.</param>
    /// <param name="fileName">Name of the file to show in the UI.</param>
    /// <param name="summary">Short summary text of the attachment. Mandatory</param>
    /// <param name="mimeType">MIME type of the attachment. Mandatory</param>
    /// <param name="comment">Comment text to add to each bug along with the attachment.</param>
    /// <param name="isPatch">Whether the attachment is a patch file or not.</param>
    /// <param name="isPrivate">Whether the attachment should be private or not.</param>
    /// <returns>Details of the newly created attachments.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="attachmentData">Attachment data</paramref> not specified.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="summary">summary</paramref> is null or blank.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="mimeType">mimeType</paramref> is null or blank.</exception>
    public List<Attachment> AddAttachmentToBugs(IEnumerable<string> idsOrAliases,
                                                byte[] attachmentData, 
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

      AddAttachmentParams attachmentParams = new AddAttachmentParams();
      attachmentParams.Comment = comment;
      attachmentParams.Data = attachmentData;
      attachmentParams.FileName = fileName;
      attachmentParams.IdsOrAliases = idsOrAliases.ToArray();
      attachmentParams.IsPatch = isPatch;
      attachmentParams.IsPrivate = isPrivate;
      attachmentParams.MIMEType = mimeType;
      attachmentParams.Summary = summary;

      try
      {
        AddAttachmentResponse resp = mBugProxy.AddAttachment(attachmentParams);
        List<Attachment> retColl = new List<Attachment>();

        foreach (var attachmentID in resp.Attachments.Keys)
          retColl.Add(new Attachment((XmlRpcStruct)resp.Attachments[attachmentID]));

        return retColl;
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 600:
            throw new AttachmentTooLargeException();

          case 601:
            throw new InvalidMIMETypeException(mimeType);

          default:
            throw;

        }
      }
    }

    /// <summary>
    /// Gets comments for each bug ID/aliases and specific comment IDs.
    /// </summary>
    /// <param name="bugIDsOrAliases">IDs or aliases of the bugs to get the comments for.</param>
    /// <param name="commentIDs">IDs of specific comments to get.</param>
    /// <param name="startDate">If set, the date/time to get comments which were posted on or after that date.</param>
    /// <returns>Comment details for the requested comments.</returns>
    /// <exception cref="InvaildOperationException"><paramref name="bugIDsOrAliases"/>and <paramref name="commentIDs"/>are null.</exception>
    public CommentCollection GetComments(IEnumerable<string> bugIDsOrAliases, 
                                          IEnumerable<int> commentIDs,
                                          DateTime? startDate)
    {
      //Either bug IDs/aliases or comment IDs must be set
      if (bugIDsOrAliases == null && commentIDs == null)
        throw new InvalidOperationException("At least one of bug IDs/aliases or comment IDs must be provided.");

      //Fill in the request params
      GetCommentParams commentParams = new GetCommentParams();

      if(bugIDsOrAliases != null)
        commentParams.IdsOrAliases = bugIDsOrAliases.ToArray();

      if(commentIDs != null)
        commentParams.CommentIDs = commentIDs.ToArray();

      commentParams.NewSince = startDate;

      //Run the command on the server
      try
      {
        GetCommentsResponse resp = mBugProxy.GetComments(commentParams);

        //Parse the bug comments
        CommentCollection retColl = new CommentCollection();

        foreach (var bugID in resp.BugComments.Keys)
        {
          XmlRpcStruct commentsHash = (XmlRpcStruct)resp.BugComments[bugID];

          foreach (var comment in (object[])commentsHash["comments"])
          {
            Comment comm = new Comment((XmlRpcStruct)comment);
          
            if(!retColl.BugComments.ContainsKey(comm.BugID))
              retColl.BugComments.Add(comm.BugID, new List<Comment>());

            retColl.BugComments[comm.BugID].Add(comm);
          }
        }

        //Parse the comments collection
        foreach (var commentID in resp.Comments.Keys)
        {
          Comment comm = new Comment((XmlRpcStruct)resp.Comments[commentID]);
          retColl.Comments.Add(comm);
        }

        return retColl;
      }
      catch(XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 110:
            throw new CommentAccessDeniedException();

          case 111:
            throw new InvalidCommentIDException();

          default:
            throw;
        }
      }
    }

    /// <summary>
    /// Gets attachment for each bug ID/aliases and specific attachment IDs.
    /// </summary>
    /// <param name="bugIDsOrAliases">IDs or aliases of the bugs to get the attachments for.</param>
    /// <param name="commentIDs">IDs of specific attachments to get.</param>
    /// <returns>Details for the requested attachments.</returns>
    /// <exception cref="InvaildOperationException"><paramref name="bugIDsOrAliases"/>and <paramref name="commentIDs"/>are null.</exception>
    public AttachmentCollection GetAttachments(IEnumerable<string> bugIDsOrAliases, IEnumerable<int> attachmentIDs)
    {
      //Either bug IDs/aliases or comment IDs must be set
      if (bugIDsOrAliases == null && attachmentIDs == null)
        throw new InvalidOperationException("At least one of bug IDs/aliases or comment IDs must be provided.");

      //Fill in the request parameters
      GetAttachmentsParam attachentParams = new GetAttachmentsParam();

      if (bugIDsOrAliases != null)
        attachentParams.BugIDsOrAliases = bugIDsOrAliases.ToArray();

      if (attachmentIDs != null)
        attachentParams.AttachmentIDs = attachmentIDs.ToArray();

      try
      {
        GetAttachmentsResponse resp = mBugProxy.GetAttachments(attachentParams);

        //Parse the attachments
        AttachmentCollection retColl = new AttachmentCollection();

        foreach (var bugID in resp.BugAttachments.Keys)
        {
          XmlRpcStruct commentsHash = (XmlRpcStruct)resp.BugAttachments[bugID];

          foreach (var attachDets in (object[])commentsHash["comments"])
          {
            Attachment attachment = new Attachment((XmlRpcStruct)attachDets);

            if (!retColl.BugAttachments.ContainsKey(attachment.BugID))
              retColl.BugAttachments.Add(attachment.BugID, new List<Attachment>());

            retColl.BugAttachments[attachment.BugID].Add(attachment);
          }
        }

        //Parse the attachments collection
        foreach (var attachmentID in resp.Attachments.Keys)
        {
          Attachment attachment = new Attachment((XmlRpcStruct)resp.Attachments[attachmentID]);
          retColl.Attachments.Add(attachment);
        }

        return retColl;
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 100:
          case 101:
            throw new InvalidBugIDOrAliasException();

          case 102:
            throw new BugAccessDeniedException();

          case 304:
            throw new AttachmentAccessDeniedException();

          default:
            throw;
        }
      }
    }

    /// <summary>
    /// Accessor for the Bugzilla server version string.
    /// </summary>
    public string ServerVersion
    {
      get
      {
        return mBugzillaProxy.GetVersion().version;
      }
    }

    /// <summary>
    /// Accessor for time information about the Bugzilla server.
    /// </summary>
    public ServerTime ServerTime
    {
      get
      {
        return new ServerTime(mBugzillaProxy.GetTime());
      }
    }

    /// <summary>
    /// Accessor for the set of installed extensions.
    /// </summary>
    public List<Extension> InstalledExtensions
    {
      get
      {
        List<Extension> extensions = new List<Extension>();
        GetExtensionsResponse resp = mBugzillaProxy.GetExtensions();

        foreach (string ext in resp.Extensions.Keys)
        {
          //The key is the name of the extension. The value is another XmlRpcStruct with a single
          //key "version" which is the version of the extension.
          XmlRpcStruct Version = (XmlRpcStruct)resp.Extensions[ext];
          extensions.Add(new Extension(ext, Version["version"].ToString()));
        }

        return extensions;
      }
    }

    /// <summary>
    /// Accessor for the set of selectable product details.
    /// </summary>
    public List<Product> SelectableProducts
    {
      get
      {
        ProductIDsResponse selectableProdIDs = mProductProxy.GetSelectableProducts();
        return GetProducts(selectableProdIDs.Ids);
      }
    }

    /// <summary>
    /// Accessor for the set of enterable product details.
    /// </summary>
    public List<Product> EnterableProducts
    {
      get
      {
        ProductIDsResponse selectableProdIDs = mProductProxy.GetEnterableProducts();
        return GetProducts(selectableProdIDs.Ids);
      }
    }

    /// <summary>
    /// Accessor for the set of accessible product details for the currently logged in user.
    /// </summary>
    public List<Product> AccessibleProducts
    {
      get
      {
        ProductIDsResponse selectableProdIDs = mProductProxy.GetAccessibleProducts();
        return GetProducts(selectableProdIDs.Ids);
      }
    }
  }
}