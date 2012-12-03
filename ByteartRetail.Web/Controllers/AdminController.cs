using ByteartRetail.DataObjects;
using ByteartRetail.Infrastructure.Communication;
using ByteartRetail.ServiceContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ByteartRetail.Web.Controllers
{
    [HandleError]
    public class AdminController : ControllerBase
    {
        #region Common Utility Actions
        [NonAction]
        private void SaveFile(HttpPostedFileBase postedFile, string filePath, string saveName)
        {
            string phyPath = Request.MapPath("~" + filePath);
            if (!Directory.Exists(phyPath))
            {
                Directory.CreateDirectory(phyPath);
            }
            try
            {
                postedFile.SaveAs(phyPath + saveName);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);

            }
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase fileData, string folder)
        {
            var result = string.Empty;
            if (fileData != null)
            {
                string ext = Path.GetExtension(fileData.FileName);
                result = Guid.NewGuid().ToString().Replace('-', '_') + ext;
                SaveFile(fileData, Url.Content("~/Images/Products/"), result);
            }
            return Content(result);
        }
        #endregion

        #region Administration
        [Authorize]
        public ActionResult Administration()
        {
            ViewBag.Message = "Please select the administration task below.";
            return View();
        }
        #endregion

        #region Categories
        [Authorize]
        public ActionResult Categories()
        {
            using (ServiceProxy<IProductService> proxy = new ServiceProxy<IProductService>())
            {
                var categories = proxy.Channel.GetCategories(QuerySpec.Empty);
                return View(categories);
            }
        }

        [Authorize]
        public ActionResult EditCategory(string id)
        {
            using (ServiceProxy<IProductService> proxy = new ServiceProxy<IProductService>())
            {
                var model = proxy.Channel.GetCategoryByID(new Guid(id), QuerySpec.Empty);
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditCategory(CategoryDataObject category)
        {
            using (ServiceProxy<IProductService> proxy = new ServiceProxy<IProductService>())
            {
                var categoryList = new CategoryDataObjectList { category };
                proxy.Channel.UpdateCategories(categoryList);
                return RedirectToSuccess("更新商品分类成功！", "Categories", "Admin");
            }
        }

        [Authorize]
        public ActionResult DeleteCategory(string id)
        {
            using (ServiceProxy<IProductService> proxy = new ServiceProxy<IProductService>())
            {
                proxy.Channel.DeleteCategories(new IDList { id });
                return RedirectToSuccess("删除商品分类成功！", "Categories", "Admin");
            }
        }

        [Authorize]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddCategory(CategoryDataObject category)
        {
            using (ServiceProxy<IProductService> proxy = new ServiceProxy<IProductService>())
            {
                proxy.Channel.CreateCategories(new CategoryDataObjectList { category });
                return RedirectToSuccess("添加商品分类成功！", "Categories", "Admin");
            }
        }
        #endregion

        #region Products
        [Authorize]
        public ActionResult Products()
        {
            using (ServiceProxy<IProductService> proxy = new ServiceProxy<IProductService>())
            {
                var model = proxy.Channel.GetProducts(QuerySpec.VerboseOnly);
                return View(model);
            }
        }

        [Authorize]
        public ActionResult EditProduct(string id)
        {
            using (ServiceProxy<IProductService> proxy = new ServiceProxy<IProductService>())
            {
                var model = proxy.Channel.GetProductByID(new Guid(id), QuerySpec.VerboseOnly);
                var categories = proxy.Channel.GetCategories(QuerySpec.Empty);
                categories.Insert(0, new CategoryDataObject { ID = Guid.Empty.ToString(), Name = "(未分类)", Description = "(未分类)" });
                if (model.Category != null)
                    ViewData["categories"] = new SelectList(categories, "ID", "Name", model.Category.ID);
                else
                    ViewData["categories"] = new SelectList(categories, "ID", "Name", Guid.Empty.ToString());
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditProduct(ProductDataObject product)
        {
            using (ServiceProxy<IProductService> proxy = new ServiceProxy<IProductService>())
            {
                proxy.Channel.UpdateProducts(new ProductDataObjectList { product });
                if (product.Category.ID != Guid.Empty.ToString())
                    proxy.Channel.CategorizeProduct(new Guid(product.ID), new Guid(product.Category.ID));
                else
                    proxy.Channel.UncategorizeProduct(new Guid(product.ID));
                return RedirectToSuccess("更新商品信息成功！", "Products", "Admin");
            }
        }

        [Authorize]
        public ActionResult AddProduct()
        {
            using (ServiceProxy<IProductService> proxy = new ServiceProxy<IProductService>())
            {
                var categories = proxy.Channel.GetCategories(QuerySpec.Empty);
                categories.Insert(0, new CategoryDataObject { ID = Guid.Empty.ToString(), Name = "(未分类)", Description = "(未分类)" });
                ViewData["categories"] = new SelectList(categories, "ID", "Name", Guid.Empty.ToString());
                return View();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddProduct(ProductDataObject product)
        {
            using (ServiceProxy<IProductService> proxy = new ServiceProxy<IProductService>())
            {
                if (string.IsNullOrEmpty(product.ImageUrl))
                {
                    var fileName = Guid.NewGuid().ToString() + ".png";
                    System.IO.File.Copy(Server.MapPath("~/Images/Products/ProductImage.png"), Server.MapPath(string.Format("~/Images/Products/{0}", fileName)));
                    product.ImageUrl = fileName;
                }
                var addedProducts = proxy.Channel.CreateProducts(new ProductDataObjectList { product });
                if (product.Category != null &&
                    product.Category.ID != Guid.Empty.ToString())
                    proxy.Channel.CategorizeProduct(new Guid(addedProducts[0].ID), new Guid(product.Category.ID));
                return RedirectToSuccess("添加商品信息成功！", "Products", "Admin");
            }
        }

        [Authorize]
        public ActionResult DeleteProduct(string id)
        {
            using (ServiceProxy<IProductService> proxy = new ServiceProxy<IProductService>())
            {
                proxy.Channel.DeleteProducts(new IDList { id });
                return RedirectToSuccess("删除商品信息成功！", "Products", "Admin");
            }
        }
        #endregion

        #region User Accounts
        [Authorize]
        public ActionResult UserAccounts()
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var users = proxy.Channel.GetUsers(QuerySpec.VerboseOnly);
                var model = new List<UserAccountModel>();
                users.ForEach(u => model.Add(UserAccountModel.CreateFromDataObject(u)));
                return View(model);
            }
        }

        [Authorize]
        public ActionResult AddUserAccount()
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var roles = proxy.Channel.GetRoles();
                if (roles == null)
                    roles = new RoleDataObjectList();

                roles.Insert(0, new RoleDataObject { ID = Guid.Empty.ToString(), Name = "(未指定)", Description = "(未指定)" });
                
                ViewData["roles"] = new SelectList(roles, "ID", "Name", Guid.Empty.ToString());
                return View();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddUserAccount(UserAccountModel model)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var user = model.ConvertToDataObject();
                var createdUsers = proxy.Channel.CreateUsers(new UserDataObjectList { user });
                if (model.Role.ID != Guid.Empty.ToString())
                    proxy.Channel.AssignRole(new Guid(createdUsers[0].ID), new Guid(model.Role.ID));
                return RedirectToSuccess("创建用户账户成功！", "UserAccounts", "Admin");
            }
        }

        [Authorize]
        public ActionResult EditUserAccount(string id)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var user = proxy.Channel.GetUserByKey(new Guid(id), QuerySpec.VerboseOnly);
                var model = UserAccountModel.CreateFromDataObject(user);
                var roles = proxy.Channel.GetRoles();
                if (roles == null)
                    roles = new RoleDataObjectList();
                roles.Insert(0, new RoleDataObject { ID = Guid.Empty.ToString(), Name = "(未指定)", Description = "(未指定)" });
                if (model.Role != null)
                    ViewData["roles"] = new SelectList(roles, "ID", "Name", model.Role.ID);
                else
                    ViewData["roles"] = new SelectList(roles, "ID", "Name", Guid.Empty.ToString());
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditUserAccount(UserAccountModel model)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var user = model.ConvertToDataObject();
                proxy.Channel.UpdateUsers(new UserDataObjectList { user });
                if (model.Role.ID != Guid.Empty.ToString())
                    proxy.Channel.AssignRole(new Guid(model.ID), new Guid(model.Role.ID));
                else
                    proxy.Channel.UnassignRole(new Guid(model.ID));
                return RedirectToSuccess("更新用户账户成功！", "UserAccounts", "Admin");
            }
        }

        [Authorize]
        public ActionResult DisableUserAccount(string id)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                proxy.Channel.DisableUser(new UserDataObject { ID = id });
                return RedirectToAction("UserAccounts");
            }
        }

        [Authorize]
        public ActionResult EnableUserAccount(string id)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                proxy.Channel.EnableUser(new UserDataObject { ID = id });
                return RedirectToAction("UserAccounts");
            }
        }

        [Authorize]
        public ActionResult DeleteUserAccount(string id)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                proxy.Channel.DeleteUsers(new UserDataObjectList { new UserDataObject { ID = id } });
                return RedirectToSuccess("删除用户账户成功！", "UserAccounts", "Admin");
            }
        }
        #endregion

        #region Roles
        [Authorize]
        public ActionResult Roles()
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var model = proxy.Channel.GetRoles();
                return View(model);
            }
        }

        [Authorize]
        public ActionResult EditRole(string id)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var model = proxy.Channel.GetRoleByKey(new Guid(id));
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditRole(RoleDataObject model)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                proxy.Channel.UpdateRoles(new RoleDataObjectList { model });
                return RedirectToSuccess("更新账户角色成功！", "Roles", "Admin");
            }
        }

        public ActionResult DeleteRole(string id)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                proxy.Channel.DeleteRoles(new IDList { id });
                return RedirectToSuccess("删除账户角色成功！", "Roles", "Admin");
            }
        }

        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRole(RoleDataObject model)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                proxy.Channel.CreateRoles(new RoleDataObjectList { model });
                return RedirectToSuccess("添加账户角色成功！", "Roles", "Admin");
            }
        }
        #endregion

        #region SalesOrders
        public ActionResult SalesOrders()
        {
            using (ServiceProxy<IOrderService> proxy = new ServiceProxy<IOrderService>())
            {
                var model = proxy.Channel.GetAllSalesOrders();
                return View(model);
            }
        }

        public ActionResult SalesOrder(string id)
        {
            using (ServiceProxy<IOrderService> proxy = new ServiceProxy<IOrderService>())
            {
                var model = proxy.Channel.GetSalesOrder(new Guid(id));
                return View(model);
            }
        }

        public ActionResult DispatchOrder(string id)
        {
            using (ServiceProxy<IOrderService> proxy = new ServiceProxy<IOrderService>())
            {
                proxy.Channel.Dispatch(new Guid(id));
                return RedirectToSuccess(string.Format("订单 {0} 已成功发货！", id.ToUpper()), "SalesOrders", "Admin");
            }
        }
        #endregion
    }
}
