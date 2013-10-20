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
using Bugzilla.Proxies.Group;
using Bugzilla.Proxies.Group.Params;
using Bugzilla.Proxies.Group.Responses;

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
    /// Proxy for interacting with the group web service API.
    /// </summary>
    private readonly IGroupProxy mGroupProxy;

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
    /// Max length of a product name.
    /// </summary>
    private const int ProductNameMaxLength = 64;

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
      mGroupProxy = XmlRpcProxyGen.Create<IGroupProxy>();
      mCreateBugTypes = new Dictionary<int, Type>();

#if DEBUG

      RequestResponseTracer tracer = new RequestResponseTracer();
      tracer.Attach(mBugProxy);
      tracer.Attach(mUserProxy);
      tracer.Attach(mBugzillaProxy);
      tracer.Attach(mProductProxy);
      tracer.Attach(mGroupProxy);

#endif

      mBugProxy.Url = mURL;
      mUserProxy.Url = mURL;
      mBugzillaProxy.Url = mURL;
      mProductProxy.Url = mURL;
      mGroupProxy.Url = mURL;

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
    /// <returns>A sequence of product details for each requested product.</returns>
    public IEnumerable<Product> GetProducts(IEnumerable<int> productIds)
    {
      GetProductsParams prodParams = new GetProductsParams();
      prodParams.Ids = productIds.ToArray();

      GetProductsResponse response = mProductProxy.GetProducts(prodParams);
      return response.Products.Select((dets) => new Product(dets)).ToList();
    }

    /// <summary>
    /// Get the details of all the fields which can be set on a bug.
    /// </summary>
    /// <returns>Details of those fields which can be set on a bug.</returns>
    public IEnumerable<BugFieldDetails> GetBugFields()
    {
      //Only interested in the name, whether the field is custom or not and the type.
      GetFieldsParam getFieldsParam = new GetFieldsParam();

      try
      {
        GetFieldsResponse getFieldsResp = mBugProxy.GetValidFields(getFieldsParam);

        List<BugFieldDetails> fields = new List<BugFieldDetails>();

        foreach (Proxies.Bug.Responses.BugField field in getFieldsResp.Fields)
          fields.Add(new BugFieldDetails(field));

        return fields;
      }
      catch (XmlRpcFaultException)
      {
        throw new BugzillaException("Error attempting to get fields for bug.");
      }
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
    public int CreateUser(string email, string password, string fullName)
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
    /// <returns>A sequence of user details which matches any of the specified search parameters.</returns>
    /// <remarks>At least one of <paramref name="ids"/>, <paramref name="loginNames"/> or <paramref name="userMatches"/>
    /// must be non-null and have at least one value.</remarks>
    /// <exception cref="ArgumentException"><paramref name="ids"/>, <paramref name="loginNames"/> 
    /// or <paramref name="userMatches"/> are all null or have no values.</exception>
    /// <exception cref="InvalidLoginOrGroupNameException">One or more user IDs or group names specified is invalid.</exception>
    /// <exception cref="UserAccessDeniedException">One or more of the requested users are not accessible to the currently logged in user.</exception>
    /// <exception cref="InvalidOperationException">Logged out users cannot use the <paramref name="userMatches"/> functionality.</exception>
    public IEnumerable<User> SearchUsers(IEnumerable<int> groupIds, 
                                         IEnumerable<string> groupNames, 
                                         IEnumerable<int> ids, 
                                         IEnumerable<string> loginNames, 
                                         IEnumerable<string> userMatches, 
                                         bool includeDisabled)
    {
      //Bugzilla requires that at least one of these be set.
      if ((ids == null 
           || ids.Count() == 0)
          && (loginNames == null 
             || loginNames.Count() == 0)
          && (userMatches == null 
             || userMatches.Count() == 0))
        throw new ArgumentException("At least one user ID, login name or user match must be specified.");

      GetUserParams searchParams = new GetUserParams();
      searchParams.GroupIds = ConvertIEnumerableToArray(groupIds);
      searchParams.Groups = ConvertIEnumerableToArray(groupNames);
      searchParams.Ids = ConvertIEnumerableToArray(ids);
      searchParams.IncludeDisabled = includeDisabled;
      searchParams.Names = ConvertIEnumerableToArray(loginNames);
      searchParams.UserMatches = ConvertIEnumerableToArray(userMatches);

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
    /// Searches for bugs which matches a set of search criteria.
    /// </summary>
    /// <param name="aliases">Aliases to match on.</param>
    /// <param name="assignedTo">Username of the person assigned to the bugs.</param>
    /// <param name="components">Components to search on.</param>
    /// <param name="reporters">Login names of the people who reported the bugs.</param>
    /// <param name="ids">Bug IDs to search on.</param>
    /// <param name="operatingSystems">Operating systems to search on.</param>
    /// <param name="platforms">Platforms to search on.</param>
    /// <param name="priorities">Bug priorities to search on.</param>
    /// <param name="products">Products to search on.</param>
    /// <param name="resolutions">Resolutions to search on.</param>
    /// <param name="severities">Severities to search on.</param>
    /// <param name="statuses">Statuses to search on.</param>
    /// <param name="summaries">Summary text to search for - full matches only, not partial matching.</param>
    /// <param name="targetMilestones">Target milestones to search on.</param>
    /// <param name="qaContacts">QA contact usernames to search on.</param>
    /// <param name="urls">See also URLs to search on.</param>
    /// <param name="versions">Versions to search on.</param>
    /// <param name="statusWhiteboard">Status whiteboard text to match on.</param>
    /// <param name="creationTime">Search for bugs created after this date.</param>
    /// <param name="lastChangeTime">Search for bugs modified after this date.</param>
    /// <param name="limit">Restricts the search results to this number of results. </param>
    /// <param name="offset">Starting position .</param>
    /// <returns>
    /// <para>A set of bugs which matches <b>any</b> of the specified criteria.</para>
    /// <para>For the <paramref name="summaries"/> and <paramref name="statusWhiteboard"/> parameters, strings are not
    /// split on spaces.</para>
    /// </returns>
    /// <remarks>Only bugs visible to the currently logged in user (if any) will be included in the search results.</remarks>
    public IEnumerable<Bug> SearchBugs(IEnumerable<string> aliases,
                                       IEnumerable<string> assignedTo,
                                       IEnumerable<string> components,
                                       IEnumerable<string> reporters,
                                       IEnumerable<int> ids,
                                       IEnumerable<string> operatingSystems,
                                       IEnumerable<string> platforms,
                                       IEnumerable<string> priorities,
                                       IEnumerable<string> products,
                                       IEnumerable<string> resolutions,
                                       IEnumerable<string> severities,
                                       IEnumerable<string> statuses,
                                       IEnumerable<string> summaries,
                                       IEnumerable<string> targetMilestones,
                                       IEnumerable<string> qaContacts,
                                       IEnumerable<string> urls,
                                       IEnumerable<string> versions,
                                       IEnumerable<string> statusWhiteboard,
                                       DateTime? creationTime,
                                       DateTime? lastChangeTime,
                                       int limit,
                                       int offset)
    {
      try
      {
        BugSearchParam searchParams = new BugSearchParam();
        searchParams.Alias = ConvertIEnumerableToArray(aliases);
        searchParams.AssignedTo = ConvertIEnumerableToArray(assignedTo);
        searchParams.Component = ConvertIEnumerableToArray(components);
        searchParams.Creator = ConvertIEnumerableToArray(reporters);
        searchParams.Id = ConvertIEnumerableToArray(ids);
        searchParams.OperatingSystem = ConvertIEnumerableToArray(operatingSystems);
        searchParams.Platform = ConvertIEnumerableToArray(platforms);
        searchParams.Priority = ConvertIEnumerableToArray(priorities);
        searchParams.Product = ConvertIEnumerableToArray(products);
        searchParams.Resolution = ConvertIEnumerableToArray(resolutions);
        searchParams.Severity = ConvertIEnumerableToArray(severities);
        searchParams.Status = ConvertIEnumerableToArray(statuses);
        searchParams.Summary = ConvertIEnumerableToArray(summaries);
        searchParams.TargetMilestone = ConvertIEnumerableToArray(targetMilestones);
        searchParams.QAContact = ConvertIEnumerableToArray(qaContacts);
        searchParams.Url = ConvertIEnumerableToArray(urls);
        searchParams.Version = ConvertIEnumerableToArray(versions);
        searchParams.Whiteboard = ConvertIEnumerableToArray(statusWhiteboard);

        if (creationTime.HasValue)
          searchParams.CreationTime = creationTime.Value;

        if (lastChangeTime.HasValue)
          searchParams.LastChangeTime = lastChangeTime.Value;

        searchParams.Limit = limit;
        searchParams.Offset = offset;

        BugSearchResponse resp = mBugProxy.Search(searchParams);
        return resp.Bugs.Select(info => new Bug(info, mBugProxy, GetCustomFieldValuesForBug(info)));
      }
      catch (XmlRpcFaultException e)
      {
        throw new BugzillaException(string.Format("Error searching for bugs. Details: {0}", e.Message));
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
    /// <returns>A bug instance for the specified ID.</returns>
    /// <exception cref="InvalidOperationException">Attempted to fetch details of the bug from the remote server when a user isn't logged in.</exception>
    /// <exception cref="InvalidBugIDOrAliasException">No bug exists with the specified ID.</exception>
    /// <exception cref="BugAccessDeniedException">Requested bug is inaccessible to the current user.</exception>
    public IEnumerable<Bug> GetBugs(IEnumerable<int> ids)
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
      createParams.CCList = ConvertIEnumerableToArray(ccList);
      createParams.Groups = ConvertIEnumerableToArray(groups);
      createParams.DependsOn = ConvertIEnumerableToArray(dependsOnBugIDs);
      createParams.Blocks = ConvertIEnumerableToArray(blockedBugIDs);

      if(deadline != null)
        createParams.Deadline = deadline.Value.ToString("yyyy-MM-dd");

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
    public IEnumerable<Attachment> AddAttachmentToBugs(IEnumerable<string> idsOrAliases,
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
    /// <exception cref="ArgumentNullException"><paramref name="idsOrAliases"/> is null.</exception>
    /// <exception cref="AttachmentTooLargeException">Size of the attachment is too large.</exception>
    /// <exception cref="InvalidMIMETypeException"><paramref name="mimeType"/> is invalid.</exception>
    public IEnumerable<Attachment> AddAttachmentToBugs(IEnumerable<string> idsOrAliases,
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

      if (idsOrAliases == null)
        throw new ArgumentNullException("idsOrAliases");

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
    /// Gets comments based on specific comment IDs.
    /// </summary>
    /// <param name="commentIDs">IDs of specific comments to get.</param>
    /// <returns>Comment details for the requested comments.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="commentIDs"/> is null.</exception>
    /// <exception cref="CommentAccessDeniedException">One or more of the requested comments are inaccessible to the current user.</exception>
    /// <exception cref="InvalidCommentIDException">One or more invalid comment IDs specified.</exception>
    public IEnumerable<Comment> GetCommentsByID(IEnumerable<int> commentIDs)
    {
      if (commentIDs == null)
        throw new ArgumentNullException("commentIDs");

      //Fill in the request params
      GetCommentParams commentParams = new GetCommentParams();
      commentParams.CommentIDs = ConvertIEnumerableToArray(commentIDs);

      //Run the command on the server
      try
      {
        GetCommentsResponse resp = mBugProxy.GetComments(commentParams);
        List<Comment> comments = new List<Comment>();
        
        //Parse the comments collection
        foreach (var commentID in resp.Comments.Keys)
        {
          Comment comm = new Comment((XmlRpcStruct)resp.Comments[commentID]);
          comments.Add(comm);
        }

        return comments;
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
            throw new BugzillaException(string.Format("Error getting comments. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Gets attachment based on the IDs of those attachments.
    /// </summary>
    /// <param name="attachmentIDs">IDs of particular attachment to fetch.</param>
    /// <returns>Details for the requested attachments.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="attachmentIDs"/> is null.</exception>
    /// <exception cref="InvalidBugIDOrAliasException">One or more invalid bug IDs/aliases specified.</exception>
    /// <exception cref="BugAccessDeniedException">Current user does not have access one or more of the specified bugs.</exception>
    /// <exception cref="AttachmentAccessDeniedException">One or more of the specified attachments are private but the current user is not in the "insiders" group.</exception>
    public IEnumerable<Attachment> GetAttachmentsByID(IEnumerable<int> attachmentIDs)
    {
      if (attachmentIDs == null)
        throw new ArgumentNullException("attachmentIDs");

      //Fill in the request parameters
      GetAttachmentsParam attachentParams = new GetAttachmentsParam();
      attachentParams.AttachmentIDs = ConvertIEnumerableToArray(attachmentIDs);

      try
      {
        GetAttachmentsResponse resp = mBugProxy.GetAttachments(attachentParams);
        List<Attachment> attachments = new List<Attachment>();

        //Parse the attachments collection
        foreach (var attachmentID in resp.Attachments.Keys)
        {
          Attachment attachment = new Attachment((XmlRpcStruct)resp.Attachments[attachmentID]);
          attachments.Add(attachment);
        }

        return attachments;
      }
      catch (XmlRpcFaultException e)
      {
        switch (e.FaultCode)
        {
          case 304:
            throw new AttachmentAccessDeniedException();

          default:
            throw new BugzillaException(string.Format("Error getting attachments. Details: {0}", e.Message));
        }
      }
    }

    /// <summary>
    /// Creates a new group.
    /// </summary>
    /// <param name="shortName">Short name of the group - must be unique.</param>
    /// <param name="description">Description of the group.</param>
    /// <param name="userRegExp">A regular expression. Any user whose Bugzilla username matches this regular expression will automatically be granted membership in this group.</param>
    /// <param name="isActive"><code>true</code> if this group can be used for bugs, <code>false</code> if this is a group that will only contain users and no bugs will be restricted to it.</param>
    /// <param name="iconURL">A URL pointing to a small icon used to identify the group.</param>
    /// <exception cref="DuplicateGroupNameException">Attemped to create a group with a duplicate name.</exception>
    /// <exception cref="InvalidGroupRegExpException">Invalid user regular expression specified.</exception>
    /// <exception cref="BugzillaException">Unknown server error when creating the new group.</exception>
    /// <returns>The ID of the new group.</returns>
    public int CreateGroup(string shortName, string description, string userRegExp, bool isActive, string iconURL)
    {
      if (string.IsNullOrEmpty(shortName))
        throw new ArgumentNullException("shortName");

      if (string.IsNullOrEmpty(description))
        throw new ArgumentNullException("description");

      CreateGroupParams createParams = new CreateGroupParams();
      createParams.ShortName = shortName;
      createParams.Description = description;
      createParams.UserRegularExpression = userRegExp;
      createParams.IsActive = isActive;
      createParams.IconURL = iconURL;

      try
      {
        CreateGroupResponse resp = mGroupProxy.CreateGroup(createParams);
        return resp.GroupID;
      }
      catch(XmlRpcFaultException e)
      {
        switch(e.FaultCode)
        {
          case 801:
            throw new DuplicateGroupNameException(e.FaultString);

          case 803:
            throw new InvalidGroupRegExpException(e.FaultString);

          default:
            throw new BugzillaException(string.Format("Error creating group. Details: {0}", e.FaultString));
        }
      }
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="name">Name of the product.</param>
    /// <param name="description">Description of the product.</param>
    /// <param name="defaultVersion">Default version for the product.</param>
    /// <param name="hasUnconfirmedStatus">Whether the product has the UNCONFIRMED status or not.</param>
    /// <param name="classification">The name of the Classification which contains this product.</param>
    /// <param name="defaultMilestone">The default milestone for this product.</param>
    /// <param name="openForBugEntry">Whether the product allows new bugs to be recorded against it or not.</param>
    /// <param name="createChartSeries">Whether to create new chart series for the product.</param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/>, <paramref name="description"/> or <paramref name="defaultVersion"/> is null or blank.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> exceeds the max length.</exception>
    /// <exception cref="InvalidClassificationException"><paramref name="classification"/> does not exist.></exception>
    /// <exception cref="DuplicateProductNameException">A product already exists with <paramref name="name"/></exception>
    /// <exception cref="BugzillaException">Unknown server error creating the new product.</exception>
    /// <returns>ID of the newly created product.</returns>
    public int CreateProduct(string name, 
                             string description, 
                             string defaultVersion, 
                             bool hasUnconfirmedStatus, 
                             string classification,
                             string defaultMilestone, 
                             bool openForBugEntry, 
                             bool createChartSeries)
    {
      //Name is required
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException("name");
      else if(name.Length > ProductNameMaxLength)
        throw new ArgumentException(string.Format("Product name cannot exceed {0} characters.", ProductNameMaxLength));

      //Description is required
      if (string.IsNullOrEmpty(description))
        throw new ArgumentNullException("description");

      //Default version is required
      if (string.IsNullOrEmpty(defaultVersion))
        throw new ArgumentNullException("defaultVersion");

      CreateProductParams createParams = new CreateProductParams();
      createParams.Classification = classification;
      createParams.CreateChartSeries = createChartSeries;
      createParams.DefaultMilestone = defaultMilestone;
      createParams.DefaultVersion = defaultVersion;
      createParams.Description = description;
      createParams.HasUnconfirmedStatus = hasUnconfirmedStatus;
      createParams.IsOpenForBugEntry = openForBugEntry;
      createParams.Name = name;
      
      try
      {
        CreateProductResponse resp = mProductProxy.CreateProduct(createParams);
        return resp.ID;
      }
      catch(XmlRpcFaultException e)
      {
        switch(e.FaultCode)
        {
          case 51:
            throw new InvalidClassificationException(e.FaultString);

          case 702:
            throw new DuplicateProductNameException(e.FaultString);

          default:
            throw new BugzillaException(string.Format("Error creating product. Details: {0}", e.FaultString));
        }
      }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Helper method which converts a generic IEnumerable into an array of equivalent type or <code>null</code>/>
    /// </summary>
    /// <typeparam name="T">Type of the returned array.</typeparam>
    /// <param name="val">Sequence to convert to an array.</param>
    /// <returns><code>null</code>, if <paramref name="val"/> is <code>null</code>, otherwise an array containing <paramref name="val"/>'s elements.</returns>
    private T[] ConvertIEnumerableToArray<T>(IEnumerable<T> val)
    {
      if (val == null)
        return null;

      return val.ToArray();
    }

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
        mGroupProxy.CookieContainer.Add(cookie);
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
    /// Accessor for the last modification date/time from the audit log.
    /// </summary>
    public DateTime LastAuditTime
    {
      get
      {
        return mBugzillaProxy.GetLastAuditTime().LastAuditTime;
      }
    }
    /// <summary>
    /// Accessor for the set of installed extensions.
    /// </summary>
    public IEnumerable<Extension> InstalledExtensions
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
    public IEnumerable<Product> SelectableProducts
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
    public IEnumerable<Product> EnterableProducts
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
    public IEnumerable<Product> AccessibleProducts
    {
      get
      {
        ProductIDsResponse selectableProdIDs = mProductProxy.GetAccessibleProducts();
        return GetProducts(selectableProdIDs.Ids);
      }
    }

    /// <summary>
    /// Accessor for the configuration parameters.
    /// </summary>
    public IEnumerable<Parameter> Parameters
    {
      get 
      {
        XmlRpcStruct resp = mBugzillaProxy.GetParameters();
        XmlRpcStruct paramResp = (XmlRpcStruct)resp["parameters"];
        List<Parameter> parameters = new List<Parameter>();

        foreach (string paramName in paramResp.Keys)
          parameters.Add(new Parameter(paramName, paramResp[paramName]));

        return parameters;
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