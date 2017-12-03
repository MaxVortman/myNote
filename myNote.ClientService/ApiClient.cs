namespace myNote.ClientService
{
    public class ApiClient
    {
        #region Private properties

        private static ApiClient instance;
        public UserService UserService { get; }
        public ShareService ShareService { get; }
        public NoteService NoteService { get; }
        public NoteGroupService NoteGroupService { get; }
        public LoginService LoginService { get; }
        public GroupService GroupService { get; }

        #endregion
        
        #region Default private constructor

        private ApiClient(string connectionString)
        {
            UserService = new UserService(connectionString);
            NoteGroupService = new NoteGroupService(connectionString);
            NoteService = new NoteService(connectionString);
            GroupService = new GroupService(connectionString);
            ShareService = new ShareService(connectionString);
            LoginService = new LoginService(connectionString);
        }

        #endregion

        #region Create Instance

        public static ApiClient CreateInstance(string connectionString)
        {
            if(instance == null)
                instance = new ApiClient(connectionString);
            return instance;
        }

        #endregion
    }
}