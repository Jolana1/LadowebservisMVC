using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbracoEshop.lib.Util;

namespace UmbracoEshop.lib.Util
{
    public class ConfigurationUtil
    {
        public const string LoginFormId = "eshop.LoginFormId";
        public const string AfterLoginFormId = "eshop.AfterLoginFormId";
        public const string AfterPasswordResetFormId = "eshop.AfterPasswordResetFormId";
        public const string EshopZoznamVyrobcovFormId = "eshop.ZoznamVyrobcovFormId";
        public const string EshopZoznamVyrobkovFormId = "eshop.ZoznamVyrobkovFormId";
        public const string EshopProductsFormId = "eshop.ProductsFormId";



        public static int GetPageId(string pageKey)
        {
            return int.Parse(ConfigurationManager.AppSettings[pageKey]);
        }

        public static string GetCfgValue(string cfgKey)
        {
            return ConfigurationManager.AppSettings[cfgKey];
        }
    }
}
//public const string Ecommerce_Quote_TransportItemCode = "DOPRAVA";
//public const string Ecommerce_Quote_PaymentItemCode = "PLATBA";
//public const string Ecommerce_Quote_DiscountItemCode = "ZĽAVA";

//public const string Ecommerce_Quote_InfMsgId = "quote-info-msg";
//public const string Ecommerce_Quote_ModalMsgId = "quote-modal-msg";

//public const string Ecommerce_Quote_InitialState = "naplnspajzu.Ecommerce.Quote.InitialState";
//public const string Ecommerce_Quote_PaidPriceState = "naplnspajzu.Ecommerce.Quote.PaidPriceState";


//public const string EcommerceAfterLoginFormId = "naplnspajzu.Ecommerce.AfterLoginFormId";
//public const string EcommerceRegistrationOkFormId = "naplnspajzu.Ecommerce.RegistrationOkFormId";
//public const string EcommerceMembersFormId = "naplnspajzu.Ecommerce.MembersFormId";
//public const string EcommerceCustomersFormId = "naplnspajzu.Ecommerce.CustomersFormId";
//public const string EcommerceCountriesFormId = "naplnspajzu.Ecommerce.CountriesFormId";
//public const string EcommerceProducersFormId = "naplnspajzu.Ecommerce.ProducersFormId";
//public const string EcommerceAvailabilitiesFormId = "naplnspajzu.Ecommerce.AvailabilitiesFormId";
//public const string EcommerceTransportTypesFormId = "naplnspajzu.Ecommerce.TransportTypesFormId";
//public const string EcommercePaymentTypesFormId = "naplnspajzu.Ecommerce.PaymentTypesFormId";
//public const string EcommerceSimpleStringsFormId = "naplnspajzu.Ecommerce.SimpleStringsFormId";
//public const string EcommercePaymentStatesFormId = "naplnspajzu.Ecommerce.PaymentStatesFormId";
//public const string EcommerceQuoteStatesFormId = "naplnspajzu.Ecommerce.QuoteStatesFormId";
//public const string EcommerceCategoriesFormId = "naplnspajzu.Ecommerce.CategoriesFormId";
//public const string EcommerceProductAttributesFormId = "naplnspajzu.Ecommerce.ProductAttributesFormId";
//public const string EcommerceProductsFormId = "naplnspajzu.Ecommerce.ProductsFormId";
//public const string EcommerceProductPricesFormId = "naplnspajzu.Ecommerce.ProductPricesFormId";
//public const string EcommerceQuotesFormId = "naplnspajzu.Ecommerce.QuotesFormId";
//public const string EcommerceQuotesEditFormId = "naplnspajzu.Ecommerce.QuotesEditFormId";

//public const string Ecommerce_ProductPublic_DetailPageId = "naplnspajzu.Ecommerce.ProductPublic_DetailPageId";
//public const string Ecommerce_ProductPublic_CategoryPageId = "naplnspajzu.Ecommerce.ProductPublic_CategoryPageId";

//public const string Ecommerce_Basket_DeliveryDataPageId = "naplnspajzu.Ecommerce.Basket_DeliveryDataPageId";
//public const string Ecommerce_Basket_ReviewAndSendPageId = "naplnspajzu.Ecommerce.Basket_ReviewAndSendPageId";
//public const string Ecommerce_Basket_FinishedPageId = "naplnspajzu.Ecommerce.Basket_FinishedPageId";

//public const string PropId_CustomerFilterModel = "PropId_CustomerFilterModel";
//public const string PropId_ProducerFilterModel = "PropId_ProducerFilterModel";
//public const string PropId_ProductFilterModel = "PropId_ProductFilterModel";
//public const string PropId_ProductInCategoryFilterModel = "PropId_ProductInCategoryFilterModel";
//public const string PropId_ProductNotInCategoryFilterModel = "PropId_ProductNotInCategoryFilterModel";
//public const string PropId_CategoryPublicFilterModel_PageSize = "PropId_CategoryPublicFilterModel_PageSize";
//public const string PropId_CategoryPublicFilterModel_ProductView = "PropId_CategoryPublicFilterModel_ProductView";
//public const string PropId_CategoryPublicFilterModel_ProductSort = "PropId_CategoryPublicFilterModel_ProductSort";
//public const string PropId_CategoryPublicFilterModel_CurrentCategory = "PropId_CategoryPublicFilterModel_CurrentCategory";
//public const string PropId_CategoryPublicFilterModel_Producer = "PropId_CategoryPublicFilterModel_Producer";
//public const string PropId_CategoryPublicFilterModel_ProductAttribute = "PropId_CategoryPublicFilterModel_ProductAttribute";
//public const string PropId_ProductAttributeFilterModel = "PropId_ProductAttributeFilterModel";
//public const string PropId_QuoteListFilterModel = "PropId_QuoteListFilterModel";

//public static string InitialQuoteState()
//{
//    return ConfigurationManager.AppSettings[ConfigurationUtil.Ecommerce_Quote_InitialState];
//}
//public static string PaiedQuotePriceState()
//{
//    return ConfigurationManager.AppSettings[ConfigurationUtil.Ecommerce_Quote_PaidPriceState];
//}

//public static int GetPageId(string pageKey)
//{
//    return int.Parse(ConfigurationManager.AppSettings[pageKey]);
//}

//public static string EshopRootUrl
//{
//    get
//    {
//        return "/eshop/kategoria/-vsetko-";
//    }
//}
//public static string QuoteViewUrl
//{
//    get
//    {
//        return "/e-shop/detail-objednavky";
//    }
//}
//    }
//}
