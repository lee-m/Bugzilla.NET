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

namespace Bugzilla.Proxies.Product.Responses
{
  /// <summary>
  /// Return value from Product.get
  /// </summary>
  internal struct GetProductsResponse
  {
    /// <summary>
    /// Collection of product information.
    /// </summary>
    [XmlRpcMember("products")]
    public ProductDets[] Products;
  }

  /// <summary>
  /// Details of a single product.
  /// </summary>
  internal struct ProductDets
  {
    /// <summary>
    /// ID of the product.
    /// </summary>
    [XmlRpcMember("id")]
    public int Id;

    /// <summary>
    /// Name of the product.
    /// </summary>
    [XmlRpcMember("name")]
    public string Name;

    /// <summary>
    /// Description of the product.
    /// </summary>
    [XmlRpcMember("description")]
    public string Description;

    /// <summary>
    /// Indicates if the product is active or not.
    /// </summary>
    [XmlRpcMember("is_active")]
    public bool IsActive;

    /// <summary>
    /// The name of the default milestone for the product.
    /// </summary>
    [XmlRpcMember("default_milestone")]
    public string DefaultMilestone;

    /// <summary>
    /// Indicates whether the UNCONFIRMED bug status is available for this product.
    /// </summary>
    [XmlRpcMember("has_unconfirmed")]
    public bool HasUnconfirmedStatus;

    /// <summary>
    /// The name of the classification this product belongs to.
    /// </summary>
    [XmlRpcMember("classification")]
    public string Classification;

    /// <summary>
    /// The components available for this product.
    /// </summary>
    [XmlRpcMember("components")]
    public ComponentDets[] Components;

    /// <summary>
    /// The versions available for this product.
    /// </summary>
    [XmlRpcMember("versions")]
    public VersionDets[] Versions;

    /// <summary>
    /// The milestones available for this product.
    /// </summary>
    [XmlRpcMember("milestones")]
    public MilestoneDets[] Milestones;
  }

  /// <summary>
  /// Details sbout a single component that is available for a product.
  /// </summary>
  internal struct ComponentDets
  {
    /// <summary>
    /// ID of the component.
    /// </summary>
    [XmlRpcMember("id")]
    public int ID;

    /// <summary>
    /// Name of the component.
    /// </summary>
    [XmlRpcMember("name")]
    public string Name;

    /// <summary>
    /// Description of the component.
    /// </summary>
    [XmlRpcMember("description")]
    public string Description;

    /// <summary>
    /// The login name of the user to whom new bugs will be assigned by default.
    /// </summary>
    [XmlRpcMember("default_assigned_to")]
    public string DefaultAsignee;

    /// <summary>
    /// The login name of the user who will be set as the QA Contact for new bugs by default.
    /// </summary>
    [XmlRpcMember("default_qa_contact")]
    public string DefaultQAContact;

    /// <summary>
    /// Sort order for this component.
    /// </summary>
    [XmlRpcMember("sort_key")]
    public int SortKey;

    /// <summary>
    /// Whether the component is active or not.
    /// </summary>
    [XmlRpcMember("is_active")]
    public bool IsActive;
  }

  /// <summary>
  /// Details about a version available for a product.
  /// </summary>
  internal struct VersionDets
  {
    /// <summary>
    /// Name of the version.
    /// </summary>
    [XmlRpcMember("name")]
    public string Name;

    /// <summary>
    /// Sort order for the version.
    /// </summary>
    [XmlRpcMember("sort_key")]
    public int SortKey;

    /// <summary>
    /// Whether the version is active or not.
    /// </summary>
    [XmlRpcMember("is_active")]
    public bool IsActive;
  }

  /// <summary>
  /// Details about a single milestone available for a product.
  /// </summary>
  internal struct MilestoneDets
  {
    /// <summary>
    /// Name of the milestone.
    /// </summary>
    [XmlRpcMember("name")]
    public string Name;

    /// <summary>
    /// Sort order for the milestone.
    /// </summary>
    [XmlRpcMember("sort_key")]
    public int SortKey;

    /// <summary>
    /// Whether the milestone is active or not.
    /// </summary>
    [XmlRpcMember("is_active")]
    public bool IsActive;
  }
}
