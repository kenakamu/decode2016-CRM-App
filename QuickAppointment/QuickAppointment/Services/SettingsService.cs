namespace QuickAppointment.Services
{
    static class SettingsService
    {
        // Dynamics CRM 関連
        public const string crmApiBase = "https://dcd16prd7.crm7.dynamics.com/api/data/v8.1/";
        public const string crmResource = "https://dcd16prd7.crm7.dynamics.com";
        // SharePoint Online 関連
        public const string spoApiBase = "https://dcd16prd7.sharepoint.com/_api/web/";
        public const string spoResource = "https://dcd16prd7.sharepoint.com";
        // ADAL 関連
        public const string ClientId = "3e76a783-2d6e-473d-8b7f-ba25368e4265";
        public const string AuthUri = "https://login.windows.net/dcd16prd7.onmicrosoft.com";
        public const string RedirectUri = "http://localhost/decode";
    }
}
