using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Request.Roles;
using Pharma263.Integration.Api.Models.Response;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Integration.Api
{
    public interface IPharmaApiService
    {
        #region Account

        [Post("/api/Account/login")]
        Task<APIResponse> Login([Body(BodySerializationMethod.Serialized)] LoginRequest body);

        [Get("/api/Account/confirm-email?userId={userId}&code={code}")]
        Task<APIResponse> ConfirmEmail([Query, AliasAs("userId")] string userId, [Query, AliasAs("code")] string code);

        [Post("/api/Account/forgot-password")]
        Task ForgotPassword([Body(BodySerializationMethod.Serialized)] ForgotPasswordRequest body);

        [Post("/api/Account/reset-password")]
        Task<APIResponse> ResetPassword([Body(BodySerializationMethod.Serialized)] ResetPasswordRequest body);

        #endregion

        #region Accounts Payable

        [Get("/api/AccountsPayable/GetAccountsPayable")]
        Task<List<AccountsPayableResponse>> GetAccountsPayable([Authorize("Bearer")] string token);

        #endregion

        #region Accounts Receivable

        [Get("/api/AccountsReceivable/GetAccountsReceivable")]
        Task<List<AccountsReceivableResponse>> GetAccountsReceivable([Authorize("Bearer")] string token);

        #endregion

        #region Customer

        [Get("/api/Customer/GetCustomers")]
        Task<Common.ApiResponse<List<CustomerResponse>>> GetCustomers([Authorize("Bearer")] string token);

        [Get("/api/Customer/GetCustomer")]
        Task<Common.ApiResponse<CustomerDetailsResponse>> GetCustomer([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        [Post("/api/Customer/CreateCustomer")]
        Task<Common.ApiResponse<bool>> CreateCustomer([Body(BodySerializationMethod.Serialized)] CreateCustomerRequest body, [Authorize("Bearer")] string token);

        [Put("/api/Customer/UpdateCustomer")]
        Task<Common.ApiResponse<bool>> UpdateCustomer([Body(BodySerializationMethod.Serialized)] UpdateCustomerRequest body, [Authorize("Bearer")] string token);

        [Delete("/api/Customer")]
        Task<Common.ApiResponse<bool>> DeleteCustomer([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        #endregion

        #region Dashboard

        [Get("/api/Dashboard/GetDashboardData")]
        Task<DashboardResponse> GetDashboard([Authorize("Bearer")] string token);

        [Get("/api/Dashboard/GetDashboardDataWithTrends")]
        Task<Common.ApiResponse<DashboardResponse>> GetDashboardWithTrends([Authorize("Bearer")] string token, [Query, AliasAs("days")] int days = 30);

        #endregion

        #region Medicine

        [Get("/api/Medicine/GetMedicines")]
        Task<Common.ApiResponse<List<MedicineResponse>>> GetMedicines([Authorize("Bearer")] string token);

        [Get("/api/Medicine/GetMedicine")]
        Task<Common.ApiResponse<MedicineDetailsResponse>> GetMedicine([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        [Post("/api/Medicine/CreateMedicine")]
        Task<Common.ApiResponse<bool>> CreateMedicine([Body(BodySerializationMethod.Serialized)] CreateMedicineRequest body, [Authorize("Bearer")] string token);

        [Put("/api/Medicine/UpdateMedicine")]
        Task<Common.ApiResponse<bool>> UpdateMedicine([Body(BodySerializationMethod.Serialized)] UpdateMedicineRequest body, [Authorize("Bearer")] string token);

        [Delete("/api/Medicine")]
        Task<Common.ApiResponse<bool>> DeleteMedine([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        #endregion

        #region Purchase

        [Get("/api/Purchase/GetPurchases")]
        Task<Common.ApiResponse<List<PurchasesResponse>>> GetPurchases([Authorize("Bearer")] string token);

        [Get("/api/Purchase/GetPurchase")]
        Task<Common.ApiResponse<PurchaseDetailsResponse>> GetPurchase([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        [Post("/api/Purchase/CreatePurchase")]
        Task<Common.ApiResponse<bool>> CreatePurchase([Body(BodySerializationMethod.Serialized)] CreatePurchaseRequest body, [Authorize("Bearer")] string token);

        [Put("/api/Purchase/UpdatePurchase")]
        Task<Common.ApiResponse<bool>> UpdatePurchase([Body(BodySerializationMethod.Serialized)] UpdatePurchaseRequest body, [Authorize("Bearer")] string token);

        [Delete("/api/Purchase")]
        Task<Common.ApiResponse<bool>> DeletePurchase([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        [Get("/api/Purchase/GetPurchaseInvoice")]
        Task<byte[]> GetPurchaseInvoice([Query, AliasAs("PurchaseId")] int id, [Authorize("Bearer")] string token);

        #endregion

        #region Supplier

        [Get("/api/Supplier/GetSuppliers")]
        Task<Common.ApiResponse<List<SupplierResponse>>> GetSuppliers([Authorize("Bearer")] string token);

        [Get("/api/Supplier/GetSupplier")]
        Task<Common.ApiResponse<SupplierDetailsResponse>> GetSupplier([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        [Post("/api/Supplier/CreateSupplier")]
        Task<Common.ApiResponse<bool>> CreateSupplier([Body(BodySerializationMethod.Serialized)] CreateSupplierRequest body, [Authorize("Bearer")] string token);

        [Put("/api/Supplier/UpdateSupplier")]
        Task<Common.ApiResponse<bool>> UpdateSupplier([Body(BodySerializationMethod.Serialized)] UpdateSupplierRequest body, [Authorize("Bearer")] string token);

        [Delete("/api/Supplier")]
        Task<Common.ApiResponse<bool>> DeleteSupplier([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        #endregion

        #region Selections

        [Get("/api/Selection/GetCustomerTypes")]
        Task<List<SelectListResponse>> GetCustomerTypes([Authorize("Bearer")] string token);

        [Get("/api/Selection/GetCustomers")]
        Task<List<SelectListResponse>> GetCustomersList([Authorize("Bearer")] string token);

        [Get("/api/Selection/GetSuppliers")]
        Task<List<SelectListResponse>> GetSuppliersList([Authorize("Bearer")] string token);

        [Get("/api/Selection/GetMedicines")]
        Task<List<SelectListResponse>> GetMedicinesList([Authorize("Bearer")] string token);

        [Get("/api/Selection/GetStocks")]
        Task<List<SelectListResponse>> GetStocks([Authorize("Bearer")] string token);

        [Get("/api/Selection/GetPaymentMethods")]
        Task<List<SelectListResponse>> GetPaymentMethods([Authorize("Bearer")] string token);

        [Get("/api/Selection/GetPurchaseStatuses")]
        Task<List<SelectListResponse>> GetPurchaseStatuses([Authorize("Bearer")] string token);

        [Get("/api/Selection/GetSaleStatuses")]
        Task<List<SelectListResponse>> GetSaleStatuses([Authorize("Bearer")] string token);

        [Get("/api/Selection/GetReturnReasons")]
        Task<List<SelectListResponse>> GetReturnReasons([Authorize("Bearer")] string token);

        [Get("/api/Selection/GetReturnDestinations")]
        Task<List<SelectListResponse>> GetReturnDestinations([Authorize("Bearer")] string token);

        #endregion

        #region Stock

        [Get("/api/Stock/GetStockList")]
        Task<Common.ApiResponse<List<StockListResponse>>> GetStockList([Authorize("Bearer")] string token);

        [Get("/api/Stock/GetStockItem")]
        Task<Common.ApiResponse<StockDetailsResponse>> GetStockItem([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        [Put("/api/Stock/UpdateStock")]
        Task<Common.ApiResponse<bool>> UpdateStock([Body(BodySerializationMethod.Serialized)] UpdateStockRequest body, [Authorize("Bearer")] string token);

        [Put("/api/Stock/AddStockBatch")]
        Task<Common.ApiResponse<bool>> AddStock([Body(BodySerializationMethod.Serialized)] List<AddStockRequest> body, [Authorize("Bearer")] string token);

        [Multipart]
        [Post("/api/Stock/import")]
        Task<Common.ApiResponse<bool>> ImportStockFromExcel([AliasAs("file")] StreamPart file, [Authorize("Bearer")] string token);

        #endregion

        #region Sales

        [Get("/api/Sale/GetSales")]
        Task<List<SalesListResponse>> GetSales([Authorize("Bearer")] string token);

        [Get("/api/Sale/GetSale")]
        Task<SaleDetailsResponse> GetSale([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        [Post("/api/Sale/CreateSale")]
        Task<APIResponse> CreateSale([Body(BodySerializationMethod.Serialized)] CreateSaleRequest body, [Authorize("Bearer")] string token);

        [Put("/api/Sale/UpdateSale")]
        Task<APIResponse> UpdateSale([Body(BodySerializationMethod.Serialized)] UpdateSaleRequest body, [Authorize("Bearer")] string token);

        [Delete("/api/Sale")]
        Task<Common.ApiResponse<bool>> DeleteSale([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        [Get("/api/Sale/GetSaleInvoice")]
        Task<byte[]> GetSaleInvoice([Query, AliasAs("SaleId")] int id, [Authorize("Bearer")] string token);

        #endregion

        #region Quotations

        [Get("/api/Quotation/GetQuotations")]
        Task<List<QuotationListResponse>> GetQuotations([Authorize("Bearer")] string token);

        [Get("/api/Quotation/GetQuotation")]
        Task<QuotationDetailsResponse> GetQuotation([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        [Post("/api/Quotation/CreateQuotation")]
        Task<APIResponse> CreateQuotation([Body(BodySerializationMethod.Serialized)] CreateQuotationRequest body, [Authorize("Bearer")] string token);

        [Put("/api/Quotation/UpdateQuotation")]
        Task<APIResponse> UpdateQuotation([Body(BodySerializationMethod.Serialized)] UpdateQuotationRequest body, [Authorize("Bearer")] string token);

        [Delete("/api/Quotation")]
        Task<APIResponse> DeleteQuotation([Query, AliasAs("id")] int id, [Authorize("Bearer")] string token);

        [Get("/api/Quotation/GenerateQuotationDoc")]
        Task<byte[]> GenerateQuotationDoc([Query, AliasAs("QuotationId")] int id, [Authorize("Bearer")] string token);

        #endregion

        #region User

        [Get("/api/User/GetUsers")]
        Task<List<UserListResponse>> GetUsers([Authorize("Bearer")] string token);

        [Get("/api/User/GetUser")]
        Task<UserDetailsResponse> GetUser([Query, AliasAs("userId")] string userId, [Authorize("Bearer")] string token);

        [Post("/api/User/AddUser")]
        Task<APIResponse> AddUser([Body(BodySerializationMethod.Serialized)] CreateUserRequest body, [Authorize("Bearer")] string token);

        [Put("/api/User/UpdateUser")]
        Task<APIResponse> UpdateUser(string userId, [Body(BodySerializationMethod.Serialized)] UpdateUserRequest body, [Authorize("Bearer")] string token);

        [Post("/api/User/change-password")]
        Task<APIResponse> ChangePassword(string userId, [Body] ChangePasswordRequest body, [Authorize("Bearer")] string token);

        [Post("/api/User/forgot-password")]
        Task<APIResponse> ForgotPassword([Body] string email);

        [Post("/api/User/reset-password")]
        Task<APIResponse> ResetUserPassword([Body] ResetPasswordRequest body);

        [Delete("/api/User/DeleteUser")]
        Task<APIResponse> DeleteUser([Query, AliasAs("userId")] string userId, [Authorize("Bearer")] string token);
        #endregion

        #region Role
        [Get("/api/Role")]
        Task<List<string>> GetAllRoles([Authorize("Bearer")] string token);

        [Post("/api/Role")]
        Task<APIResponse> CreateRole([Body(BodySerializationMethod.Serialized)] AddRoleRequest body, [Authorize("Bearer")] string token);

        [Delete("/api/Role/{roleName}")]
        Task<APIResponse> DeleteRole(string roleName, [Authorize("Bearer")] string token);

        [Put("/api/Role")]
        Task<APIResponse> UpdateRole([Body(BodySerializationMethod.Serialized)] UpdateRoleRequest body, [Authorize("Bearer")] string token);

        [Get("/api/Role/{roleName}/users")]
        Task<List<UserListResponse>> GetUsersInRole(string roleName, [Authorize("Bearer")] string token);

        [Post("/api/Role/add-user-to-role")]
        Task<APIResponse> AddUserToRole([Body(BodySerializationMethod.Serialized)] AddUserToRoleRequest body, [Authorize("Bearer")] string token);

        [Post("/api/Role/remove-user-from-role")]
        Task<APIResponse> RemoveUserFromRole([Body(BodySerializationMethod.Serialized)] RemoveUserFromRoleRequest body, [Authorize("Bearer")] string token);

        [Get("/api/Role/user-roles/{userId}")]
        Task<List<string>> GetUserRoles([Query, AliasAs("UserId")] string userId, [Authorize("Bearer")] string token);
        #endregion
    }
}
