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

using Bugzilla.Proxies.Product.Params;
using Bugzilla.Proxies.Product.Responses;

namespace Bugzilla.Proxies.Product
{
  /// <summary>
  /// Product level proxy for interacting with products.
  /// </summary>
  [XmlRpcProxyAssemblyName("IProductProxy")]
  internal interface IProductProxy : IXmlRpcProxy
  {
    /// <summary>
    /// Gets details of the specified products.
    /// </summary>
    /// <param name="prodIDs">Product IDs to get the details for.</param>
    /// <returns>Array of product information for each requested product.</returns>
    [XmlRpcMethod("Product.get")]
    GetProductsResponse GetProducts(GetProductsParams prodIDs);

    /// <summary>
    /// Returns a list of the ids of the products the user can search or enter bugs against.
    /// </summary>
    /// <returns>A list of product IDs.</returns>
    [XmlRpcMethod("Product.get_accessible_products")]
    ProductIDsResponse GetAccessibleProducts();

    /// <summary>
    /// Returns a list of the ids of the products the user can enter bugs against.
    /// </summary>
    /// <returns>A list of product IDs.</returns>
    [XmlRpcMethod("Product.get_enterable_products")]
    ProductIDsResponse GetEnterableProducts();

    /// <summary>
    /// Returns a list of the ids of the products the user can search on.
    /// </summary>
    /// <returns>A list of product IDs.</returns>
    [XmlRpcMethod("Product.get_selectable_products")]
    ProductIDsResponse GetSelectableProducts();

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="createParams">Details of the new product to create.</param>
    /// <returns>ID of the newly created product.</returns>
    [XmlRpcMethod("Product.create")]
    CreateProductResponse CreateProduct(CreateProductParams createParams);
  }
}
