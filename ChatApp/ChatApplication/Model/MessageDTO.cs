namespace ChatApplication.ChatServiceReference
{
    public partial class MessageDTO
    {
        public override string ToString()
        {
            return "["+Timestamp+"]" + "<" + User.Name + "> " + MessageText;
        }
    }
}
