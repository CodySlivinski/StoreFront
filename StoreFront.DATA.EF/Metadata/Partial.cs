using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;




namespace StoreFront.DATA.EF.Models//.Metadata
{
    //internal class Partial
    //{
    //}
    #region Category

    [ModelMetadataType(typeof(CategoryMetadata))] //This applies the metadata to the Category mo
    public partial class Category { }

    #endregion

    #region Order

    [ModelMetadataType(typeof(OrderMetaData))] //This applies the metadata to the Category mo
    public partial class Order { }

    #endregion

    #region Product

    [ModelMetadataType(typeof(ProductMetadata))] //This applies the metadata to the Category mo
    public partial class Product 
    {
        [NotMapped]    
        public IFormFile? ProductImage { get; set; }
    }

    #endregion

    #region ProductStatus

    [ModelMetadataType(typeof(ProductStatusMetadata))] //This applies the metadata to the Category mo
    public partial class ProductStatus { }

    #endregion

    #region Restriction

    [ModelMetadataType(typeof(RestrictionMetadata))] //This applies the metadata to the Category mo
    public partial class Restriction { }

    #endregion

    #region Supplier

    [ModelMetadataType(typeof(SupplierMetadata))] //This applies the metadata to the Category mo
    public partial class Supplier { }

    #endregion

    #region UserDetail

    [ModelMetadataType(typeof(UserDetailMetadata))] //This applies the metadata to the Category mo
    public partial class UserDetail { }

    #endregion



}
