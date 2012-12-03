using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace ByteartRetail.DataObjects
{
    #region Metadata Annotations
    [MetadataType(typeof(CategoryDataObjectMetadata))]
    public partial class CategoryDataObject
    {
        public override string ToString()
        {
            return this.Name;
        }
    }

    public class CategoryDataObjectMetadata
    {
        [Required(ErrorMessage="请输入商品分类名称", AllowEmptyStrings=false)]
        [Display(Name="分类名称")]
        public string Name { get; set; }

        [Required(ErrorMessage="请输入商品分类说明", AllowEmptyStrings=false)]
        [Display(Name = "分类说明")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    [MetadataType(typeof(ProductDataObjectMetadata))]
    public partial class ProductDataObject
    {
        public string CategoryName
        {
            get
            {
                if (this.Category == null)
                    return "(未分类)";
                else
                    return this.Category.Name;
            }
        }
        public override string ToString()
        {
            return this.Name;
        }
    }

    public class ProductDataObjectMetadata
    {
        [Required(ErrorMessage="请输入商品名称", AllowEmptyStrings=false)]
        [Display(Name="商品名称")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请输入商品说明", AllowEmptyStrings = false)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "商品说明")]
        public string Description { get; set; }

        [Required(ErrorMessage = "请选择商品图片", AllowEmptyStrings=false)]
        [Display(Name = "商品图片")]
        public string ImageUrl { get; set; }

        [DataType(DataType.Currency, ErrorMessage="输入的数据必须是货币类型")]
        [Required(ErrorMessage = "请输入单价", AllowEmptyStrings = false)]
        [Display(Name = "单价")]
        public decimal? UnitPrice { get; set; }

        [Display(Name="是否为推荐商品？")]
        [Required(ErrorMessage="请设置该商品是否为推荐商品")]
        public bool? IsFeatured { get; set; }
    }

    

    [MetadataType(typeof(RoleDataObjectMetadata))]
    public partial class RoleDataObject
    {
        public override string ToString()
        {
            return Name;
        }
    }

    public class RoleDataObjectMetadata
    {
        [Required(ErrorMessage="请输入角色名称")]
        [Display(Name="角色名称")]
        public string Name { get; set; }

        [Display(Name="角色描述")]
        public string Description { get; set; }
    }
    #endregion

    #region Object Extenders
    /// <summary>
    /// 表示<c>QuerySpec</c>类型的扩展。
    /// </summary>
    public partial class QuerySpec
    {
        #region Public Fields
        /// <summary>
        /// 返回一个<c>QuerySpec</c>类型的值，该值表示一个空的<c>QuerySpec</c>值：仅查询
        /// 并构建聚合根的数据传输对象。
        /// </summary>
        public static readonly QuerySpec Empty = new QuerySpec
        {
            Verbose = null
        };

        /// <summary>
        /// 返回一个<c>QuerySpec</c>类型的值，该值表示需要查询并构建聚合根及其下各层的数据传输对象。
        /// </summary>
        public static readonly QuerySpec VerboseOnly = new QuerySpec
        {
            Verbose = true
        };
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return string.Format("Verbose={0}", Verbose ?? false);
        }
        #endregion
    }

    /// <summary>
    /// 表示一个提供了分页相关信息的数据传输对象。
    /// </summary>
    public partial class Pagination
    {
        #region Public Methods
        public override string ToString()
        {
            return string.Format("PageSize={0} PageNumber={1} TotalPages={2}",
                this.PageSize,
                this.PageNumber,
                this.TotalPages ?? 0);
        }
        #endregion
    }

    public partial class UserDataObject
    {
        public override string ToString()
        {
            return this.UserName;
        }
    }

    public partial class SalesOrderDataObject
    {
        public string StatusText
        {
            get
            {
                if (this.Status != null && this.Status.HasValue)
                    switch (this.Status)
                    {
                        case SalesOrderStatusDataObject.Created:
                            return "新创建";
                        case SalesOrderStatusDataObject.Delivered:
                            return "已收货";
                        case SalesOrderStatusDataObject.Dispatched:
                            return "已发货";
                        case SalesOrderStatusDataObject.Paid:
                            return "已付款";
                        case SalesOrderStatusDataObject.Picked:
                            return "已提货";
                        default:
                            return null;
                    }
                else
                    return null;
            }
        }

        public string DateCreatedText
        {
            get { return this.DateCreated == null ? "N/A" : this.DateCreated.Value.ToShortDateString(); }
        }

        public string DateDispatchedText
        {
            get { return this.DateDispatched == null ? "N/A" : this.DateDispatched.Value.ToShortDateString(); }
        }

        public string DateDeliveredText
        {
            get { return this.DateDelivered == null ? "N/A" : this.DateDelivered.Value.ToShortDateString(); }
        }

        public int TotalLines
        {
            get { return this.SalesLines == null ? 0 : this.SalesLines.Count; }
        }

        public string IDText
        {
            get { return this.ID.Substring(0, 14) + "..."; }
        }

        public string TotalAmount
        {
            get { return string.Format("{0:N2} 元", this.Subtotal); }
        }

        public bool CanConfirm
        {
            get
            {
                return this.Status != null && this.Status == SalesOrderStatusDataObject.Dispatched;
            }
        }

        public override string ToString()
        {
            return this.ID;
        }
    }
    #endregion

    #region View Models
    public partial class UserAccountModel
    {
        public string ID { get; set; }
        [Required(ErrorMessage="请输入用户名")]
        [Display(Name="用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        [Display(Name="密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "请重新输入密码以便确认")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "确认密码与输入的密码不符")]
        public string ConfirmPassword { get; set; }

        [Display(Name="电子邮件")]
        [Required(ErrorMessage="请输入电子邮件")]
        [DataType(DataType.EmailAddress, ErrorMessage="电子邮件格式不正确")]
        public string Email { get; set; }

        [Display(Name="已禁用")]
        public bool? IsDisabled { get; set; }

        [Display(Name="注册时间")]
        [DataType(DataType.Date)]
        public DateTime? DateRegistered { get; set; }

        [Display(Name = "注册时间")]
        public string DateRegisteredStr
        {
            get { return DateRegistered.HasValue ? DateRegistered.Value.ToShortDateString() : "N/A"; }
        }

        [Display(Name = "角色")]
        public RoleDataObject Role { get; set; }

        [Display(Name = "角色")]
        public string RoleStr
        {
            get
            {
                if (Role != null && !string.IsNullOrEmpty(Role.Name))
                    return Role.Name;
                return "(未指定)";
            }
        }

        [Display(Name="最后登录")]
        [DataType(DataType.Date)]
        public DateTime? DateLastLogon { get; set; }

        [Display(Name = "最后登录")]
        public string DateLastLogonStr
        {
            get { return DateLastLogon.HasValue ? DateLastLogon.Value.ToShortDateString() : "N/A"; }
        }

        [Display(Name="联系人")]
        [Required(ErrorMessage="请输入联系人")]
        public string Contact { get; set; }

        [Display(Name = "电话号码")]
        [Required(ErrorMessage = "请输入电话号码")]
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", 
            ErrorMessage="电话号码格式不正确")]
        public string PhoneNumber { get; set; }

        [Display(Name = "联系地址 - 国家")]
        [Required(ErrorMessage = "请输入国家")]
        public string ContactAddress_Country { get; set; }

        [Display(Name = "联系地址 - 省/州")]
        [Required(ErrorMessage = "请输入省/州")]
        public string ContactAddress_State { get; set; }

        [Display(Name = "联系地址 - 市")]
        [Required(ErrorMessage = "请输入市")]
        public string ContactAddress_City { get; set; }

        [Display(Name = "联系地址 - 街道")]
        [Required(ErrorMessage = "请输入街道")]
        public string ContactAddress_Street { get; set; }

        [Display(Name = "联系地址 - 邮编")]
        [Required(ErrorMessage = "请输入邮编")]
        public string ContactAddress_Zip { get; set; }

        [Display(Name = "收货地址 - 国家")]
        [Required(ErrorMessage = "请输入国家")]
        public string DeliveryAddress_Country { get; set; }

        [Display(Name = "收货地址 - 省/州")]
        [Required(ErrorMessage = "请输入省/州")]
        public string DeliveryAddress_State { get; set; }

        [Display(Name = "收货地址 - 市")]
        [Required(ErrorMessage = "请输入市")]
        public string DeliveryAddress_City { get; set; }

        [Display(Name = "收货地址 - 街道")]
        [Required(ErrorMessage = "请输入街道")]
        public string DeliveryAddress_Street { get; set; }

        [Display(Name = "收货地址 - 邮编")]
        [Required(ErrorMessage = "请输入邮编")]
        public string DeliveryAddress_Zip { get; set; }

        public override string ToString()
        {
            return UserName;
        }

        public static UserAccountModel CreateFromDataObject(UserDataObject d)
        {
            return new UserAccountModel
            {
                ID = d.ID,
                UserName = d.UserName,
                Password = d.Password,
                Email = d.Email,
                IsDisabled = d.IsDisabled,
                DateRegistered = d.DateRegistered,
                DateLastLogon = d.DateLastLogon,
                Role = d.Role,
                Contact = d.Contact,
                PhoneNumber = d.PhoneNumber,
                ContactAddress_City = d.ContactAddress.City,
                ContactAddress_Street = d.ContactAddress.Street,
                ContactAddress_State = d.ContactAddress.State,
                ContactAddress_Country = d.ContactAddress.Country,
                ContactAddress_Zip = d.ContactAddress.Zip,
                DeliveryAddress_City = d.DeliveryAddress.City,
                DeliveryAddress_Street = d.DeliveryAddress.Street,
                DeliveryAddress_State = d.DeliveryAddress.State,
                DeliveryAddress_Country = d.DeliveryAddress.Country,
                DeliveryAddress_Zip = d.DeliveryAddress.Zip,
            };
        }

        public UserDataObject ConvertToDataObject()
        {
            return new UserDataObject
            {
                ID = this.ID,
                UserName = this.UserName,
                Password = this.Password,
                IsDisabled = this.IsDisabled,
                Email = this.Email,
                DateRegistered = this.DateRegistered,
                DateLastLogon = this.DateLastLogon,
                Contact = this.Contact,
                PhoneNumber = this.PhoneNumber,
                ContactAddress = new AddressDataObject
                {
                    Country = ContactAddress_Country,
                    State = ContactAddress_State,
                    City = ContactAddress_City,
                    Street = ContactAddress_Street,
                    Zip = ContactAddress_Zip
                },
                DeliveryAddress = new AddressDataObject
                {
                    Country = DeliveryAddress_Country,
                    State = DeliveryAddress_State,
                    City = DeliveryAddress_City,
                    Street = DeliveryAddress_Street,
                    Zip = DeliveryAddress_Zip
                }
            };
        }
    }
    #endregion
}
