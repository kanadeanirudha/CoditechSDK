namespace Coditech.Common.API.Model
{
    public class ActionViewModel
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public object? RouteValues { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string IconClass { get; set; }
    }
}
