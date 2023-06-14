using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StoreFront.DATA.EF.Models//.Metadata
{
    //internal class Metadata
    //{
    //}

    #region Category MetaData

    public class CategoryMetadata
    {
        //public int CategoryId { get: set: }

        [Required(ErrorMessage = "*Category is required")] 
        [StringLength(50, ErrorMessage = "*Max 50 characters")] 
        [Display(Name = "Category")]
        public string CategoryName { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Max 500 characters")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? CategoryDescription { get; set; }
    }

    #endregion

    #region Order MetaData

    public class OrderMetaData
    {
        //public int OrderId { get; set; }
        //ublic string CustomerId { get; set; } = null!;

        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]// 0:d => MM/dd/yyyy
        public DateTime? OrderDate { get; set; }

        [StringLength(50, ErrorMessage = "*Max 50 characters")]
        [Display(Name = "Ship To")]
        public string? ShipToName { get; set; }

        [StringLength(30, ErrorMessage = "*30 is the maximum characters")]
        [Display(Name = "City")]
        public string? ShipCity { get; set; }

        [StringLength(5, ErrorMessage = "*Max 5 characters")]
        [Display(Name = "Zipcode")]
        [DataType(DataType.PostalCode)]
        public string? ShipZip { get; set; }

        [StringLength(2, ErrorMessage = "*Maximum 2 characters")]
        [Display(Name = "State")]
        public string? ShipState { get; set; }

        [StringLength(50, ErrorMessage = "*Maximum 50 characters")]
        [Display(Name = "Country")]
        public string? ShipCountry { get; set; }
    }

    #endregion

    #region Product Metadata

    public class ProductMetadata
    {
        //public int ProductId { get; set; }

        [Required(ErrorMessage = "*Name is required")]
        [StringLength(200, ErrorMessage = "*Max 200 characters")]
        public string Name { get; set; } = null!;

        [StringLength(200, ErrorMessage = "*Max 200 characters")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        //public int CategoryId { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, (double)decimal.MaxValue)]//Will not allow negative or higher than max decimal values
        public decimal? Price { get; set; }

        [StringLength(50, ErrorMessage ="*Max 50 characters")]
        [Display (Name ="Quantity Per Unit")]
        public string? QuantityPerUnit { get; set; }

        [Display(Name = "Units in Stock")]
        public short? UnitsInStock { get; set; }

        [Display(Name = "Units on Order")]
        public short? UnitsOnOrder { get; set; }

        [Display(Name = "Restock Level")]
        public short? RestockLevel { get; set; }

        //public int ProductStatusId { get; set; }

        //public int SupplierId { get; set; }

        //public int RestrictionId { get; set; }

        [StringLength(75)] 
        public string? Image { get; set; }
    }

    #endregion

    #region ProductStats Metadata

    public class ProductStatusMetadata
    {
        //public int ProductStatusId { get; set; }

        [Required(ErrorMessage = "*Product Status is required")]
        [StringLength(50,ErrorMessage ="*Max 50 characters")]
        [Display(Name = "Product Status")]
        public string ProductStatus1 { get; set; } = null!;
    }

    #endregion

    #region Restriction Metadata

    public class RestrictionMetadata
    {
        //public int RestrictionId { get; set; }

        [StringLength(50,ErrorMessage ="*Max 50 characters")]
        [Display(Name = "Restriction Type")]
        public string? RestrictionType { get; set; }

        [StringLength(100, ErrorMessage = "*Max 100 characters")]
        [Display(Name = "Permit Needed")]
        public string? PermitNeeded { get; set; }
    }

    #endregion

    #region Supplier Metadata

    public class SupplierMetadata
    {
        //public int SupplierId { get; set; }

        [Required(ErrorMessage = "*Name is required")]
        [StringLength(50, ErrorMessage = "*Max 50 characters")]
        public string Name { get; set; } = null!;

        [StringLength(150, ErrorMessage = "*Max 150 characters")]
        public string? Address { get; set; }

        [StringLength(30, ErrorMessage = "*Max 30 characters")]
        public string? City { get; set; }

        [StringLength(2, ErrorMessage = "*Max 2 characters")]
        public string? State { get; set; }

        [StringLength(50, ErrorMessage = "*Maximum 50 characters")]
        public string? Country { get; set; }

        [StringLength(24, ErrorMessage = "*Max 24 characters")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string? Phone { get; set; }
    }

    #endregion

    #region UserDetail Metadata

    public class UserDetailMetadata
    {
        //public string CustomerId { get; set; } = null!;

        [Required(ErrorMessage = "*First name is required")]
        [StringLength(50, ErrorMessage = "*Max 50 characters")]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "*Last name is required")]
        [StringLength(50, ErrorMessage = "*Max 50 characters")]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;

        [StringLength(150, ErrorMessage = "*Max 150 characters")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "*Max 50 characters")]
        public string? City { get; set; }

        [StringLength(5, ErrorMessage = "*Max 5 characters")]
        [Display(Name = "Zipcode")]
        [DataType(DataType.PostalCode)]
        public string? Zip { get; set; }

        [StringLength(2, ErrorMessage = "*Max 2 characters")]
        public string? State { get; set; }

        [StringLength(50, ErrorMessage = "*Maximum 50 characters")]
        public string? Country { get; set; }

        [StringLength(24, ErrorMessage = "*Max 24 characters")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string? Phone { get; set; }
    }

    #endregion

}
