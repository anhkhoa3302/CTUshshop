using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CTUshshop.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string Email => "Email";

        public static string ChangePassword => "ChangePassword";

        public static string DownloadPersonalData => "DownloadPersonalData";

        public static string DeletePersonalData => "DeletePersonalData";

        public static string ExternalLogins => "ExternalLogins";

        public static string PersonalData => "PersonalData";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";


        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);

        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);


        public static string UserProducts => "UserProducts";
        public static string UserProductsNavClass(ViewContext viewContext) => PageNavClass(viewContext, UserProducts);

        public static string AddUserProducts => "AddUserProducts";
        public static string AddUserProductsNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddUserProducts);

        public static string EditUserProducts => "EditUserProducts";
        public static string EditUserProductsNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditUserProducts);

        public static string DetailUserProducts => "DetailUserProducts";
        public static string DetailUserProductsNavClass(ViewContext viewContext) => PageNavClass(viewContext, DetailUserProducts);

        public static string UserOrders => "UserOrders";
        public static string UserOrdersNavClass(ViewContext viewContext) => PageNavClass(viewContext, UserOrders);

        public static string UserOrdersDetails => "UserOrdersDetails";
        public static string UserOrdersDetailsNavClass(ViewContext viewContext) => PageNavClass(viewContext, UserOrdersDetails);


        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
