namespace GroupProject.BankDatabase
{
    [Serializable]
    public class UserStorage
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public UserStorage() { }
        public UserStorage(string userName, string userId)
        {
            UserName = userName;
            UserId = userId;
        }
    }
}
