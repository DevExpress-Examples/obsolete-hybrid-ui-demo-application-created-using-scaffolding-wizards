using DevExpress.DevAV;
using DevExpress.DevAV.Localization;
using DevExpress.Mvvm.DataAnnotations;

namespace DevExpress.DevAV.DevAVDbDataModel {

    public class DevAVDbMetadataProvider {
        public static void BuildMetadata(MetadataBuilder<Customer> builder) {
            builder.DisplayName(DevAVDbResources.Customer);
            builder.Property(x => x.Name).DisplayName(DevAVDbResources.Customer_Name);
            builder.Property(x => x.AddressState).DisplayName(DevAVDbResources.Customer_AddressState);
            builder.Property(x => x.AddressLine).DisplayName(DevAVDbResources.Customer_AddressLine);
            builder.Property(x => x.AddressCity).DisplayName(DevAVDbResources.Customer_AddressCity);
            builder.Property(x => x.AddressZipCode).DisplayName(DevAVDbResources.Customer_AddressZipCode);
            builder.Property(x => x.Phone).DisplayName(DevAVDbResources.Customer_Phone);
            builder.Property(x => x.Fax).DisplayName(DevAVDbResources.Customer_Fax);
            builder.Property(x => x.Website).DisplayName(DevAVDbResources.Customer_Website);
            builder.Property(x => x.AnnualRevenue).DisplayName(DevAVDbResources.Customer_AnnualRevenue);
            builder.Property(x => x.TotalStores).DisplayName(DevAVDbResources.Customer_TotalStores);
            builder.Property(x => x.TotalEmployees).DisplayName(DevAVDbResources.Customer_TotalEmployees);
            builder.Property(x => x.Status).DisplayName(DevAVDbResources.Customer_Status);
            builder.Property(x => x.Logo).DisplayName(DevAVDbResources.Customer_Logo);
        }
        public static void BuildMetadata(MetadataBuilder<CustomerStore> builder) {
            builder.DisplayName(DevAVDbResources.CustomerStore);
            builder.Property(x => x.AddressState).DisplayName(DevAVDbResources.CustomerStore_AddressState);
            builder.Property(x => x.AddressLine).DisplayName(DevAVDbResources.CustomerStore_AddressLine);
            builder.Property(x => x.AddressCity).DisplayName(DevAVDbResources.CustomerStore_AddressCity);
            builder.Property(x => x.AddressZipCode).DisplayName(DevAVDbResources.CustomerStore_AddressZipCode);
            builder.Property(x => x.State).DisplayName(DevAVDbResources.CustomerStore_State);
            builder.Property(x => x.Phone).DisplayName(DevAVDbResources.CustomerStore_Phone);
            builder.Property(x => x.Fax).DisplayName(DevAVDbResources.CustomerStore_Fax);
            builder.Property(x => x.TotalEmployees).DisplayName(DevAVDbResources.CustomerStore_TotalEmployees);
            builder.Property(x => x.SquereFootage).DisplayName(DevAVDbResources.CustomerStore_SquereFootage);
            builder.Property(x => x.AnnualSales).DisplayName(DevAVDbResources.CustomerStore_AnnualSales);
            builder.Property(x => x.CrestLarge).DisplayName(DevAVDbResources.CustomerStore_CrestLarge);
            builder.Property(x => x.CrestSmall).DisplayName(DevAVDbResources.CustomerStore_CrestSmall);
            builder.Property(x => x.Location).DisplayName(DevAVDbResources.CustomerStore_Location);
            builder.Property(x => x.Customer).DisplayName(DevAVDbResources.CustomerStore_Customer);
        }
        public static void BuildMetadata(MetadataBuilder<Order> builder) {
            builder.DisplayName(DevAVDbResources.Order);
            builder.Property(x => x.InvoiceNumber).DisplayName(DevAVDbResources.Order_InvoiceNumber);
            builder.Property(x => x.PONumber).DisplayName(DevAVDbResources.Order_PONumber);
            builder.Property(x => x.OrderDate).DisplayName(DevAVDbResources.Order_OrderDate);
            builder.Property(x => x.SaleAmount).DisplayName(DevAVDbResources.Order_SaleAmount);
            builder.Property(x => x.ShippingAmount).DisplayName(DevAVDbResources.Order_ShippingAmount);
            builder.Property(x => x.TotalAmount).DisplayName(DevAVDbResources.Order_TotalAmount);
            builder.Property(x => x.ShipDate).DisplayName(DevAVDbResources.Order_ShipDate);
            builder.Property(x => x.OrderTerms).DisplayName(DevAVDbResources.Order_OrderTerms);
            builder.Property(x => x.Customer).DisplayName(DevAVDbResources.Order_Customer);
            builder.Property(x => x.Employee).DisplayName(DevAVDbResources.Order_Employee);
            builder.Property(x => x.Store).DisplayName(DevAVDbResources.Order_Store);
        }
        public static void BuildMetadata(MetadataBuilder<Employee> builder) {
            builder.DisplayName(DevAVDbResources.Employee);
            builder.Property(x => x.FirstName).DisplayName(DevAVDbResources.Employee_FirstName);
            builder.Property(x => x.LastName).DisplayName(DevAVDbResources.Employee_LastName);
            builder.Property(x => x.Prefix).DisplayName(DevAVDbResources.Employee_Prefix);
            builder.Property(x => x.HomePhone).DisplayName(DevAVDbResources.Employee_HomePhone);
            builder.Property(x => x.MobilePhone).DisplayName(DevAVDbResources.Employee_MobilePhone);
            builder.Property(x => x.Email).DisplayName(DevAVDbResources.Employee_Email);
            builder.Property(x => x.Skype).DisplayName(DevAVDbResources.Employee_Skype);
            builder.Property(x => x.BirthDate).DisplayName(DevAVDbResources.Employee_BirthDate);
            builder.Property(x => x.Picture).DisplayName(DevAVDbResources.Employee_Picture);
            builder.Property(x => x.AddressState).DisplayName(DevAVDbResources.Employee_AddressState);
            builder.Property(x => x.AddressLine).DisplayName(DevAVDbResources.Employee_AddressLine);
            builder.Property(x => x.AddressCity).DisplayName(DevAVDbResources.Employee_AddressCity);
            builder.Property(x => x.AddressZipCode).DisplayName(DevAVDbResources.Employee_AddressZipCode);
            builder.Property(x => x.Department).DisplayName(DevAVDbResources.Employee_Department);
            builder.Property(x => x.Title).DisplayName(DevAVDbResources.Employee_Title);
            builder.Property(x => x.Status).DisplayName(DevAVDbResources.Employee_Status);
            builder.Property(x => x.HireDate).DisplayName(DevAVDbResources.Employee_HireDate);
            builder.Property(x => x.PersonalProfile).DisplayName(DevAVDbResources.Employee_PersonalProfile);
            builder.Property(x => x.Order).DisplayName(DevAVDbResources.Employee_Order);
        }
        public static void BuildMetadata(MetadataBuilder<EmployeeTask> builder) {
            builder.DisplayName(DevAVDbResources.EmployeeTask);
            builder.Property(x => x.Subject).DisplayName(DevAVDbResources.EmployeeTask_Subject);
            builder.Property(x => x.Description).DisplayName(DevAVDbResources.EmployeeTask_Description);
            builder.Property(x => x.StartDate).DisplayName(DevAVDbResources.EmployeeTask_StartDate);
            builder.Property(x => x.DueDate).DisplayName(DevAVDbResources.EmployeeTask_DueDate);
            builder.Property(x => x.Status).DisplayName(DevAVDbResources.EmployeeTask_Status);
            builder.Property(x => x.Priority).DisplayName(DevAVDbResources.EmployeeTask_Priority);
            builder.Property(x => x.Completion).DisplayName(DevAVDbResources.EmployeeTask_Completion);
            builder.Property(x => x.Reminder).DisplayName(DevAVDbResources.EmployeeTask_Reminder);
            builder.Property(x => x.ReminderDateTime).DisplayName(DevAVDbResources.EmployeeTask_ReminderDateTime);
            builder.Property(x => x.AssignedEmployee).DisplayName(DevAVDbResources.EmployeeTask_AssignedEmployee);
        }
        public static void BuildMetadata(MetadataBuilder<Quote> builder) {
            builder.DisplayName(DevAVDbResources.Quote);
            builder.Property(x => x.Number).DisplayName(DevAVDbResources.Quote_Number);
            builder.Property(x => x.Date).DisplayName(DevAVDbResources.Quote_Date);
            builder.Property(x => x.SubTotal).DisplayName(DevAVDbResources.Quote_SubTotal);
            builder.Property(x => x.ShippingAmount).DisplayName(DevAVDbResources.Quote_ShippingAmount);
            builder.Property(x => x.Total).DisplayName(DevAVDbResources.Quote_Total);
            builder.Property(x => x.Opportunity).DisplayName(DevAVDbResources.Quote_Opportunity);
            builder.Property(x => x.Customer).DisplayName(DevAVDbResources.Quote_Customer);
            builder.Property(x => x.CustomerStore).DisplayName(DevAVDbResources.Quote_CustomerStore);
            builder.Property(x => x.Employee).DisplayName(DevAVDbResources.Quote_Employee);
        }
        public static void BuildMetadata(MetadataBuilder<Product> builder) {
            builder.DisplayName(DevAVDbResources.Product);
            builder.Property(x => x.Name).DisplayName(DevAVDbResources.Product_Name);
            builder.Property(x => x.Description).DisplayName(DevAVDbResources.Product_Description);
            builder.Property(x => x.ProductionStart).DisplayName(DevAVDbResources.Product_ProductionStart);
            builder.Property(x => x.Available).DisplayName(DevAVDbResources.Product_Available);
            builder.Property(x => x.Image).DisplayName(DevAVDbResources.Product_Image);
            builder.Property(x => x.CurrentInventory).DisplayName(DevAVDbResources.Product_CurrentInventory);
            builder.Property(x => x.Backorder).DisplayName(DevAVDbResources.Product_Backorder);
            builder.Property(x => x.Manufacturing).DisplayName(DevAVDbResources.Product_Manufacturing);
            builder.Property(x => x.Barcode).DisplayName(DevAVDbResources.Product_Barcode);
            builder.Property(x => x.Cost).DisplayName(DevAVDbResources.Product_Cost);
            builder.Property(x => x.SalePrice).DisplayName(DevAVDbResources.Product_SalePrice);
            builder.Property(x => x.RetailPrice).DisplayName(DevAVDbResources.Product_RetailPrice);
            builder.Property(x => x.ConsumerRating).DisplayName(DevAVDbResources.Product_ConsumerRating);
            builder.Property(x => x.Category).DisplayName(DevAVDbResources.Product_Category);
            builder.Property(x => x.Engineer).DisplayName(DevAVDbResources.Product_Engineer);
            builder.Property(x => x.Support).DisplayName(DevAVDbResources.Product_Support);
        }
    }
}