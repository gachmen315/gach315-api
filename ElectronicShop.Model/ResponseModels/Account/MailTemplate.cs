namespace ElectronicShop.Model.ResponseModels.Account
{
    public class MailTemplate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public string MailSubject { get; set; }
        public string MailContent { get; set; }
        public string Note { get; set; }
    }

    public class MailTemplates : MailTemplate
    {
        public int TotalRows { get; set; }
    }
}