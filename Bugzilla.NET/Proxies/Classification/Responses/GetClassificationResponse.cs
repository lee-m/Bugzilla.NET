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

namespace Bugzilla.Proxies.Classification.Responses
{
  /// <summary>
  /// Response value from querying for classifications.
  /// </summary>
  internal struct GetClassificationResponse
  {
    /// <summary>
    /// Details of each queried classification.
    /// </summary>
    [XmlRpcMember("classifications")]
    public ClassificationDets[] Classifications;
  }

  /// <summary>
  /// Returned details of a single classification.
  /// </summary>
  internal struct ClassificationDets
  {
    /// <summary>
    /// ID of the classification.
    /// </summary>
    [XmlRpcMember("id")]
    public int ID;

    /// <summary>
    /// Name of the classification
    /// </summary>
    [XmlRpcMember("name")]
    public string Name;

    /// <summary>
    /// Description of the classification
    /// </summary>
    [XmlRpcMember("description")]
    public string Description;

    /// <summary>
    /// Sort order of the classification.
    /// </summary>
    [XmlRpcMember("sort_key")]
    public int SortKey;

    /// <summary>
    /// Details of each product associated with this classification.
    /// </summary>
    [XmlRpcMember("products")]
    public ClassificationProductDets[] AssociatedProducts;
  }

  /// <summary>
  /// Details of a product linked to a classification
  /// </summary>
  internal struct ClassificationProductDets
  {
    /// <summary>
    /// ID of the product.
    /// </summary>
    [XmlRpcMember("id")]
    public int ID;

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
  }
}
