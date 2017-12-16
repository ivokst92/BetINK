namespace BetINK.Web.Areas.Admin.Models.Shared
{
    public class DeleteConfirmationViewModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string CancelPath { get; set; }
        public string DestroyPath { get; set; }
    }
}
