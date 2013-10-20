//Copyright (C) 2013 by Lee Millward

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

namespace Bugzilla.Proxies.Product.Params
{
  /// <summary>
  /// Parameters used to create a new product.
  /// </summary>
  internal struct CreateProductParams
  {
    /// <summary>
    /// Name of the product.
    /// </summary>
    [XmlRpcMember("name")]
    [XmlRpcMissingMapping(MappingAction.Error)]
    public string Name;

    /// <summary>
    /// Description of the product.
    /// </summary>
    [XmlRpcMember("description")]
    [XmlRpcMissingMapping(MappingAction.Error)]
    public string Description;

     /// <summary>
    /// Default version of the product
    /// </summary>
    [XmlRpcMember("version")]
    [XmlRpcMissingMapping(MappingAction.Error)]
    public string DefaultVersion;

    /// <summary>
    /// Indicates whether the UNCONFIRMED bug status is available for this product.
    /// </summary>
    [XmlRpcMember("has_unconfirmed")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public bool HasUnconfirmedStatus;

    /// <summary>
    /// The name of the classification this product belongs to.
    /// </summary>
    [XmlRpcMember("classification")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string Classification;

    /// <summary>
    /// The name of the default milestone for the product.
    /// </summary>
    [XmlRpcMember("default_milestone")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public string DefaultMilestone;

    /// <summary>
    /// Whether the product is open for bug entry or not.
    /// </summary>
    [XmlRpcMember("is_open")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public bool IsOpenForBugEntry;

    /// <summary>
    /// Whether new chart series should be created for this product.
    /// </summary>
    [XmlRpcMember("create_series")]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public bool CreateChartSeries;
  }
}
