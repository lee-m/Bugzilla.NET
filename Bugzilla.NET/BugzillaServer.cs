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
using System.Reflection;
using System.Reflection.Emit;

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
    /// Base path to the root Bugzilla landing page - i.e. http://localhost/bugzilla
    /// </summary>
    private readonly string mBugzillaBasePath;

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
    /// If the login's "remember" parameter was set, these are the login cookies.
    /// </summary>
    private CookieCollection mLoginCookies;

    /// <summary>
    /// Series of dymamically created types used when creating new bugs.
    /// </summary>
    /// <remarks>The key is the hash code of all custom fields contained within the type, the value is the dynamically
    /// generated type containing a field within the type for each custom field.</remarks>
    private Dictionary<int, Type> mCreateBugTypes;

    /// <summary>
    /// The type of each custom field which can be set on a bug keyed off the field name.
    /// </summary>
    private List<BugCustomField> mCustomFieldTemplate;

    /// <summary>
    /// Options to be passed to <see cref="GetBugs">GetBugs</see>/>.
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

    #region Public Methods

    /// <summary>
    /// Creates a new instance and logs the specified user into the remote server.
    /// </summary>
    /// <param name="hostName">Host containing the bugzilla server.</param>
    /// <param name="path">The path on the host to the Bugzilla installation.</param>
    /// <param name="userName">Username to log in with.</param>
    /// <param name="password">Password for the specified user.</param>
    /// <param name="remember">Whether the login cookies should expire with the session or not.</param>
    /// <remarks>Calls <see cref="Login"/> with the specified username and password.</remarks>
    public BugzillaServer(string hostName, string path, string userName, string password, bool remember) : this(hostName, path, null)
    {
      Login(userName, password, remember);
      FetchCustomFieldDetails();
    }

    /// <summary>
    /// Overloaded constructor to be used when a login isn't required.
    /// </summary>
    /// <param name="hostName">Host containing the bugzilla server.</param>
    /// <param name="path">The path on the host to the Bugzilla installation.</param>
    /// <param name="loginCookies">Any previously saved login details from a previous session to re-use to avoid having to login again.</param>
    /// <exception cref="ArgumentNullException"><paramref name="hostName"/> is null or empty.</exception>
    public BugzillaServer(string hostName, string path, CookieCollection loginCookies)
    {
      if (string.IsNullOrEmpty(hostName))
        throw new ArgumentNullException("hostName");

      mHostName = hostName;

      if (!string.IsNullOrEmpty(path))
        mBugzillaBasePath = string.Format(@"http://{0}/{1}", hostName, path);
      else
        mBugzillaBasePath = string.Format(@"http://{0}", hostName);

      mURL = string.Format("{0}/xmlrpc.cgi", mBugzillaBasePath);
      mBugProxy = XmlRpcProxyGen.Create<IBugProxy>();
      mUserProxy = XmlRpcProxyGen.Create<IUserProxy>();
      mBugzillaProxy = XmlRpcProxyGen.Create<IBugzillaProxy>();
      mProductProxy = XmlRpcProxyGen.Create<IProductProxy>();
      mCreateBugTypes = new Dictionary<int, Type>();

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

      if (loginCookies != null)
      {
        mLoggedIn = true;
        SetProxyCookies(loginCookies);
        FetchCustomFieldDetails();
      }
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

        mLoginCookies = mUserProxy.CookieContainer.GetCookies(new Uri(mBugzillaBasePath));
        SetProxyCookies(mLoginCookies);

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
            throw new BugzillaException(string.Format("Error logging in. Details: {0}", e.Message));
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
            throw new BugzillaException(string.Format("Error creating new user. Details: {0}", e.Message));
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
    /// <remarks>At least one of <paramref name="ids"/>, <paramref name="loginNames"/> or <paramref name="userMatches"/>
    /// must be non-null and have at least one value.</remarks>
    /// <exception cref="ArgumentException"><paramref name="ids"/>, <paramref name="loginNames"/> 
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
            throw new BugzillaException(string.Format("Error searching users. Details: {0}", e.Message));
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
            throw new BugzillaException(string.Format("Error offering new user account by email. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Creates one or bug instance with the specified ids.
    /// </summary>
    /// <param name="ids">IDs of the bug instances to fetch/create.</param>
    /// <param name="fetchOptions">Whether the fetch the bug's data from the remote server or not.</param>
    /// <returns>A bug instance for the specified ID.</returns>
    /// <exception cref="InvalidOperationException">Attempted to fetch details of the bug from the remote server when a user isn't logged in.</exception>
    /// <exception cref="InvalidBugIDOrAliasException">No bug exists with the specified ID.</exception>
    /// <exception cref="BugAccessDeniedException">Requested bug is inaccessible to the current user.</exception>
    public IEnumerable<Bug> GetBugs(IEnumerable<int> ids, BugFetchOptions fetchOptions)
    {
      if (fetchOptions == BugFetchOptions.NoFetch)
      {
        List<Bug> bugs = new List<Bug>();

        foreach (int id in ids)
        {
          XmlRpcStruct info = new XmlRpcStruct();
          info["id"] = id;

          bugs.Add(new Bug(info, mBugProxy, GetCustomFieldValuesForBug(info)));
        }

        return bugs;
      }
      else
      {
        //Someone must be logged in to fetch bug details from the server
        if (!mLoggedIn)
          throw new InvalidOperationException("A user must be logged in before getting bug details from remote server.");

        GetBugParams getParams = new GetBugParams();
        getParams.IDsOrAliases = ids.Select(id => id.ToString()).ToArray();
        getParams.Permissive = false;

        try
        {
          GetBugsResponse resp = mBugProxy.GetBugs(getParams);
          List<Bug> bugs = new List<Bug>();

          foreach (var info in resp.Bugs)
            bugs.Add(new Bug(info, mBugProxy, GetCustomFieldValuesForBug(info)));

          return bugs;
        }
        catch (XmlRpcFaultException e)
        {
          throw new BugzillaException(string.Format("Error getting bug(s) details. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Creates a new bug.
    /// </summary>
    /// <param name="product">The name of the product the bug is being filed against. Required.</param>
    /// <param name="component">The name of a component in the specified product. Required.</param>
    /// <param name="summary">A brief description of the bug being filed. Required.</param>
    /// <param name="version">The version of the specified product the bug was found in. Required.</param>
    /// <param name="description">The initial description for this bug.</param>
    /// <param name="operatingSystem">The operating system the bug was discovered on.</param>
    /// <param name="platform">What type of hardware the bug was experienced on.</param>
    /// <param name="priority">What order the bug should be fixed by the assignee.</param>
    /// <param name="severity">How severe the bug is.</param>
    /// <param name="alias">A unique alias for the bug used to refer to it by instead of its ID.</param>
    /// <param name="assignedTo">Who to assign the bug to.</param>
    /// <param name="ccList">User names to CC on the bug.</param>
    /// <param name="initialCommentPrivate">Whether the initial comment added to the bug is private or public.</param>
    /// <param name="groups">Which groups to put this bug into.</param>
    /// <param name="qaContact">The QA contact for the bug if the default QA contact is not to be used.</param>
    /// <param name="status">The status that this bug should start out as.</param>
    /// <param name="targetMilestone">A valid target milestone for the specified product.</param>
    /// <param name="dependsOnBugIDs">IDs of any bugs this bug depends on.</param>
    /// <param name="blockedBugIDs">IDs of any bugs this bug blocks.</param>
    /// <param name="estimate">Estimate of how long the bug will take to fix in hours.</param>
    /// <param name="deadline">Deadline date for fixing the bug.</param>
    /// <param name="url">URL for the bug.</param>
    /// <param name="customFields">Set of custom fields to set on the new bug. The key is the field name and the value 
    /// the field's value.</param>
    /// <exception cref="ArgumentNullException"><paramref name="product"/>, <paramref name="component"/>, 
    /// <paramref name="summary"/> or <paramref name="version"/> is null or a blank string.</exception>
    /// <exception cref="ArgumentException"><paramref name="description"/> is longer than 65535 characters.</exception>
    /// <exception cref="FieldCannotBeBlankException">No value was provided for a field which cannot be blank.
    /// Details of which field will be provided in the exception message.</exception>
    /// <exception cref="InvalidFieldValueException">An invalid field name was specified. Details of which field will be in 
    /// the exception message.</exception>
    /// <exception cref="InvalidNewBugAliasException">The alias specified for the new bug was invalid in some way. Details of 
    /// why will be in the exception message.</exception>
    /// <exception cref="InvalidProductException">Specified product does not exist or is inaccessible.</exception>
    /// <exception cref="CyclicBugDependenciesException">One or more of the bugs specified in <paramref name="dependsOnBugIDs"/> or
    /// <paramref name="blockedBugIDs"/> would create a cyclic dependency.</exception>
    /// <exception cref="GroupRestrictionDeniedException">One or more of the groups specified in <paramref name="groups"/> 
    /// does not exist or is inaccessible.</exception>
    /// <exception cref="InvalidUserException">The user name for the assignee, QA contact or a user in the CC list is invalid. 
    /// Additional details of which will be in the exception message.</exception>
    /// <exception cref="InvalidCustomFieldNameException">One of the field names in <paramref name="customFields"/> is invalid. 
    /// Additional details will be in the exception message</exception>
    /// <returns>The ID of the newly created bug.</returns>
    public int CreateBug(string product, 
                         string component, 
                         string summary, 
                         string version, 
                         string description,
                         string operatingSystem, 
                         string platform, 
                         string priority, 
                         string severity, 
                         string alias,
                         string assignedTo, 
                         IEnumerable<string> ccList, 
                         bool initialCommentPrivate, 
                         IEnumerable<string> groups, 
                         string qaContact, 
                         string status, 
                         string targetMilestone,
                         IEnumerable<int> dependsOnBugIDs,
                         IEnumerable<int> blockedBugIDs,
                         double? estimate,
                         DateTime? deadline,
                         string url,
                         BugCustomFields customFields)
    {
      //Check that required parameters have a value
      if (string.IsNullOrEmpty(product))
        throw new ArgumentNullException("product");

      if(string.IsNullOrEmpty(component))
        throw new ArgumentNullException("component");

      if(string.IsNullOrEmpty(summary))
        throw new ArgumentNullException("summary");

      if (string.IsNullOrEmpty(version))
        throw new ArgumentNullException("version");

      //Description must be less than 65636 
      if (description != null && description.Length > ushort.MaxValue)
        throw new ArgumentException("Description must be less than 65535 characters.");

      //Create a type to pass to the proxy based on any custom fields we have and set the common fields.
      CreateBugParams createParams = BugCreateUpdateParamsFactory.Instance.GetCreateBugParamsTypeInstance(customFields);
      createParams.Product = product;
      createParams.Component = component;
      createParams.Summary = summary;
      createParams.Version = version;
      createParams.Description = description;
      createParams.OperatingSystem = operatingSystem;
      createParams.Platform = platform;
      createParams.Priority = priority;
      createParams.Severity = severity;
      createParams.Alias = alias;
      createParams.AssignedTo = assignedTo;
      createParams.IsCommentPrivate = initialCommentPrivate;
      createParams.QAContact = qaContact;
      createParams.Status = status;
      createParams.TargetMilestone = targetMilestone;
      createParams.EstimatedTime = estimate;
      createParams.URL = url;

      if(deadline != null)
        createParams.Deadline = deadline.Value.ToString("yyyy-MM-dd");

      if (ccList != null)
        createParams.CCList = ccList.ToArray();

      if (groups != null)
        createParams.Groups = groups.ToArray();

      if (dependsOnBugIDs != null)
        createParams.DependsOn = dependsOnBugIDs.ToArray();

      if (blockedBugIDs != null)
        createParams.Blocks = blockedBugIDs.ToArray();

      //Set any custom field values
      if (customFields != null)
      {
        Type createParamsType = createParams.GetType();

        foreach (BugCustomField customField in customFields)
          createParamsType.GetField(customField.FieldName).SetValue(createParams, customField.FieldValue);
      }

      try
      {
        CreateBugResponse resp = mBugProxy.CreateNewBug(createParams);
        return resp.Id;
      }
      catch (XmlRpcFaultException e)
      {
        switch(e.FaultCode)
        {
          case 50:
            throw new FieldCannotBeBlankException(e.FaultString);

          case 51:
          case 104:
            throw new InvalidFieldValueException(e.FaultString);

          case 53:
            throw new InvalidCustomFieldNameException(e.FaultString);

          case 103:
            throw new InvalidNewBugAliasException(e.FaultString);

          case 106:
            throw new InvalidProductException(e.FaultString);

          case 116:
            throw new CyclicBugDependenciesException(e.FaultString);

          case 120:
            throw new GroupRestrictionDeniedException();

          case 504:
            throw new InvalidUserException(e.FaultString);

          default:
            throw new BugzillaException(string.Format("Error creating new bug. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Adds an attachment to multiple bugs in a single operation.
    /// </summary>
    /// <param name="idsOrAliases">IDs and/or aliases of bugs to add the attachment to.</param>
    /// <param name="fileName">Name of the file to attach.</param>
    /// <param name="summary">Short summary text of the attachment. Mandatory</param>
    /// <param name="comment">Comment text to add along with the attachment.</param>
    /// <param name="isPatch">Whether the attachment is a patch file or not.</param>
    /// <param name="isPrivate">Whether the attachment should be private or not.</param>
    /// <returns>Details of the newly created attachments.</returns>
    /// <remarks>The MIME type will be automatically determined from either the extension of the file, or it's data..</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="fileName"/> is null or blank.</exception>
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
    /// <exception cref="ArgumentNullException"><paramref name="attachmentData"/> not specified.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="summary"/> is null or blank.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="mimeType"/> is null or blank.</exception>
    /// <exception cref="AttachmentTooLargeException">Size of the attachment is too large.</exception>
    /// <exception cref="InvalidMIMETypeException"><paramref name="mimeType"/> is invalid.</exception>
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
            throw new BugzillaException(string.Format("Error adding attachment to bug(s). Details: {0}", e.Message));
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
    /// <exception cref="InvalidOperationException"><paramref name="bugIDsOrAliases"/> and <paramref name="commentIDs"/> are null.</exception>
    /// <exception cref="CommentAccessDeniedException">One or more of the requested comments are inaccessible to the current user.</exception>
    /// <exception cref="InvalidCommentIDException">One or more invalid comment IDs specified.</exception>
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
            throw new BugzillaException(string.Format("Error getting comments for bug. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Gets attachment for each bug ID/aliases and specific attachment IDs.
    /// </summary>
    /// <param name="bugIDsOrAliases">IDs or aliases of the bugs to get the attachments for.</param>
    /// <param name="attachmentIDs">IDs of particular attachment to fetch.</param>
    /// <returns>Details for the requested attachments.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="bugIDsOrAliases"/> and <paramref name="attachmentIDs"/> are null.</exception>
    /// <exception cref="InvalidBugIDOrAliasException">One or more invalid bug IDs/aliases specified.</exception>
    /// <exception cref="BugAccessDeniedException">Current user does not have access one or more of the specified bugs.</exception>
    /// <exception cref="AttachmentAccessDeniedException">One or more of the specified attachments are private but the current user is not in the "insiders" group.</exception>
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
            throw new BugzillaException(string.Format("Error getting attachments. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Adds or removes URLs from the "see also" field on the bug.
    /// </summary>
    /// <param name="bugIDsOrAliases">IDs and/or aliases of the bugs to update.</param>
    /// <param name="urlsToAdd">URLs to add. If a URL does not start with "http://" or "https://" then "http://" will be automatically added.</param>
    /// <param name="urlsToRemove">URLs to remove.</param>
    /// <exception cref="InvalidBugIDOrAliasException">One or more invalid bug IDs specified.</exception>
    /// <exception cref="BugAccessDeniedException">One of more specified bugs are inaccessible to the current user.</exception>
    /// <exception cref="InvalidSeeAlsoURLException">One or more of the specified URLs are invalid.</exception>
    /// <exception cref="SeeAlsoEditAccessDenied">Currently logged in user does not have security access to edit one or 
    /// more of the specified bug's see also field.</exception>
    /// <returns>Details of the actual changes which were made to the specified bugs.</returns>
    /// <remarks>
    /// <para>
    /// Attempting to add an already added URL or attempts to remove an invalid URL will be silently ignored.
    /// </para>
    /// If the same URL is specified in both <paramref name="urlsToAdd"/> and <paramref name="urlsToRemove"/> it will be <b>added</b>.
    /// <para>
    /// </para>
    /// </remarks>
    public List<SeeAlsoModifications> UpdateSeeAlsoURLs(IEnumerable<string> bugIDsOrAliases,
                                                        IEnumerable<string> urlsToAdd,
                                                        IEnumerable<string> urlsToRemove)
    {
      if (bugIDsOrAliases == null)
        throw new ArgumentNullException("bugIDsOrAliases");

      UpdateSeeAlsoParams updateParams = new UpdateSeeAlsoParams();
      updateParams.IdsOrAliases = bugIDsOrAliases.ToArray();

      if (urlsToAdd != null)
        updateParams.URLsToAdd = urlsToAdd.ToArray();

      if (urlsToRemove != null)
        updateParams.URLsToRemove = urlsToRemove.ToArray();

      try
      {
        UpdateSeeAlsoResponse resp = mBugProxy.UpdateSeeAlso(updateParams);
        List<SeeAlsoModifications> retList = new List<SeeAlsoModifications>();

        foreach (object key in resp.Changes.Keys)
        {
          XmlRpcStruct mods = (XmlRpcStruct)((XmlRpcStruct)resp.Changes[key])["see_also"];

          int bugID = int.Parse(key.ToString());
          object[] added = (object[])mods["added"];
          object[] removed = (object[])mods["removed"];

          retList.Add(new SeeAlsoModifications(bugID, added.Cast<string>().ToArray(), removed.Cast<string>().ToArray()));
        }

        return retList;
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

          case 119:
            throw new BugEditAccessDeniedException();

          case 112:
            throw new InvalidSeeAlsoURLException();

          case 115:
            throw new SeeAlsoEditAccessDenied();

          default:
            throw new BugzillaException(string.Format("Error updating see/also field for bug(s). Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Gets a custom fields "template" ready for each field's value to be set for creating new bugs.
    /// </summary>
    /// <remarks>Each call to this method will return back a new <see cref="BugCustomFields"/> instance.</remarks>
    /// <returns>A new <see cref="BugCustomFields"/> template without any field values set.</returns>
    public BugCustomFields GetNewBugCustomFieldsTemplate()
    {
      List<BugCustomField> copiedCustomFields = new List<BugCustomField>();

      foreach (BugCustomField customField in mCustomFieldTemplate)
        copiedCustomFields.Add(new BugCustomField(customField.FieldName, customField.FieldType));

      return new BugCustomFields(copiedCustomFields);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Fetches details of allowable custom fields from the remote server.
    /// </summary>
    private void FetchCustomFieldDetails()
    {
      //Only interested in the name, whether the field is custom or not and the type.
      GetFieldsParam getFieldsParam = new GetFieldsParam();
      getFieldsParam.IncludeFields = new string[] { "type", "is_custom", "name" };

      GetFieldsResponse getFieldsResp = mBugProxy.GetValidFields(getFieldsParam);
      mCustomFieldTemplate = new List<BugCustomField>();

      foreach (var field in getFieldsResp.Fields)
      {
        if (field.IsCustomField)
          mCustomFieldTemplate.Add(new BugCustomField(field.InternalName, (BugFieldDetails.BugFieldType)field.Type));
      }
    }

    /// <summary>
    /// Gets a set of bug custom field instances initialised to each field's value.
    /// </summary>
    /// <param name="bugFieldInfo">Bug field values to extract the custom field values from.</param>
    /// <returns>A <see cref="BugCustomFields"/> instance populated with each custom field's details.</returns>
    private BugCustomFields GetCustomFieldValuesForBug(XmlRpcStruct bugFieldInfo)
    {
      List<BugCustomField> customFields = new List<BugCustomField>();

      foreach (BugCustomField customField in mCustomFieldTemplate)
        customFields.Add(new BugCustomField(customField.FieldName, 
                                            customField.FieldType, 
                                            bugFieldInfo[customField.FieldName]));

      return new BugCustomFields(customFields);
    }
    /// <summary>
    /// Adds each cookie in the collection to each proxy's cookie container.
    /// </summary>
    /// <param name="cookies">Collection of cookies to add to each proxy.</param>
    private void SetProxyCookies(CookieCollection cookies)
    {
      foreach (Cookie cookie in cookies)
      {
        mBugProxy.CookieContainer.Add(cookie);
        mBugzillaProxy.CookieContainer.Add(cookie);
        mProductProxy.CookieContainer.Add(cookie);
      }
    }

    #endregion

    #region Public Properties

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

    /// <summary>
    /// Accessor for the login cookies to remember the login across sessions.
    /// </summary>
    public CookieCollection LoginCookies
    {
      get { return mLoginCookies; }
    }

    #endregion
  }
}