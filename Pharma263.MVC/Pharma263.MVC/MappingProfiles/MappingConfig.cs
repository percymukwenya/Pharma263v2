using AutoMapper;
using Pharma263.MVC.DTOs.AccountsPayable;
using Pharma263.MVC.DTOs.AccountsReceivable;
using Pharma263.MVC.DTOs.Customer;
using Pharma263.MVC.DTOs.Medicine;
using Pharma263.MVC.DTOs.PaymentMethods;
using Pharma263.MVC.DTOs.Purchases;
using Pharma263.MVC.DTOs.Quotation;
using Pharma263.MVC.DTOs.Sales;
using Pharma263.MVC.DTOs.Stock;
using Pharma263.MVC.DTOs.Suppliers;
using Pharma263.MVC.DTOs.User;
using Pharma263.MVC.Models;
using IntegrationModels = Pharma263.Integration.Api.Models;

namespace Pharma263.MVC.MappingProfiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<IntegrationModels.Response.UserListResponse, UserDto>();
            CreateMap<IntegrationModels.Response.UserDetailsResponse, UserDto>();
            CreateMap<UserDto, UpdateUserDto>();
            CreateMap<AddUserDto, IntegrationModels.Request.CreateUserRequest>();
            CreateMap<UpdateUserDto, IntegrationModels.Request.UpdateUserRequest>();

            CreateMap<CustomerDetailsDto, UpdateCustomerDto>();
            CreateMap<MedicineDetailsDto, UpdateMedicineDto>();
            CreateMap<SupplierDetailsDto, UpdateSupplierDto>();
            CreateMap<StockDto, UpdateStockDto>();
            CreateMap<IntegrationModels.Response.StockSelectionResponse, StockSelectionDto>();

            CreateMap<DTOs.Auth.LoginDto, IntegrationModels.Request.LoginRequest>();

            CreateMap<AddCustomerDto, IntegrationModels.Request.CreateCustomerRequest>();
            CreateMap<UpdateCustomerDto, IntegrationModels.Request.UpdateCustomerRequest>();
            CreateMap<IntegrationModels.Response.CustomerResponse, CustomerDto>();
            CreateMap<IntegrationModels.Response.CustomerDetailsResponse, CustomerDetailsDto>();

            CreateMap<AddMedicineDto, IntegrationModels.Request.CreateMedicineRequest>();
            CreateMap<UpdateMedicineDto, IntegrationModels.Request.UpdateMedicineRequest>();
            CreateMap<IntegrationModels.Response.MedicineResponse, MedicineDto>();
            CreateMap<IntegrationModels.Response.MedicineResponse, GetMedicineListDto>();
            CreateMap<IntegrationModels.Response.MedicineDetailsResponse, MedicineDetailsDto>();

            CreateMap<AddSupplierDto, IntegrationModels.Request.CreateSupplierRequest>();
            CreateMap<UpdateSupplierDto, IntegrationModels.Request.UpdateSupplierRequest>();
            CreateMap<IntegrationModels.Response.SupplierResponse, SupplierDto>();
            CreateMap<IntegrationModels.Response.SupplierResponse, GetSupplierListDto>();
            CreateMap<IntegrationModels.Response.SupplierDetailsResponse, SupplierDetailsDto>();

            CreateMap<AddPurchaseDto,  IntegrationModels.Request.CreatePurchaseRequest>();
            CreateMap<PurchaseItemsDto, IntegrationModels.PurchaseItemModel>().ReverseMap();
            CreateMap<UpdatePurchaseDto, IntegrationModels.Request.UpdatePurchaseRequest>();
            CreateMap<IntegrationModels.Response.PurchasesResponse, PurchaseDto>();
            CreateMap<IntegrationModels.Response.PurchaseDetailsResponse, PurchaseDetailsDto>();

            CreateMap<IntegrationModels.Response.LowStockModel, LowStock>()
                .ForMember(dest => dest.MedicineName, opt => opt.MapFrom(src => src.Name));

            CreateMap<IntegrationModels.Response.SelectListResponse, ListItemDto>();

            CreateMap<IntegrationModels.Response.StockListResponse, StockDto>();
            CreateMap<IntegrationModels.Response.StockDetailsResponse, StockDto>();
            CreateMap<UpdateStockDto, IntegrationModels.Request.UpdateStockRequest>();

            CreateMap<AddSaleDto, IntegrationModels.Request.CreateSaleRequest>();
            CreateMap<SalesItemDto, IntegrationModels.SaleItemModel>().ReverseMap();
            CreateMap<UpdateSaleDto, IntegrationModels.Request.UpdateSaleRequest>();
            CreateMap<IntegrationModels.Response.SalesListResponse, SaleDto>();
            CreateMap<IntegrationModels.Response.SaleDetailsResponse, SaleDetailsDto>();
            CreateMap<IntegrationModels.Response.SalesListResponse, StockSelectionDto>();
            CreateMap<IntegrationModels.SaleItemModel, GetSalesItemDto>();

            CreateMap<AddQuotationDto, IntegrationModels.Request.CreateQuotationRequest>();
            CreateMap<QuotationItemDto, IntegrationModels.QuotationItemModel>().ReverseMap();
            CreateMap<UpdateQuotationDto, IntegrationModels.Request.UpdateQuotationRequest>();
            CreateMap<IntegrationModels.Response.QuotationListResponse, QuotationDto>();
            CreateMap<IntegrationModels.Response.QuotationDetailsResponse, QuotationDetailsDto>();
            CreateMap<IntegrationModels.QuotationItemModel, GetQuotationItemDto>();
            // QuotationItemModel -> QuotationItems

            CreateMap<IntegrationModels.Response.AccountsPayableResponse, GetAccountsPayableListDto>();

            CreateMap<IntegrationModels.Response.AccountsReceivableResponse, GetAccountsReceivableListDto>();
        }
    }
}
